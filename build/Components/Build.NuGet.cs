using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

// ReSharper disable AllUnderscoreLocalParameterName

partial class Build
{
    [Parameter]
    readonly string NugetApiKey;

    [Parameter]
    readonly string NugetFeedName;

    [Parameter]
    readonly string NugetFeedUrl;

    Target NuGetPush =>
        _ =>
            _.Requires(
                    () => !string.IsNullOrEmpty(NugetApiKey),
                    () => !string.IsNullOrEmpty(ReleaseAssets),
                    () => !string.IsNullOrEmpty(NugetFeedName),
                    () => !string.IsNullOrEmpty(NugetFeedUrl)
                )
                .DependsOn(EnsureHasNuGetSource)
                .Executes(() =>
                {
                    var packages = ReleaseAssetsDirectory.GlobFiles("*.nupkg");

                    foreach (var packagePath in packages)
                    {
                        DotNetTasks.DotNetNuGetPush(
                            settings =>
                                settings
                                    .SetSource(NugetFeedName)
                                    .SetApiKey(NugetApiKey)
                                    .SetTargetPath(packagePath)
                        );
                    }
                });

    Target EnsureHasNuGetSource =>
        _ =>
            _.Unlisted()
                .Requires(
                    () => !string.IsNullOrEmpty(NugetApiKey),
                    () => !string.IsNullOrEmpty(NugetFeedName),
                    () => !string.IsNullOrEmpty(NugetFeedUrl)
                )
                .Executes(() =>
                {
                    var sourceList = GetNuGetSources();

                    if (sourceList.All(s => s.Name != NugetFeedName))
                    {
                        var source = new NuGetSource
                        {
                            IsEnabled = true,
                            Name = NugetFeedName,
                            Url = NugetFeedUrl
                        };

                        DotNetTasks.DotNetNuGetAddSource(
                            settings => settings.Apply(source.ConfigureAddSource)
                        );
                    }
                });

    IEnumerable<NuGetSource> GetNuGetSources() =>
        NuGetSource.ParseAll(
            string.Join("\n", DotNetTasks.DotNet("nuget list source").Select(output => output.Text))
        );
}

class NuGetSource
{
    // ReSharper disable once InconsistentNaming
    static readonly Regex s_ParsePattern = new Regex(
        @"\d+\.\s+([^\s]+)\s\[(Enabled|Disabled)\]\n\s+([^\s]+)"
    );

    public bool IsEnabled;
    public string Name;
    public string Url;

    public Configure<DotNetNuGetAddSourceSettings> ConfigureAddSource =>
        settings => settings.SetName(Name).SetSource(Url);

    public static IEnumerable<NuGetSource> ParseAll(string input)
    {
        var matchCollection = s_ParsePattern.Matches(input);

        foreach (Match match in matchCollection)
        {
            var name = match.Groups[1].Value;
            var enabledString = match.Groups[2].Value;
            var url = match.Groups[3].Value;
            var enabledValue = ParseEnabledValue(enabledString);

            yield return new NuGetSource
            {
                IsEnabled = enabledValue,
                Name = name,
                Url = url,
            };
        }
    }

    static bool ParseEnabledValue(string enabledString)
    {
        return enabledString switch
        {
            "Enabled" => true,
            "Disabled" => false,
            _
                => throw new ArgumentException(
                    "Failed to parse Enabled value, invalid input: " + enabledString
                )
        };
    }

    public override string ToString() => $"Enabled:{IsEnabled} {Name} // {Url}";
}
