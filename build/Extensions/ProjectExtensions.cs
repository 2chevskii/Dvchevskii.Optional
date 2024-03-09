using Nuke.Common.ProjectModel;

static class ProjectExtensions
{
    public static string ShortName(this Project project) => project.GetProperty("ShortName");
}
