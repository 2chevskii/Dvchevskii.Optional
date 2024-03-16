using System;
using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Serilog;

// ReSharper disable AllUnderscoreLocalParameterName

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
                        settings => settings.AddLoggers($"html;logfilename={project.Name}.html")
                    )
                    .When(
                        Host.IsGitHubActions(),
                        settings =>
                            settings.AddLoggers(
                                "GitHubActions;summary.includePassedTests=true;summary.includeSkippedTests=true"
                            )
                    );
}
