// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RichTextBoxParagraphExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Text;

    public static class RichTextBoxParagraphExtensions
    {
        public static void SetData(this RichTextBoxParagraph paragraph, bool showTimestamp = true, bool showThreadId = true)
        {
            var timestamp = string.Format("{0} ", paragraph.LogEntry.Time);
            var toolTip = new StringBuilder();

            if (!showTimestamp)
            {
                toolTip.AppendLine("Time: " + timestamp);
                timestamp = string.Empty;
            }

            toolTip.Append("Log event: " + paragraph.LogEntry.Log.Tag);
            paragraph.ToolTip = toolTip;

            var threadId = string.Empty;
            var data = paragraph.LogEntry.Data;
            if (showThreadId && data.ContainsKey("ThreadId"))
            {
                threadId = string.Format("[{0}] ", data["ThreadId"]);
            }

            var text = string.Format("{0}{1}{2}", timestamp, threadId, paragraph.LogEntry.Message);
            paragraph.Inlines.Add(text);
        }
    }
}