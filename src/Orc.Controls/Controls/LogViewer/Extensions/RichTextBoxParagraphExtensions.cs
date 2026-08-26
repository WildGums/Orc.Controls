namespace Orc.Controls;

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Catel;

public static class RichTextBoxParagraphExtensions
{
    public static void SetData(this RichTextBoxParagraph paragraph, bool showTimestamp = true,
        bool showThreadId = true, bool showMultilineMessagesExpanded = false)
    {
        var timestamp = $"{paragraph.LogEntry.DateTime.LocalDateTime} ";
        var toolTip = new StringBuilder();

        if (!showTimestamp)
        {
            toolTip.AppendLine(LanguageHelper.GetRequiredString("Controls_LogViewer_ToolTip_Time") + " " + timestamp);
            timestamp = string.Empty;
        }

        var logEntry = paragraph.LogEntry;

        toolTip.AppendLine($"{LanguageHelper.GetRequiredString("Controls_LogViewer_ToolTip_LogType")} {logEntry?.Category}");

        // Note: last call must be Append instead of AppendLine
        toolTip.Append($"{LanguageHelper.GetRequiredString("Controls_LogViewer_ToolTip_LogEvent")} {logEntry?.LogLevel}");

        paragraph.SetCurrentValue(FrameworkContentElement.ToolTipProperty, toolTip.ToString());

        var threadId = string.Empty;
        if (showThreadId)
        {
            //var data = logEntry?.Data;
            //if (data is not null && data.TryGetValue("ThreadId", out var existingThreadId))
            //{
            //    threadId = $"[{existingThreadId}] ";
            //}
        }

        var message = logEntry?.Message;
        var buttonRequired = false;
        if (!showMultilineMessagesExpanded && !string.IsNullOrEmpty(message))
        {
            var lines = message.Split('\n');
            if (lines.Length > 1)
            {
                message = lines[0].Trim();
                buttonRequired = true;
            }
        }

        var text = $"{timestamp}{threadId}{message}";
        paragraph.Inlines.Clear();
        paragraph.Inlines.Add(text);

        if (!buttonRequired)
        {
            return;
        }

        var button = new TextBlock
        {
            Text = LanguageHelper.GetRequiredString("Controls_LogViewer_ExpandButton"),
            Margin = new Thickness(5, 0, 0, 0),
            Cursor = Cursors.SizeNWSE
        };

        button.MouseLeftButtonDown += (_, _) => paragraph.SetData(showTimestamp, showThreadId, true);

        paragraph.Inlines.Add(button);
    }
}
