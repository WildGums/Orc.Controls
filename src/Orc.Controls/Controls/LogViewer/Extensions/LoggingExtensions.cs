namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel.Logging;

    public static class LoggingExtensions
    {
        #region Methods
        public static IEnumerable<RichTextBoxParagraph> ConvertToParagraphs(this IEnumerable<LogEntry> logEntries)
        {
            return logEntries.Select(x => new RichTextBoxParagraph(x));
        }
        #endregion
    }
}
