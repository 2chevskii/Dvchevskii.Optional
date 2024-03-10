using System;
using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.ReportGenerator;
using Serilog;

[Requires<CoverletTasks>(Version = "6.0.1")]
[Requires<ReportGeneratorTasks>(Version = "5.2.2")]
partial class Build
{
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
                                                .SetTargetArgs(GetCoverletTargetArgs(project))
                                                .SetAssembly(
                                                    project.GetAssemblyOutputPath(Configuration)
                                                )
                                                .SetInclude(GetCoverageAssemblyFilter(project))
                                                .SetOutput(GetCoverageOutputPath(project))
                                    )
                        )
                );

    Target CoverageReport =>
        _ =>
            _.DependsOn(CoverageCollect)
                .Executes(
                    () =>
                        ReportGeneratorTasks.ReportGenerator(
                            settings =>
                                settings.CombineWith(
                                    TestProjects,
                                    (settings, project) =>
                                        settings
                                            .SetReports(GetCoverageOutputPath(project))
                                            .SetTargetDirectory(
                                                GetCoverageReportOutputDirectory(project)
                                            )
                                )
                        )
                );

    AbsolutePath GetCoverageReportOutputDirectory(Project project) =>
        ArtifactPaths.Coverage / $"report.{project.Name}";

    AbsolutePath GetCoverageOutputPath(Project project) =>
        ArtifactPaths.Coverage / $"coverage.{project.Name}.xml";

    string GetCoverageAssemblyFilter(Project project)
    {
        string mainProjectName = project.Name.Replace(".Tests", string.Empty);

        return $"[{mainProjectName}]*";
    }

    IEnumerable<string> GetCoverletTargetArgs(Project project) =>
        ["test", project, "--no-build", "--configuration", Configuration];
}

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
