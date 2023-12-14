﻿namespace Orc.Controls.Example
{
    using System;
    using Catel.Logging;

    internal class ExampleLogListener : LogListenerBase
    {
        #region Constructors
        public ExampleLogListener()
        {
            IgnoreCatelLogging = false;
        }
        #endregion

        #region Methods
        protected override bool ShouldIgnoreLogMessage(ILog log, string message, LogEvent logEvent, object extraData, LogData logData, DateTime time)
        {
            return message.StartsWith("Uninitialized");
        }
        #endregion
    }
}
