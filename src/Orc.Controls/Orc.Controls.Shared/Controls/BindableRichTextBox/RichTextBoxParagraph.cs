// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RichTextBoxParagraph.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows.Documents;
    using Catel.Logging;

    public class RichTextBoxParagraph: Paragraph
    {
        public RichTextBoxParagraph(LogEntry logEntry)
        {
            LogEntry = logEntry;
        }

        public LogEntry LogEntry { get; private set; }
    }
}