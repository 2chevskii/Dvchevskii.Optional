using System.Diagnostics.CodeAnalysis;
using Nuke.Common;
using Nuke.Common.Tools.DotNet;

[SuppressMessage("ReSharper", "AllUnderscoreLocalParameterName")]
partial class Build
{
    Target Restore => _ => _.Executes(() => DotNetTasks.DotNetRestore(settings => settings.SetProjectFile(Sln)));
}
