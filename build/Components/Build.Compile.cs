using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using Serilog;

[SuppressMessage("ReSharper", "AllUnderscoreLocalParameterName")]
partial class Build
{
    Target Compile => _ => _.DependsOn(CompileSrc, CompileTests);

    Target CompileSrc =>
        _ =>
            _.DependsOn(Restore)
                .Executes(
                    () => Log.Information("Building SRC projects: {Projects}", SrcProjects.Names()),
                    () => Log.Information("Version: {Version}", Version.SemVer),
                    () =>
                    {
                        List<string> builtProjects = new();

                        void BuildProject(Project project, IEnumerable<Project> allProjects)
                        {
                            var references = project
                                .GetItems("ProjectReference")
                                .Select(reference =>
                                {
                                    AbsolutePath referencePath = AbsolutePath.Create(
                                        project.Directory + (RelativePath)reference
                                    );

                                    var projectName = referencePath.NameWithoutExtension;
                                    return projectName;
                                });

                            Log.Information(
                                "Building project {ProjectName}, references: {@References}",
                                project.Name,
                                references
                            );

                            foreach (string reference in references)
                            {
                                var referenceProject = allProjects.First(x => x.Name == reference);

                                BuildProject(referenceProject, allProjects);
                            }

                            if (!builtProjects.Contains(project.Name))
                            {
                                DotNetTasks.DotNetBuild(
                                    settings =>
                                        settings.Apply(BuildSettingsBase).SetProjectFile(project)
                                );
                                builtProjects.Add(project.Name);
                            }
                        }

                        SrcProjects.ForEach(x => BuildProject(x, SrcProjects));

                        /*DotNetTasks.DotNetBuild(
                            settings =>
                                settings
                                    .Apply(BuildSettingsBase)
                                    .CombineWith(
                                        SrcProjects,
                                        (settings, project) => settings.SetProjectFile(project)
                                    )
                        );*/
                    }
                );

    Target CompileTests =>
        _ =>
            _.DependsOn(Restore)
                .DependsOn(CompileSrc)
                .Executes(
                    () =>
                        Log.Information("Building TEST projects: {Projects}", TestProjects.Names()),
                    () => Log.Information("Version: {Version}", Version.SemVer),
                    () =>
                        DotNetTasks.DotNetBuild(
                            settings =>
                                settings
                                    .Apply(BuildSettingsBase)
                                    .CombineWith(
                                        TestProjects,
                                        (settings, project) => settings.SetProjectFile(project)
                                    )
                        )
                );

    Configure<DotNetBuildSettings> BuildSettingsBase =>
        settings =>
            settings
                .EnableNoRestore()
                .EnableNoDependencies()
                .SetVersion(Version.SemVer)
                .SetConfiguration(Configuration);
}
