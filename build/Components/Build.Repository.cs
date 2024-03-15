using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LibGit2Sharp;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitHub;
using Serilog;

partial class Build
{
    long RepositoryId;
    Repository Repository;

    [GitRepository, Required]
    GitRepository GitRepository;

    async Task<long> GetRepositoryId()
    {
        if (RepositoryId == 0L)
        {
            Log.Verbose(
                "Fetching RepositoryId for repository {GitHubOwner}/{GitHubName}",
                GitRepository.GetGitHubOwner(),
                GitRepository.GetGitHubName()
            );

            RepositoryId = (
                await GitHubTasks.GitHubClient.Repository.Get(
                    GitRepository.GetGitHubOwner(),
                    GitRepository.GetGitHubName()
                )
            ).Id;

            Log.Verbose("RepositoryId: {RepositoryId}", RepositoryId);
        }

        return RepositoryId;
    }

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
