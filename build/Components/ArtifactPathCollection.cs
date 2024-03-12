using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

sealed class ArtifactPathCollection(Build build)
{
    AbsolutePath BuildRootDirectory => ((INukeBuild)build).RootDirectory;
    public AbsolutePath RootDirectory => BuildRootDirectory / "artifacts";
    public AbsolutePath PackagesDirectory => RootDirectory / "pkg";
    public AbsolutePath LibrariesDirectory => RootDirectory / "lib";
    public AbsolutePath TestResults => RootDirectory / "test_results";
    public AbsolutePath Coverage => TestResults / "coverage";
    public AbsolutePath CoverageHtmlReport => Coverage / "html";
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

    string Version => build.Version.SemVer;

    public string GetProjectPackageName(Project project, string extension = null) =>
        $"{project.Name}.{Version}{(extension != null ? "." + extension : string.Empty)}";

    public AbsolutePath GetProjectBuildArchivePath(Project project, string targetFramework = null)
    {
        targetFramework ??= GetDefaultTargetFramework(project);

        string archiveName = $"{project.Name}.{Version}_{targetFramework}.zip";
        return LibrariesDirectory / archiveName;
    }

    public AbsolutePath GetProjectNuGetPackagePath(Project project)
    {
        return build.GetProjectBinConfigurationDirectory(project)
            / GetProjectPackageName(project, "nupkg");
    }

    public AbsolutePath GetProjectNuGetSymbolPackagePath(Project project)
    {
        return build.GetProjectBinConfigurationDirectory(project)
            / GetProjectPackageName(project, "snupkg");
    }

    public AbsolutePath GetProjectNuGetPackageArtifactPath(Project project)
    {
        return PackagesDirectory / GetProjectPackageName(project, "nupkg");
    }

    public AbsolutePath GetProjectNuGetSymbolPackageArtifactPath(Project project)
    {
        return PackagesDirectory / GetProjectPackageName(project, "snupkg");
    }

    public AbsolutePath GetProjectCoverageOutputPath(Project project)
    {
        return Coverage / (project.Name + ".xml");
    }

    string GetDefaultTargetFramework(Project project) => project.GetTargetFrameworks()!.First();
}
