// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEntryDoubleClickEventArgs.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel.Logging;

    public class LogEntryDoubleClickEventArgs : EventArgs
    {
        #region Constructors
        public LogEntryDoubleClickEventArgs(LogEntry logEntry)
        {
            LogEntry = logEntry;
        }
        #endregion

        #region Properties
        public LogEntry LogEntry { get; private set; }
        #endregion
    }
}
