// ReSharper disable AllUnderscoreLocalParameterName

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitReleaseManager;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using Octokit;
using Octokit.Internal;
using Serilog;
using Credentials = Octokit.Credentials;
using FileMode = System.IO.FileMode;
using Repository = LibGit2Sharp.Repository;

class Build : NukeBuild
{
    const string NugetOrgSource = "https://api.nuget.org/v3/index.json";

    [Nuke.Common.Parameter(
        "Configuration to build - Default is 'Debug' (local) or 'Release' (server)"
    )]
    readonly Configuration Configuration = Configuration.Debug;

    readonly string NugetApiKey = EnvironmentInfo.GetVariable("NUGET_API_KEY");

    [GitVersion]
    readonly GitVersion Version;

    IGitHubClient GitHubClient;

    string TagName => $"v{Version.SemVer}";

    Target ActionsSetSemver =>
        _ =>
            _.OnlyWhenDynamic(() => Host is GitHubActions)
                .Executes(() =>
                {
                    string versionString = Version.SemVer;
                    GitHubActions.Instance.WriteDebug(
                        $"Appending calculated semantic version to GitHub environment: {versionString}"
                    );
                    AbsolutePath githubEnvPath = EnvironmentInfo.GetVariable("GITHUB_ENV");
                    githubEnvPath.AppendAllLines(new[] { "GITVERSION_SEMVER=" + versionString });
                });

    Target Clean =>
        _ =>
            _.Executes(
                () => DotNetTasks.DotNetClean(settings => settings.SetConfiguration(Configuration))
            );

    Target CleanAll =>
        _ =>
            _.Executes(
                () =>
                    DotNetTasks.DotNetClean(
                        settings => settings.SetConfiguration(Configuration.Debug)
                    ),
                () =>
                    DotNetTasks.DotNetClean(
                        settings => settings.SetConfiguration(Configuration.Release)
                    )
            );

    Target Restore => _ => _.Executes(() => DotNetTasks.DotNetRestore());

    Target CompileMain =>
        _ =>
            _.DependsOn(Restore)
                .Executes(() =>
                {
                    Log.Information("Compiling Dvchevskii.Optional:{Configuration}", Configuration);
                    DotNetTasks.DotNetBuild(
                        settings =>
                            settings
                                .SetProjectFile(
                                    RootDirectory
                                        / "src/Dvchevskii.Optional/Dvchevskii.Optional.csproj"
                                )
                                .SetConfiguration(Configuration)
                                .SetVersion(Version.SemVer)
                                .EnableNoRestore()
                    );
                });

    Target CompileTests =>
        _ =>
            _.DependsOn(Restore, CompileMain)
                .Executes(() =>
                {
                    Log.Information(
                        "Compiling Dvchevskii.Optional.Tests:{Configuration}",
                        Configuration
                    );
                    DotNetTasks.DotNetBuild(
                        settings =>
                            settings
                                .SetProjectFile(
                                    RootDirectory
                                        / "test/Dvchevskii.Optional.Tests/Dvchevskii.Optional.Tests.csproj"
                                )
                                .SetConfiguration(Configuration)
                                .EnableNoRestore()
                                .EnableNoDependencies()
                    );
                });

    Target Compile => _ => _.DependsOn(CompileMain, CompileTests);

    Target UnitTest =>
        _ =>
            _.DependsOn(Compile)
                .Executes(
                    () =>
                        DotNetTasks.DotNetTest(
                            c =>
                                c.SetProjectFile(
                                        RootDirectory
                                            / "test/Dvchevskii.Optional.Tests/Dvchevskii.Optional.Tests.csproj"
                                    )
                                    .SetConfiguration(Configuration)
                                    .EnableNoRestore()
                                    .EnableNoBuild()
                                    .AddLoggers(
                                        "console;verbosity=detailed",
                                        "html;logfilename=test-results.html"
                                    )
                        )
                );

    Target Pack =>
        _ =>
            _.DependsOn(CompileMain)
                .Executes(
                    () =>
                        DotNetTasks.DotNetPack(
                            c =>
                                c.SetConfiguration(Configuration)
                                    .EnableNoRestore()
                                    .EnableNoBuild()
                                    .EnableNoDependencies()
                                    .SetVersion(Version.SemVer)
                        )
                );

    Target GetSemver =>
        _ => _.Executes(() => Log.Information("Semantic version is: {SemVer}", Version.SemVer));

    Target CreateVersionTag =>
        _ =>
            _.Executes(() =>
            {
                using Repository repository = GetRepository();

                if (repository.Head.FriendlyName != "master")
                {
                    throw new Exception(
                        "Failed to create next version tag: Not on 'master' branch"
                    );
                }

                Tag tag = repository.ApplyTag(TagName);

                PushOptions pushOptions = new PushOptions
                {
                    CredentialsProvider = (_, usernameFromUrl, _) =>
                        new UsernamePasswordCredentials
                        {
                            Username = GitHubActions.Instance.RepositoryOwner,
                            Password = GitHubActions.Instance.Token
                        }
                };

                Remote remote = repository.Network.Remotes["origin"];
                repository.Network.Push(remote, tag.CanonicalName, pushOptions);
            });

    Target CreateReleaseDraft =>
        _ =>
            _.Executes(async () =>
            {
                using Repository repository = GetRepository();
                Tag tag = repository.Tags.FirstOrDefault(
                    x => x.PeeledTarget.Sha == repository.Head.Tip.Sha
                );
                if (tag == null)
                {
                    throw new Exception("Tag was not found");
                }

                string releaseName = tag.FriendlyName.StartsWith(
                    "v",
                    StringComparison.OrdinalIgnoreCase
                )
                    ? tag.FriendlyName.Substring(1)
                    : tag.FriendlyName;

                IReadOnlyCollection<AbsolutePath> packageAssets = (
                    RootDirectory / "artifacts/packages"
                ).GlobFiles("*.*nupkg");
                AbsolutePath libraryAsset =
                    RootDirectory / "artifacts/libraries/netstandard2.0.zip";
                (RootDirectory / "artifacts/libraries/netstandard2.0").ZipTo(libraryAsset);

                Release release = await GitHubClient.Repository.Release.Create(
                    GitHubActions.Instance.RepositoryOwner,
                    GitHubActions.Instance.Repository.Substring(
                        GitHubActions.Instance.Repository.IndexOf('/') + 1
                    ),
                    new NewRelease(tag.FriendlyName)
                    {
                        Name = releaseName,
                        Draft = true,
                        TargetCommitish = "master"
                    }
                );

                foreach (AbsolutePath assetPath in packageAssets.Append(libraryAsset))
                {
                    await using FileStream fs = new FileStream(
                        assetPath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read
                    );

                    ReleaseAssetUpload upload = new ReleaseAssetUpload(
                        assetPath.Name,
                        "application/octet-stream",
                        fs,
                        null
                    );
                    ReleaseAsset _ = await GitHubClient.Repository.Release.UploadAsset(
                        release,
                        upload
                    );
                }
            });

    public static int Main() => Execute<Build>(x => x.Compile);

    Repository GetRepository()
    {
        return new Repository(RootDirectory);
    }

    protected override void OnBuildInitialized()
    {
        if (!IsLocalBuild && !string.IsNullOrEmpty(GitHubActions.Instance.Token))
        {
            GitHubClient = new GitHubClient(
                new ProductHeaderValue("Dvchevskii.Optional"),
                new InMemoryCredentialStore(
                    new Credentials(GitHubActions.Instance.Token, AuthenticationType.Bearer)
                )
            );
        }
    }
}
