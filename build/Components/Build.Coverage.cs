using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.ReportGenerator;

[Requires<CoverletTasks>(Version = "6.0.1")]
[Requires<ReportGeneratorTasks>(Version = "5.2.2")]
partial class Build
{
    const string CoverageFormat = "cobertura";

    Target CoverageCollect =>
        _ =>
            _.DependsOn(CompileTests)
                .Executes(
                    () =>
                        CoverletTasks.Coverlet(
                            settings =>
                                settings
                                    .Apply(CoverletSettingsBase)
                                    .CombineWith(
                                        TestProjects,
                                        (settings, project) =>
                                            settings.Apply(GetCoverletSettingsProject(project))
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
                                settings
                                    .SetReports(
                                        TestProjects
                                            .Select(
                                                project =>
                                                    ArtifactPaths
                                                        .GetProjectCoverageOutputPath(project)
                                                        .ToString()
                                            )
                                    )
                                    .SetTargetDirectory(ArtifactPaths.CoverageHtmlReport)
                        )
                );

    Configure<CoverletSettings> CoverletSettingsBase =>
        settings => settings.SetFormat(CoverageFormat).SetTarget("dotnet");

    Configure<CoverletSettings> GetCoverletSettingsProject(Project project) =>
        settings =>
            settings
                .SetTargetArgs(GetCoverletTargetArgs(project))
                .SetAssembly(GetProjectAssemblyOutputPath(project))
                .SetInclude(GetCoverageAssemblyFilter(project))
                .SetOutput(ArtifactPaths.GetProjectCoverageOutputPath(project));

    string GetCoverageAssemblyFilter(Project project)
    {
        return $"[{project.GetSrcProjectName()}]*";
    }

    IEnumerable<string> GetCoverletTargetArgs(Project project) =>
        ["test", project, "--no-build", "--configuration", Configuration];
}
