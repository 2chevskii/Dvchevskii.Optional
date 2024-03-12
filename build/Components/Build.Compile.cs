using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Serilog;

[SuppressMessage("ReSharper", "AllUnderscoreLocalParameterName")]
partial class Build
{
    Target Compile => _ => _.DependsOn(CompileSrc, CompileTests);

    Target CompileSrc =>
        _ =>
            _.DependsOn(Restore)
                .Executes(
                    () => Log.Information("Building SRC projects: {Projects}", SrcProjects.Names()),
                    () => Log.Information("Version: {Version}", Version.SemVer),
                    () =>
                        DotNetTasks.DotNetBuild(
                            settings =>
                                settings
                                    .Apply(BuildSettingsBase)
                                    .CombineWith(
                                        SrcProjects,
                                        (settings, project) => settings.SetProjectFile(project)
                                    )
                        )
                );

    Target CompileTests =>
        _ =>
            _.DependsOn(Restore)
                .DependsOn(CompileSrc)
                .Executes(
                    () =>
                        Log.Information("Building TEST projects: {Projects}", TestProjects.Names()),
                    () => Log.Information("Version: {Version}", Version.SemVer),
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
                .SetVersion(Version.SemVer)
                .SetConfiguration(Configuration);
}
