using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Serilog;

[Requires<CoverletTasks>(Version = "6.0.1")]
partial class Build
{
    [Parameter]
    readonly bool HtmlTestResults;

    Target Test =>
        _ =>
            _.DependsOn(CompileTests)
                .Executes(
                    () =>
                        Log.Information(
                            "Running tests using projects: {Projects}",
                            TestProjects.Names()
                        ),
                    () =>
                        DotNetTasks.DotNetTest(
                            settings =>
                                settings
                                    .Apply(TestSettingsBase)
                                    .CombineWith(
                                        TestProjects,
                                        (settings, project) =>
                                            settings
                                                .SetProjectFile(project)
                                                .Apply(TestSettingsLogging(project))
                                    )
                        )
                );

    Target CoverageCollect =>
        _ =>
            _.DependsOn(CompileTests)
                .Executes(
                    () =>
                        CoverletTasks.Coverlet(
                            settings =>
                                settings
                                    .SetFormat("cobertura")
                                    .SetTarget("dotnet")
                                    .CombineWith(
                                        TestProjects,
                                        (settings, project) =>
                                            settings
                                                .SetTargetArgs(
                                                    "test",
                                                    project,
                                                    "--no-build",
                                                    "--configuration",
                                                    Configuration
                                                )
                                                .SetAssembly(
                                                    project.Directory
                                                        / "bin"
                                                        / Configuration
                                                        / project.GetTargetFrameworks().First()
                                                        / $"{project.Name}.dll"
                                                )
                                                .SetInclude(
                                                    $"[{project.Name.Replace(".Tests", string.Empty)}]*"
                                                )
                                                .SetOutput(
                                                    ArtifactPaths.Coverage
                                                        / $"coverage.{project.Name}.xml"
                                                )
                                    )
                        )
                );

    Configure<DotNetTestSettings> TestSettingsBase =>
        settings =>
            settings
                .EnableNoBuild()
                .SetConfiguration(Configuration)
                .SetResultsDirectory(ArtifactPaths.TestResults);

    Func<Project, Configure<DotNetTestSettings>> TestSettingsLogging =>
        project =>
            settings =>
                settings
                    .AddLoggers("console;verbosity=detailed")
                    .When(
                        HtmlTestResults,
                        settings =>
                            settings.AddLoggers(
                                $"html;logfilename=test-results.{project.Name}.html"
                            )
                    )
                    .When(
                        Host.IsGitHubActions(),
                        settings =>
                            settings.AddLoggers(
                                "GitHubActions;summary.includePassedTests=true;summary.includeSkippedTests=true"
                            )
                    );
}
