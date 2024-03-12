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

    Task<long> GetRepositoryId() =>
        RepositoryId != 0L
            ? Task.FromResult(RepositoryId)
            : GitHubTasks
                .GitHubClient.Repository.Get(
                    GitRepository.GetGitHubOwner(),
                    GitRepository.GetGitHubName()
                )
                .ContinueWith(task => RepositoryId = task.Result.Id);

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
