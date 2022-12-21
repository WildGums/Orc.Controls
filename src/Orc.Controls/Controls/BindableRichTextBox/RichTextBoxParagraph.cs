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
