using Nuke.Common;
using Nuke.Common.Tools.GitVersion;

[Requires<GitVersionTasks>(Version = "6.0.0-beta.6")]
partial class Build
{
    [GitVersion]
    internal readonly GitVersion Version;
}
