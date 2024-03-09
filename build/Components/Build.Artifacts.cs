using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using Serilog;

partial class Build
{
    ArtifactPathCollection ArtifactPaths => new(this);

    Target ArtifactsLib =>
        _ =>
            _.DependsOn(CompileSrc)
                .OnlyWhenStatic(() => Configuration == Configuration.Release)
                .Executes(
                    () =>
                        SrcProjects.ForEach(
                            project =>
                                ArtifactPaths
                                    .GetProjectBuildDirectory(project)
                                    .ZipTo(ArtifactPaths.GetProjectBuildArchivePath(project))
                        )
                );

    Target ArtifactsPkg =>
        _ =>
            _.DependsOn(Pack)
                .OnlyWhenStatic(() => Configuration == Configuration.Release)
                .Executes(
                    () =>
                        SrcProjects.ForEach(project =>
                        {
                            var nupkg = ArtifactPaths.GetProjectNuGetPackagePath(project);
                            var snupkg = ArtifactPaths.GetProjectNuGetSymbolPackagePath(project);

                            var nupkgTarget = ArtifactPaths.GetProjectNuGetPackageArtifactPath(
                                project
                            );
                            var snupkgTarget =
                                ArtifactPaths.GetProjectNuGetSymbolPackageArtifactPath(project);

                            FileSystemTasks.CopyFile(
                                nupkg,
                                nupkgTarget,
                                FileExistsPolicy.Overwrite
                            );
                            FileSystemTasks.CopyFile(
                                snupkg,
                                snupkgTarget,
                                FileExistsPolicy.Overwrite
                            );
                        })
                );

    void CreateArtifactDirectories() =>
        ArtifactPaths.All.ForEach(directory => directory.CreateDirectory());
}

partial class Build
{
    Target Test =>
        _ =>
            _.DependsOn(CompileTests)
                .Executes(
                    () => Log.Information("Running tests using projects: {Projects}", TestProjects),
                    () =>
                        DotNetTasks.DotNetTest(
                            settings =>
                                settings
                                    .Apply(TestSettingsBase)
                                    .CombineWith(
                                        TestProjects,
                                        (settings, project) => settings.SetProjectFile(project)
                                    )
                        )
                );

    Configure<DotNetTestSettings> TestSettingsBase =>
        _ => _.EnableNoBuild().SetConfiguration(Configuration);
}
