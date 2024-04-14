// ReSharper disable AllUnderscoreLocalParameterName
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;

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
                                GetProjectBuildDirectory(project)
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
