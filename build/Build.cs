using System.Text;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
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

    Target OutputVersionEnv => _ => _.Executes(() =>
    {
        string currentEnv = EnvironmentInfo.GetVariable("GITHUB_ENV");
        StringBuilder sb = new StringBuilder(currentEnv);
        string versionString = Version.SemVer;
        sb.Append("GITVERSION_SEMVER=");
        sb.AppendLine(versionString);
        EnvironmentInfo.SetVariable("GITHUB_ENV", sb.ToString());
    });

    Target Clean =>
        _ =>
            _.Before(Restore)
                .Executes(() => DotNetTasks.DotNetClean(c => c.SetConfiguration(Configuration)));

    Target Restore => _ => _.Executes(() => DotNetTasks.DotNetRestore());

    Target Compile =>
        _ =>
            _.DependsOn(Restore)
                .Executes(
                    () =>
                        DotNetTasks.DotNetBuild(
                            c =>
                                c.SetVersion(Version.SemVer)
                                    .SetConfiguration(Configuration)
                                    .EnableNoRestore()
                        )
                );

    Target Pack =>
        _ =>
            _.DependsOn(Compile)
                .Executes(
                    () =>
                        DotNetTasks.DotNetPack(
                            c =>
                                c.SetConfiguration(Configuration)
                                    .EnableNoBuild()
                                    .EnableNoRestore()
                                    .SetVersion(Version.SemVer)
                        )
                );

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
