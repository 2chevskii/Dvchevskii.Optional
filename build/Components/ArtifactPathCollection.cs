using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;

sealed class ArtifactPathCollection(Build build)
{
    const string DIRNAME_BIN = "bin";

    AbsolutePath BuildRootDirectory => ((INukeBuild)build).RootDirectory;
    public AbsolutePath RootDirectory => BuildRootDirectory / "artifacts";
    public AbsolutePath PackagesDirectory => RootDirectory / "pkg";
    public AbsolutePath LibrariesDirectory => RootDirectory / "lib";
    public AbsolutePath TestResults => RootDirectory / "test_results";
    public AbsolutePath Coverage => TestResults / "coverage";
    public AbsolutePath DocsDirectory => RootDirectory / "docs";

    public AbsolutePath[] All =>
        [
            RootDirectory,
            PackagesDirectory,
            LibrariesDirectory,
            TestResults,
            Coverage,
            DocsDirectory
        ];

    Configuration CurrentConfiguration => build.Configuration;
    string Version => build.Version.SemVer;

    public AbsolutePath GetProjectBuildDirectory(
        Project project,
        Configuration configuration = null,
        string targetFramework = null
    )
    {
        configuration ??= CurrentConfiguration;
        targetFramework ??= GetDefaultTargetFramework(project);

        return project.Directory / DIRNAME_BIN / configuration / targetFramework;
    }

    public AbsolutePath GetProjectBuildArchivePath(Project project, string targetFramework = null)
    {
        targetFramework ??= GetDefaultTargetFramework(project);

        string archiveName = $"{project.Name}.{Version}_{targetFramework}.zip";

        return LibrariesDirectory / archiveName;
    }

    public AbsolutePath GetProjectNuGetPackagePath(
        Project project,
        Configuration configuration = null,
        string version = null
    )
    {
        configuration ??= CurrentConfiguration;
        version ??= Version;

        return project.Directory / DIRNAME_BIN / configuration / $"{project.Name}.{version}.nupkg";
    }

    public AbsolutePath GetProjectNuGetSymbolPackagePath(
        Project project,
        Configuration configuration = null,
        string version = null
    )
    {
        configuration ??= CurrentConfiguration;
        version ??= Version;

        return project.Directory / DIRNAME_BIN / configuration / $"{project.Name}.{version}.snupkg";
    }

    public AbsolutePath GetProjectNuGetPackageArtifactPath(Project project, string version = null)
    {
        version ??= Version;

        return PackagesDirectory / $"{project.Name}.{version}.nupkg";
    }

    public AbsolutePath GetProjectNuGetSymbolPackageArtifactPath(
        Project project,
        string version = null
    )
    {
        version ??= Version;

        return PackagesDirectory / $"{project.Name}.{version}.snupkg";
    }

    string GetDefaultTargetFramework(Project project) => project.GetTargetFrameworks()!.First();
}

partial class Build
{
    Target CleanArtifacts =>
        _ =>
            _.Executes(
                () => ArtifactPaths.All.ForEach(directory => directory.CreateOrCleanDirectory())
            );
}
