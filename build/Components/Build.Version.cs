using Nuke.Common.Tools.GitVersion;

partial class Build
{
    [GitVersion]
    internal readonly GitVersion Version;
}
