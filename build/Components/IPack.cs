using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

interface IPack : ICompile
{
    Target Pack =>
        _ =>
            _.DependsOn(Compile)
                .Executes(
                    () =>
                        DotNetTasks.DotNetPack(
                            settings =>
                                settings
                                    .EnableNoBuild()
                                    .SetVersion(Version.SemVer)
                                    .SetConfiguration(Configuration)
                                    .SetProcessWorkingDirectory(Sln.Directory)
                                    .CombineWith(
                                        MainProjects,
                                        (settings, project) => settings.SetProject(project)
                                    )
                        )
                );
}
