namespace Orc.Controls.Settings;

using System.Collections.Generic;
using System.Windows;

/// <summary>
/// Settings-specific helper methods that use WpfTreeHelper for navigation.
/// </summary>
internal static class SettingsHelper
{
    /// <summary>
    /// Gets the combined settings key by traversing up the visual/logical tree, 
    /// with special handling for TabControl virtualization.
    /// </summary>
    /// <param name="startElement">The element to start from</param>
    /// <param name="separator">The separator to use between keys</param>
    /// <returns>Combined settings key path</returns>
    public static string? GetCombinedSettingsKey(DependencyObject? startElement, string separator = "/")
    {
        if (startElement is null)
        {
            return string.Empty;
        }

        var keys = new List<string>();
        var current = startElement;

        while (current is not null)
        {
            var settingsKey = SettingsManagement.GetSettingsKey(current)?.ToString();
            if (!string.IsNullOrEmpty(settingsKey))
            {
                keys.Add(settingsKey);
            }
            else
            {
                var isSet = DependencyPropertyHelper.IsPropertySet(current, SettingsManagement.SettingsKeyProperty);
                if (isSet)
                {
                    return null;
                }
            }

            current = WpfTreeHelper.GetNextParent(current);
        }

        // Reverse the list to get the path from root to current element
        keys.Reverse();
        return string.Join(separator, keys);
    }
}
