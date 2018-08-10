// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerLogListener.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel.Logging;

    public class LogViewerLogListener : RollingInMemoryLogListener
    {
        #region Constructors
        public LogViewerLogListener()
        {
            MaximumNumberOfLogEntries = 500;
        }
        #endregion
    }
}
