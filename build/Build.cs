// ReSharper disable AllUnderscoreLocalParameterName

using System;
using LibGit2Sharp;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Serilog;
using Repository = LibGit2Sharp.Repository;

class Build : NukeBuild
{
    const string NugetOrgSource = "https://api.nuget.org/v3/index.json";

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = Configuration.Debug;

    readonly string NugetApiKey = EnvironmentInfo.GetVariable("NUGET_API_KEY");

    [GitVersion]
    readonly GitVersion Version;

    string TagName => $"v{Version.SemVer}";

    Target ActionsSetSemver =>
        _ =>
            _.OnlyWhenDynamic(() => Host is GitHubActions)
                .Executes(() =>
                {
                    string versionString = Version.SemVer;
                    GitHubActions.Instance.WriteDebug(
                        $"Appending calculated semantic version to GitHub environment: {versionString}"
                    );
                    AbsolutePath githubEnvPath = EnvironmentInfo.GetVariable("GITHUB_ENV");
                    githubEnvPath.AppendAllLines(new[] { "GITVERSION_SEMVER=" + versionString });
                });

    Target Clean =>
        _ =>
            _.Executes(
                () => DotNetTasks.DotNetClean(settings => settings.SetConfiguration(Configuration))
            );

    Target CleanAll =>
        _ =>
            _.Executes(
                () =>
                    DotNetTasks.DotNetClean(
                        settings => settings.SetConfiguration(Configuration.Debug)
                    ),
                () =>
                    DotNetTasks.DotNetClean(
                        settings => settings.SetConfiguration(Configuration.Release)
                    )
            );

    Target Restore => _ => _.Executes(() => DotNetTasks.DotNetRestore());

    Target CompileMain =>
        _ =>
            _.DependsOn(Restore)
                .Executes(() =>
                {
                    Log.Information("Compiling Dvchevskii.Optional:{Configuration}", Configuration);
                    DotNetTasks.DotNetBuild(
                        settings =>
                            settings
                                .SetProjectFile(
                                    RootDirectory
                                        / "src/Dvchevskii.Optional/Dvchevskii.Optional.csproj"
                                )
                                .SetConfiguration(Configuration)
                                .SetVersion(Version.SemVer)
                                .EnableNoRestore()
                    );
                });

    Target CompileTests =>
        _ =>
            _.DependsOn(Restore, CompileMain)
                .Executes(() =>
                {
                    Log.Information(
                        "Compiling Dvchevskii.Optional.Tests:{Configuration}",
                        Configuration
                    );
                    DotNetTasks.DotNetBuild(
                        settings =>
                            settings
                                .SetProjectFile(
                                    RootDirectory
                                        / "test/Dvchevskii.Optional.Tests/Dvchevskii.Optional.Tests.csproj"
                                )
                                .SetConfiguration(Configuration)
                                .EnableNoRestore()
                                .EnableNoDependencies()
                    );
                });

    Target Compile => _ => _.DependsOn(CompileMain, CompileTests);

    Target UnitTest =>
        _ =>
            _.DependsOn(Compile)
                .Executes(
                    () =>
                        DotNetTasks.DotNetTest(
                            c =>
                                c.SetProjectFile(
                                        RootDirectory
                                            / "test/Dvchevskii.Optional.Tests/Dvchevskii.Optional.Tests.csproj"
                                    )
                                    .SetConfiguration(Configuration)
                                    .EnableNoRestore()
                                    .EnableNoBuild()
                                    .AddLoggers(
                                        "console;verbosity=detailed",
                                        "html;logfilename=test-results.html"
                                    )
                        )
                );

    Target Pack =>
        _ =>
            _.DependsOn(CompileMain)
                .Executes(
                    () =>
                        DotNetTasks.DotNetPack(
                            c =>
                                c.SetConfiguration(Configuration)
                                    .EnableNoRestore()
                                    .EnableNoBuild()
                                    .EnableNoDependencies()
                                    .SetVersion(Version.SemVer)
                        )
                );

    Target GetSemver =>
        _ => _.Executes(() => Log.Information("Semantic version is: {SemVer}", Version.SemVer));

    Target CreateVersionTag =>
        _ =>
            _.Executes(() =>
            {
                using Repository repository = GetRepository();

                if (repository.Head.FriendlyName != "master")
                {
                    throw new Exception(
                        "Failed to create next version tag: Not on 'master' branch"
                    );
                }

                Tag tag = repository.ApplyTag(TagName);

                PushOptions pushOptions = new PushOptions
                {
                    CredentialsProvider = (_, usernameFromUrl, _) =>
                        new UsernamePasswordCredentials
                        {
                            Username = GitHubActions.Instance.RepositoryOwner,
                            Password = GitHubActions.Instance.Token
                        }
                };

                Remote remote = repository.Network.Remotes["origin"];
                repository.Network.Push(remote, tag.CanonicalName, pushOptions);
            });

    public static int Main() => Execute<Build>(x => x.Compile);

    Repository GetRepository()
    {
        return new Repository(RootDirectory);
    }
}
