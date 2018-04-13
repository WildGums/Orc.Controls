// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RichTextBoxParagraph.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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