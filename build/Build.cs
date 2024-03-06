using Nuke.Common;

class Build : NukeBuild, IHazArtifacts, ICompile, IRestore
{
    public static int Main() => Execute<Build>(x => ((ICompile)x).Compile);

    protected override void OnBuildInitialized() => ((IHazArtifacts)this).CreateArtifactDirectories();
}
