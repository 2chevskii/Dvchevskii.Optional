using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Utilities.Text.Yaml;
using Serilog;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild
        ? Configuration.Debug
        : Configuration.Release;

    [GitVersion]
    readonly GitVersion Version;

    Target ShowVersion =>
        _ => _.Executes(() => Log.Information("Version:\n{Version}", Version.ToYaml()));

    Target Clean =>
        _ =>
            _.Before(Restore)
                .Executes(() => DotNetTasks.DotNetClean(c => c.SetConfiguration(Configuration)));

    Target Restore => _ => _.Executes(() => DotNetTasks.DotNetRestore());

    Target Compile =>
        _ =>
            _.DependsOn(Restore)
                .Executes(() => DotNetTasks.DotNetBuild(c => c.SetConfiguration(Configuration)));

    Target UnitTest =>
        _ =>
            _.DependsOn(Compile)
                .Executes(
                    () =>
                        DotNetTasks.DotNetTest(
                            c =>
                                c.SetConfiguration(Configuration)
                                    .EnableNoBuild()
                                    .AddLoggers(
                                        "console;verbosity=detailed",
                                        "html;logfilename=testResults.html"
                                    )
                        )
                );
}
