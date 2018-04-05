// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RichTextBoxParagraphExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public static class RichTextBoxParagraphExtensions
    {
        public static void SetData(this RichTextBoxParagraph paragraph, bool showTimestamp = true, bool showThreadId = true, bool showMultilineMessagesExpanded = false)
        {
            var timestamp = $"{paragraph.LogEntry.Time} ";
            var toolTip = new StringBuilder();

            if (!showTimestamp)
            {
                toolTip.AppendLine("Time: " + timestamp);
                timestamp = string.Empty;
            }

            toolTip.Append("Log event: " + paragraph.LogEntry.Log.Tag);
            paragraph.SetCurrentValue(FrameworkContentElement.ToolTipProperty, toolTip);

            var threadId = string.Empty;
            var data = paragraph.LogEntry.Data;
            if (showThreadId && data.ContainsKey("ThreadId"))
            {
                threadId = $"[{data["ThreadId"]}] ";
            }

            var message = paragraph.LogEntry.Message;
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
            if (buttonRequired)
            {
                var button = new TextBlock { Text = "[...]", Margin = new Thickness(5, 0, 0, 0), Cursor = Cursors.Arrow };
                button.MouseLeftButtonDown += (sender, args) => { paragraph.SetData(showTimestamp, showThreadId, true); };
                paragraph.Inlines.Add(button);
            }
        }
    }
}