using Nuke.Common;
using Nuke.Common.Tools.GitVersion;
using Serilog;

interface IHazVersion : INukeBuild
{
    [GitVersion, Required]
    GitVersion Version => TryGetValue(() => Version);

    Target ShowVersion =>
        _ =>
            _.Executes(
                () => Log.Information("Calculated Semantic version: {SemVer}", Version.SemVer)
            );
}