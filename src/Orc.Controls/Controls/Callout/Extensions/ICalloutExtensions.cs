namespace Orc.Controls;

using System;
using Catel;

public static class ICalloutExtensions
{
    public static string GetCalloutConfigurationKeyPrefix(this ICallout callout)
    {
        ArgumentNullException.ThrowIfNull(callout);

        return GetCalloutConfigurationKeyPrefix(callout.Name, callout.Version);
    }

    public static string GetCalloutConfigurationKeyPrefix(string? name, string? version)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            name = LanguageHelper.GetRequiredString("Controls_Callout_Unnamed");
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            version = LanguageHelper.GetRequiredString("Controls_Callout_DefaultVersion");
        }

        var value = $"Callouts.{name}.{version}";
        return value;
    }
}
