// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerLogListener.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Logging
{
    using System;
    using Catel.Logging;

    public class LogViewerLogListener : LogListenerBase
    {
        protected override string FormatLogEvent(ILog log, string message, LogEvent logEvent, object extraData, DateTime time)
        {
            // TODO: add thread it, etc
            return base.FormatLogEvent(log, message, logEvent, extraData, time);
        }
    }
}