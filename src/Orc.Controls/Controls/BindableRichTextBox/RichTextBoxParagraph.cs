// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RichTextBoxParagraph.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows.Documents;
    using Catel.Logging;

    public class RichTextBoxParagraph : Paragraph
    {
        #region Constructors
        public RichTextBoxParagraph(LogEntry logEntry)
        {
            LogEntry = logEntry;
        }
        #endregion

        #region Properties
        public LogEntry LogEntry { get; }
        #endregion
    }
}
