// ReSharper disable AllUnderscoreLocalParameterName
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;

partial class Build
{
    [Parameter]
    readonly bool CleanAllConfigurations;

    Target CleanSln =>
        _ =>
            _.Executes(
                () =>
                    DotNetTasks.DotNetClean(
                        settings =>
                            settings
                                .SetProject(Sln)
                                .CombineWith(
                                    CleanAllConfigurations ? Configuration.All : [Configuration],
                                    (settings, configuration) =>
                                        settings.SetConfiguration(configuration)
                                )
                    )
            );

    Target CleanArtifacts =>
        _ =>
            _.Executes(
                () => ArtifactPaths.All.ForEach(directory => directory.CreateOrCleanDirectory())
            );
}
