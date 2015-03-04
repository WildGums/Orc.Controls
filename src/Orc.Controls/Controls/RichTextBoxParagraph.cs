// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RichTextBoxParagraph.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Controls
{
    using System.Windows.Documents;
    using System.Windows.Input;
    using Examples.Models;

    public class RichTextBoxParagraph: Paragraph
    {
        public RichTextBoxParagraph(LogRecord logRecord)
        {
            LogRecord = logRecord;
        }

        public LogRecord LogRecord { get; private set; }
    }
}