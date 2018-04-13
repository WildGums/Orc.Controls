// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel.Logging;

    public static class LoggingExtensions
    {
        public static IEnumerable<RichTextBoxParagraph> ConvertToParagraphs(this IEnumerable<LogEntry> logEntries)
        {
            return logEntries.Select(x => new RichTextBoxParagraph(x));
        }
    }
}