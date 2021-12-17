// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Documents;

    public static class StringExtensions
    {
        #region Methods
        internal static string ChunkIntValue(this string input, string partValue, out int val)
        {
            val = int.Parse(partValue);
            return input.Substring(partValue.Length);
        }

        internal static bool IsDateTimeFormatPart(this string part)
        {
            return part.Contains('h') || part.Contains('H') || part.Contains('m') || part.Contains('s') || part.Contains('t');
        }

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
            if (string.IsNullOrWhiteSpace(pattern)) return false;

            try
            {
                var regEx = Regex.Match("", pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        public static Stream ToStream(this string s)
        {
            var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        #endregion
    }
}
