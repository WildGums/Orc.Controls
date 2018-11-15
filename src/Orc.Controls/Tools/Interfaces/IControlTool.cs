// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IControlTool.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;

    public interface IControlTool
    {
        #region Properties
        string Name { get; }
        bool IsOpened { get; }
        #endregion

        #region Methods
        void Open();
        void Close();
        #endregion

        event EventHandler<EventArgs> Opened;
        event EventHandler<EventArgs> Closed;
    }
}
