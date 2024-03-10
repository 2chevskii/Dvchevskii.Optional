using Nuke.Common;

public static class HostExtensions
{
    public static bool IsGitHubActions(this Host host) => EnvironmentInfo.HasVariable("GITHUB_ACTIONS");
}
