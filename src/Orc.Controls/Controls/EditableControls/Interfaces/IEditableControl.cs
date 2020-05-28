// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEditableControl.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;

    public interface IEditableControl
    {
        #region Properties
        bool IsInEditMode { get; }
        #endregion

        event EventHandler<EventArgs> EditStarted;
        event EventHandler<EventArgs> EditEnded;
    }
}
