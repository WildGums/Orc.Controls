namespace Orc.Controls;

using System.Windows.Documents;
using Catel.Logging;

public class RichTextBoxParagraph : Paragraph
{
    public RichTextBoxParagraph(LogEntry logEntry)
    {
        LogEntry = logEntry;
    }

    public LogEntry LogEntry { get; }
}
