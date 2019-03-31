// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindReplaceSettingsExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Extensions
{
    using System.Text.RegularExpressions;
    using Catel;

    public static class FindReplaceSettingsExtensions
    {
        #region Methods
        public static Regex GetRegEx(this FindReplaceSettings settings, string textToFind, bool isLeftToRight = false)
        {
            Argument.IsNotNull(() => textToFind);
            Argument.IsNotNull(() => settings);

            var options = RegexOptions.None;
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
                return new Regex(textToFind, options);
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

            return new Regex(pattern, options);
        }
        #endregion
    }
}
