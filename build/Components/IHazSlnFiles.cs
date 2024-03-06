using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using Serilog;

interface IHazSlnFiles : INukeBuild
{
    [Solution, Required] Solution Sln => TryGetValue(() => Sln);
    IReadOnlyCollection<Project> MainProjects => Sln.GetSolutionFolder("src")!.Projects;
    IReadOnlyCollection<Project> TestProjects => Sln.GetSolutionFolder("test")!.Projects;
}

interface IHazArtifacts : INukeBuild
{
    ArtifactPathCollection ArtifactPaths => new ArtifactPathCollection(this);

    void CreateArtifactDirectories() =>
        new[]
        {
            ArtifactPaths.RootDirectory, ArtifactPaths.PackagesDirectory, ArtifactPaths.LibrariesDirectory,
            ArtifactPaths.TestResults, ArtifactPaths.Coverage,
            ArtifactPaths.DocsDirectory
        }.ForEach(directory => directory.CreateDirectory());

    public sealed class ArtifactPathCollection
    {
        readonly INukeBuild _build;

        AbsolutePath BuildRootDirectory => _build.RootDirectory;
        public AbsolutePath RootDirectory => BuildRootDirectory / "artifacts";
        public AbsolutePath PackagesDirectory => RootDirectory / "pkg";
        public AbsolutePath LibrariesDirectory => RootDirectory / "lib";
        public AbsolutePath TestResults => RootDirectory / "test_results";
        public AbsolutePath Coverage => TestResults / "coverage";
        public AbsolutePath DocsDirectory => RootDirectory / "docs";

        public ArtifactPathCollection(INukeBuild build)
        {
            _build = build;
        }
    }
}

interface IRestore : IHazSlnFiles
{
    Target Restore => _ => _.Executes(() => DotNetTasks.DotNetRestore(settings => settings.SetProjectFile(Sln)));
}

interface ICompile : IHazSlnFiles, IHazArtifacts, IHazConfiguration, IRestore, IHazVersion
{
    Target Compile => _ => _.DependsOn(CompileMain, CompileTests);

    Target CompileMain => _ => _
        .DependsOn(Restore)
        .Executes(
            () =>
                Log.Information("Building projects: {@Projects}", MainProjects.Select(x => x.Name)),
            () => Log.Information("Version: {Version}", Version.SemVer),
            () =>
                DotNetTasks.DotNetBuild(settings =>
                    settings.Apply(BuildSettingsBase)
                        .CombineWith(MainProjects, (settings, project) => settings.SetProjectFile(project))));

    Target CompileTests => _ => _
        .DependsOn(Restore)
        .DependsOn(CompileMain)
        .Executes(() => DotNetTasks.DotNetBuild(settings =>
            settings.Apply(BuildSettingsBase)
                .CombineWith(TestProjects, (settings, project) => settings.SetProjectFile(project))));

    Configure<DotNetBuildSettings> BuildSettingsBase => settings => settings.EnableNoRestore()
        .EnableNoDependencies()
        .SetConfiguration(Configuration)
        .SetVersion(Version.SemVer);
}

interface IHazVersion : INukeBuild
{
    [GitVersion, Required] GitVersion Version => TryGetValue(() => Version);

    Target ShowVersion => _ =>
        _.Executes(() => Log.Information("Calculated Semantic version: {SemVer}", Version.SemVer));
}
