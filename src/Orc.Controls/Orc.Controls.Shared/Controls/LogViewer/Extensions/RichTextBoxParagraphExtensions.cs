// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RichTextBoxParagraphExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Text;

    public static class RichTextBoxParagraphExtensions
    {
        public static void SetData(this RichTextBoxParagraph paragraph, bool showTimestamp = true, bool showThreadId = true)
        {
            var timestamp = $"{paragraph.LogEntry.Time} ";
            var toolTip = new StringBuilder();

            if (!showTimestamp)
            {
                toolTip.AppendLine("Time: " + timestamp);
                timestamp = string.Empty;
            }

            toolTip.Append("Log event: " + paragraph.LogEntry.Log.Tag);
            paragraph.SetCurrentValue(System.Windows.FrameworkContentElement.ToolTipProperty, toolTip);

            var threadId = string.Empty;
            var data = paragraph.LogEntry.Data;
            if (showThreadId && data.ContainsKey("ThreadId"))
            {
                threadId = $"[{data["ThreadId"]}] ";
            }

            var text = $"{timestamp}{threadId}{paragraph.LogEntry.Message}";
            paragraph.Inlines.Add(text);
        }
    }
}