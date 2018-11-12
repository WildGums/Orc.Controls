// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAccentColorService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Services
{
    using System;
    using System.Windows.Media;

    public interface IAccentColorService
    {
        #region Methods
        Color GetAccentColor();

        void SetAccentColor(Color color);
        #endregion

        event EventHandler<EventArgs> AccentColorChanged;
    }
}
