namespace Orc.Controls;

using System;
using System.Text.RegularExpressions;

public static class FindReplaceSettingsExtensions
{
    public static Regex GetRegEx(this FindReplaceSettings settings, string textToFind, bool isLeftToRight = false)
    {
        ArgumentNullException.ThrowIfNull(textToFind);
        ArgumentNullException.ThrowIfNull(settings);

        var options = RegexOptions.Compiled;
        if (settings.IsSearchUp && !isLeftToRight)
        {
            options |= RegexOptions.RightToLeft;
        }

        if (!settings.CaseSensitive)
        {
            options |= RegexOptions.IgnoreCase;
        }

        if (settings.UseRegex)
        {
            return new Regex(textToFind, options, TimeSpan.FromSeconds(1));
        }

        var pattern = Regex.Escape(textToFind);
        if (settings.UseWildcards)
        {
            pattern = pattern.Replace("\\*", ".*").Replace("\\?", ".");
        }

        if (settings.WholeWord)
        {
            pattern = "\\b" + pattern + "\\b";
        }

        return new Regex(pattern, options, TimeSpan.FromSeconds(1));
    }
}
