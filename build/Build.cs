using Nuke.Common;
using Nuke.Common.Tools.DotNet;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild
        ? Configuration.Debug
        : Configuration.Release;

    Target Clean => _ => _.Before(Restore).Executes(() => { });

    Target Restore => _ => _.Executes(() => { });

    Target Compile => _ => _.DependsOn(Restore).Executes(() => { });

    Target UnitTest =>
        _ =>
            _.DependsOn(Compile)
                .Executes(
                    () =>
                        DotNetTasks.DotNetTest(
                            c =>
                                c.EnableNoBuild()
                                    .AddLoggers("console;verbosity=detailed", "html;logfilename=testResults.html")
                        )
                );
}
