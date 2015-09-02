// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleLogListener.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples
{
    using System;
    using Catel.Logging;

    internal class ExampleLogListener : LogListenerBase
    {
        public ExampleLogListener()
        {
            IgnoreCatelLogging = false;
        }

        protected override bool ShouldIgnoreLogMessage(ILog log, string message, LogEvent logEvent, object extraData, LogData logData, DateTime time)
        {
            if (message.StartsWith("Uninitialized"))
            {
                return true;
            }

            return false;
        }
    }
}