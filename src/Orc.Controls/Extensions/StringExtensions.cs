// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Text.RegularExpressions;
    using System.Windows.Documents;

    public static class StringExtensions
    {
        #region Methods
        public static Inline ToInline(this string text)
        {
            return new Run(text);
        }

        public static string GetRegexStringFromSearchPattern(this string pattern)
        {
            var regexString = string.Empty;
            //if exact matching
            if (pattern.StartsWith("\"") && pattern.EndsWith("\""))
            {
                var modifiedPattern = "/^" + pattern.Substring(1, pattern.Length - 2) + "$/";
                regexString = modifiedPattern.ExtractRegexString();
            }
            
            if (string.IsNullOrWhiteSpace(regexString))
            {
                regexString = pattern.ExtractRegexString();
            }

            if (string.IsNullOrWhiteSpace(regexString))
            {
                regexString = pattern.ConstructWildcardRegex();
            }

            return regexString;
        }

        public static string ConstructWildcardRegex(this string pattern)
        {
            // Always add a wildcard at the end of the pattern
            pattern = "*" + pattern.Trim('*') + "*";

            // Make it case insensitive by default
            return "(?i)^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
        }

        public static string ExtractRegexString(this string filter)
        {
            if (!filter.StartsWith("/") || !filter.EndsWith("/"))
            {
                return string.Empty;
            }

            filter = filter.Substring(1, filter.Length - 2);
            return !filter.IsValidRegexPattern() ? string.Empty : filter;
        }

        public static bool IsValidRegexPattern(this string pattern)
        {
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new Regex(pattern);
                return true;
            }
            catch
            {
                // ignored
            }

            return false;
        }
        #endregion
    }
}
