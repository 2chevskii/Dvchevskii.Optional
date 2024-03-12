using System.Linq;
using LibGit2Sharp;
using Nuke.Common;
using Serilog;

partial class Build
{
    Repository Repository;

    Target CreateReleaseTag =>
        _ =>
            _.Executes(async () =>
            {
                Log.Information(
                    "Repository tags: {Tags}",
                    Repository.Tags.Select(tag => tag.CanonicalName)
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
