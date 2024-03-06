using System.Linq;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Serilog;

interface ICompile : IHazSlnFiles, IHazArtifacts, IHazConfiguration, IRestore, IHazVersion
{
    Target Compile => _ => _.DependsOn(CompileMain, CompileTests);

    Target CompileMain =>
        _ =>
            _.DependsOn(Restore)
                .Executes(
                    () =>
                        Log.Information(
                            "Building projects: {@Projects}",
                            MainProjects.Select(x => x.Name)
                        ),
                    () => Log.Information("Version: {Version}", Version.SemVer),
                    () =>
                        DotNetTasks.DotNetBuild(
                            settings =>
                                settings
                                    .Apply(BuildSettingsBase)
                                    .CombineWith(
                                        MainProjects,
                                        (settings, project) => settings.SetProjectFile(project)
                                    )
                        )
                );

    Target CompileTests =>
        _ =>
            _.DependsOn(Restore)
                .DependsOn(CompileMain)
                .Executes(
                    () =>
                        DotNetTasks.DotNetBuild(
                            settings =>
                                settings
                                    .Apply(BuildSettingsBase)
                                    .CombineWith(
                                        TestProjects,
                                        (settings, project) => settings.SetProjectFile(project)
                                    )
                        )
                );

    Configure<DotNetBuildSettings> BuildSettingsBase =>
        settings =>
            settings
                .EnableNoRestore()
                .EnableNoDependencies()
                .SetConfiguration(Configuration)
                .SetVersion(Version.SemVer);
}