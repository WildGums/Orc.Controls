// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecordDoubleClickEventArgs.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Examples.Models;

    public class LogRecordDoubleClickEventArgs : EventArgs
    {
        #region Constructors
        public LogRecordDoubleClickEventArgs(LogRecord logRecord)
        {
            LogRecord = logRecord;
        }
        #endregion

        #region Properties
        public LogRecord LogRecord { get; private set; }
        #endregion
    }
}