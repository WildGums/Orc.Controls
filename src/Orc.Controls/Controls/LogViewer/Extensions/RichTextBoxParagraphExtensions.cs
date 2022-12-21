namespace Orc.Controls
{
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public static class RichTextBoxParagraphExtensions
    {
        #region Methods
        public static void SetData(this RichTextBoxParagraph paragraph, bool showTimestamp = true, bool showThreadId = true, bool showMultilineMessagesExpanded = false)
        {
            var timestamp = $"{paragraph.LogEntry.Time} ";
            var toolTip = new StringBuilder();

            if (!showTimestamp)
            {
                toolTip.AppendLine("Time: " + timestamp);
                timestamp = string.Empty;
            }

            var logEntry = paragraph.LogEntry;

            toolTip.AppendLine($"Log type: {logEntry?.Log?.TargetType?.FullName}");

            // Note: last call must be Append instead of AppendLine
            toolTip.Append($"Log event: {logEntry?.LogEvent}");

            paragraph.SetCurrentValue(FrameworkContentElement.ToolTipProperty, toolTip.ToString());

            var threadId = string.Empty;
            if (showThreadId)
            {
                var data = logEntry?.Data;
                if (data is not null && data.TryGetValue("ThreadId", out var existingThreadId))
                {
                    threadId = $"[{existingThreadId}] ";
                }
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
                Text = "[...]",
                Margin = new Thickness(5, 0, 0, 0),
                Cursor = Cursors.SizeNWSE
            };

            button.MouseLeftButtonDown += (sender, args) => paragraph.SetData(showTimestamp, showThreadId, true);

            paragraph.Inlines.Add(button);
        }
        #endregion
    }
}
