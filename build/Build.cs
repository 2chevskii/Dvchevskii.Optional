using JetBrains.Annotations;
using Nuke.Common;
using Serilog;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
partial class Build : NukeBuild
{
    // public static int Main() => Execute<Build>(x => x.Compile);

    static int Main()
    {
        return Execute<Build>(build => build.Compile);
    }

    protected override void OnBuildInitialized()
    {
        Log.Information("Build version: {Version}", Version.SemVer);
        CreateArtifactDirectories();
        LoadRepository();
    }

    protected override void OnBuildFinished()
    {
        DisposeRepository();
    }
}
