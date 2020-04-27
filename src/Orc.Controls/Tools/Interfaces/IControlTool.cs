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
        bool IsEnabled { get; }
        #endregion

        #region Methods
        void Attach(object target);
        void Detach();
        void Open(object parameter);
        void Close();
        #endregion

        event EventHandler<EventArgs> Attached;
        event EventHandler<EventArgs> Detached;
        event EventHandler<EventArgs> Opening;
        event EventHandler<EventArgs> Opened;
        event EventHandler<EventArgs> Closed;
    }
}
