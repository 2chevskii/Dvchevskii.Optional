using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;

interface IHazArtifacts : INukeBuild
{
    ArtifactPathCollection ArtifactPaths => new ArtifactPathCollection(this);

    void CreateArtifactDirectories() =>
        new[]
        {
            ArtifactPaths.RootDirectory,
            ArtifactPaths.PackagesDirectory,
            ArtifactPaths.LibrariesDirectory,
            ArtifactPaths.TestResults,
            ArtifactPaths.Coverage,
            ArtifactPaths.DocsDirectory
        }.ForEach(directory => AbsolutePathExtensions.CreateDirectory(directory));

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