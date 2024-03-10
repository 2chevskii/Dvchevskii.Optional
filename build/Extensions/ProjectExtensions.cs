using Nuke.Common.ProjectModel;

static class ProjectExtensions
{
    public static bool IsPackable(this Project project) => project.GetProperty<bool>("IsPackable");
}
