using System;
using System.Linq;
using Nuke.Common.ProjectModel;

static class ProjectExtensions
{
    const string ExtDll = ".dll";
    const string TestProjectNameSuffix = ".Tests";

    public static bool IsPackable(this Project project) => project.GetProperty<bool>("IsPackable");

    public static string GetDefaultTargetFramework(this Project project) =>
        project.GetTargetFrameworks()!.First();

    public static string GetAssemblyName(this Project project)
    {
        return project.Name + ExtDll;
    }

    public static string GetSrcProjectName(this Project project)
    {
        if (!project.Name.EndsWith(TestProjectNameSuffix))
        {
            throw new ArgumentException(
                $"Given project name does not end with '{TestProjectNameSuffix}'",
                nameof(project)
            );
        }

        var mainProjectNameLength = project.Name.Length - TestProjectNameSuffix.Length;
        return project.Name[..mainProjectNameLength];
    }
}
