using System.Collections.Generic;
using System.Linq;
using Nuke.Common.ProjectModel;

static class EnumerableExtensions
{
    public static IEnumerable<string> Names(this IEnumerable<Project> projects) =>
        projects.Select(project => project.Name);

    public static IEnumerable<Project> Packable(this IEnumerable<Project> projects) =>
        projects.Where(project => project.IsPackable());
}
