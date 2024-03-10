using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Serilog;

partial class Build
{
    Target Pack =>
        _ =>
            _.DependsOn(CompileSrc)
                .Executes(
                    () =>
                        Log.Information(
                            "Creating NuGet packages from projects: {Projects}",
                            SrcProjects
                        ),
                    () =>
                        DotNetTasks.DotNetPack(
                            settings =>
                                settings
                                    .Apply(PackSettingsBase)
                                    .CombineWith(
                                        SrcProjects.Packable(),
                                        (settings, project) => settings.SetProject(project)
                                    )
                        )
                );

    Configure<DotNetPackSettings> PackSettingsBase =>
        settings =>
            settings
                .EnableNoBuild()
                .EnableNoDependencies()
                .SetVersion(Version.SemVer)
                .SetConfiguration(Configuration);
}
