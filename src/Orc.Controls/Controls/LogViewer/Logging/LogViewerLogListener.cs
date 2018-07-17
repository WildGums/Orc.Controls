// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerLogListener.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel;
    using Catel.Logging;

    public class LogViewerLogListener : RollingInMemoryLogListener
    {
        public LogViewerLogListener()
        {
            MaximumNumberOfLogEntries = 500;
        }
    }
}
