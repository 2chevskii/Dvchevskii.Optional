using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.GitVersion;

[Requires<GitVersionTasks>(Version = "6.0.0-beta.5")]
partial class Build
{
    [GitVersion]
    internal readonly GitVersion Version;
}
