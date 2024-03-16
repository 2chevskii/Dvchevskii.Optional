using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibGit2Sharp;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitHub;
using Octokit;
using Serilog;

// ReSharper disable AllUnderscoreLocalParameterName

partial class Build
{
    [Parameter]
    readonly string ReleaseAssets;

    [Parameter]
    readonly string ReleaseTagName;
    AbsolutePath ReleaseAssetsDirectory => RootDirectory / ReleaseAssets;

    Target CreateReleaseDraft =>
        _ =>
            _.OnlyWhenDynamic(() => GitHubActions.Instance.RefType == "tag")
                .Requires(() => !string.IsNullOrEmpty(GitHubActions.Instance.Token))
                .Executes(async () =>
                {
                    var tagName = GitHubActions.Instance.RefName;
                    Log.Information("Creating release draft for tag {TagName}", tagName);

                    var githubRepositoryId = await GetRepositoryId();
                    Log.Verbose("GitHub repository Id is {RepositoryId}", githubRepositoryId);

                    var releaseName = tagName.TrimStart('v', 'V');

                    Release release = await GitHubTasks.GitHubClient.Repository.Release.Create(
                        githubRepositoryId,
                        new NewRelease(tagName)
                        {
                            Draft = true,
                            Name = releaseName,
                            // we consider release a pre-release
                            // when source tag has pre-release label in it
                            Prerelease = tagName.Contains('-'),
                            Body = "## TODO: Write release notes here"
                        }
                    );

                    Log.Information("Searching for assets to upload...");
                    var assetsToUpload = GetReleaseAssets();

                    if (assetsToUpload.Count == 0)
                    {
                        Log.Warning(
                            "Release does not seem to contain any assets, ReleaseAssets parameter was: {ReleaseAssetsParam}",
                            ReleaseAssets
                        );
                    }
                    else
                    {
                        Log.Information(
                            "Found {AssetCount} release assets to upload",
                            assetsToUpload.Count
                        );
                        Log.Verbose("Artifacts prepared for upload: {AssetPaths}", assetsToUpload);

                        var uploadTasks = assetsToUpload.Select(assetPath =>
                        {
                            Log.Debug("Uploading asset: {AssetPath}", assetPath);
                            var filename = assetPath.Name;
                            var stream = File.OpenRead(assetPath);

                            var assetUpload = new ReleaseAssetUpload
                            {
                                FileName = filename,
                                ContentType = Constants.ReleaseAssetContentType,
                                RawData = stream
                            };

                            var uploadTask = GitHubTasks
                                .GitHubClient.Repository.Release.UploadAsset(release, assetUpload)
                                .ContinueWith(_ =>
                                {
                                    stream.Dispose();
                                    Log.Debug("Finished asset upload: {AssetPath}", assetPath);
                                });

                            return uploadTask;
                        });

                        Log.Information("Waiting for all the asset upload tasks to finish...");
                        await Task.WhenAll(uploadTasks);
                        Log.Information("Release assets upload finished");
                    }
                });

    Target CreateVersionTag =>
        _ =>
            _.Executes(() =>
            {
                var tagName = $"v{Version.SemVer}";
                var tag = Repository.ApplyTag(tagName);

                Log.Information(
                    "Creating tag {TagName} on commit {CommitSha}",
                    tagName,
                    Repository.Head.Tip.Sha[..7]
                );

                Remote remote = Repository.Network.Remotes["origin"];

                Log.Debug(
                    "Pushing created tag to remote {RemoteName} at {RemoteUrl}",
                    remote.Name,
                    remote.PushUrl
                );

                Repository.Network.Push(
                    remote,
                    tag.CanonicalName,
                    new PushOptions
                    {
                        CredentialsProvider = (_, _, _) =>
                            new UsernamePasswordCredentials
                            {
                                Username = GitHubActions.Instance.RepositoryOwner,
                                Password = GitHubActions.Instance.Token
                            }
                    }
                );
            });

    Target DownloadReleaseAssets =>
        _ =>
            _.Requires(() => !string.IsNullOrEmpty(ReleaseTagName))
                .Executes(async () =>
                {
                    var repoId = await GetRepositoryId();
                    var release = await GitHubTasks.GitHubClient.Repository.Release.Get(
                        repoId,
                        ReleaseTagName
                    );

                    Log.Information(
                        "Got release {ReleaseName} on tag {TagName}",
                        release.Name,
                        release.TagName
                    );

                    Log.Verbose(
                        "Release assets: {Assets}",
                        release.Assets.Select(asset => asset.Name)
                    );

                    ReleaseAssetsDirectory.CreateOrCleanDirectory();

                    Log.Information(
                        "Downloading {AssetCount} assets to directory {ReleaseAssetDirectory}",
                        release.Assets.Count,
                        ReleaseAssetsDirectory
                    );

                    var downloadTasks = release.Assets.Select(
                        asset =>
                            HttpTasks.HttpDownloadFileAsync(
                                asset.BrowserDownloadUrl,
                                ReleaseAssetsDirectory / asset.Name
                            )
                    );

                    await Task.WhenAll(downloadTasks);
                });

    IReadOnlyCollection<AbsolutePath> GetReleaseAssets()
    {
        if (string.IsNullOrEmpty(ReleaseAssets))
        {
            return Array.Empty<AbsolutePath>();
        }

        return ReleaseAssetsDirectory.GlobFiles("*.{nupkg,snupkg}", "*.zip", "*.tar", "*.gz");
    }
}

file static class Constants
{
    public const string ReleaseAssetContentType = "application/octet-stream";
}
