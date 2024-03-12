using System;
using System.Linq;
using LibGit2Sharp;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Serilog;

partial class Build
{
    Repository Repository;

    Target CreateReleaseTag =>
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

    void LoadRepository()
    {
        Log.Information(
            "Loading Git repository from {GitRepositoryDirectory} directory",
            RootDirectory
        );
        Repository = new Repository(RootDirectory);
    }

    void DisposeRepository()
    {
        Log.Information("Disposing Git repository");
        Repository.Dispose();
    }
}
