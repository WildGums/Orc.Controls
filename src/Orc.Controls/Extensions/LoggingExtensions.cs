// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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