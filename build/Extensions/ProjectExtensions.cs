using System.Linq;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

static class ProjectExtensions
{
    public static bool IsPackable(this Project project) => project.GetProperty<bool>("IsPackable");

    public static AbsolutePath GetBuildDirectory(
        this Project project,
        Configuration configuration,
        string targetFramework = null
    ) =>
        project.Directory
        / "bin"
        / configuration
        / (targetFramework ?? project.GetDefaultTargetFramework());

    public static AbsolutePath GetAssemblyOutputPath(
        this Project project,
        Configuration configuration,
        string targetFramework = null
    ) => project.GetBuildDirectory(configuration, targetFramework) / $"{project.Name}.dll";

    public static string GetDefaultTargetFramework(this Project project) =>
        project.GetTargetFrameworks()!.First();
}
