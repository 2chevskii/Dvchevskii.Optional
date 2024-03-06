using System.Collections.Generic;
using Nuke.Common;
using Nuke.Common.ProjectModel;

interface IHazSlnFiles : INukeBuild
{
    [Solution, Required]
    Solution Sln => TryGetValue(() => Sln);
    IReadOnlyCollection<Project> MainProjects => Sln.GetSolutionFolder("src")!.Projects;
    IReadOnlyCollection<Project> TestProjects => Sln.GetSolutionFolder("test")!.Projects;
}
