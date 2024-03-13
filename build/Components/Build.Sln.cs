using System.Collections.Generic;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

partial class Build
{
    [Solution(SuppressBuildProjectCheck = true)]
    readonly Solution Sln;
    IReadOnlyCollection<Project> SrcProjects => Sln.GetSolutionFolder("src")!.Projects;
    IReadOnlyCollection<Project> TestProjects => Sln.GetSolutionFolder("test")!.Projects;

    public AbsolutePath GetProjectBinConfigurationDirectory(Project project)
    {
        return project.Directory / Constants.BIN / Configuration;
    }

    public AbsolutePath GetProjectBuildDirectory(Project project, string targetFramework = null)
    {
        targetFramework ??= project.GetDefaultTargetFramework();

        return GetProjectBinConfigurationDirectory(project) / targetFramework;
    }

    public AbsolutePath GetProjectAssemblyOutputPath(Project project, string targetFramework = null)
    {
        return GetProjectBuildDirectory(project, targetFramework) / project.GetAssemblyName();
    }
}

file static class Constants
{
    public const string BIN = "bin";
}
