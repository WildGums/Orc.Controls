// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccentColorService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Services
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using Catel;

    public class AccentColorService : IAccentColorService
    {
        #region Fields
        private Color? _accentColor;
        #endregion

        #region IAccentColorService Members
        public virtual Color GetAccentColor()
        {
            if (_accentColor.HasValue)
            {
                return _accentColor.Value;
            }

            var accentColorBrush = Application.Current.TryFindResource("AccentColorBrush") as SolidColorBrush;
            var finalBrush = accentColorBrush ?? Brushes.Orange;

            _accentColor = finalBrush.Color;

            return _accentColor.Value;
        }

        public virtual void SetAccentColor(Color color)
        {
            _accentColor = color;

            RaiseAccentColorChanged();
        }

        public event EventHandler<EventArgs> AccentColorChanged;
        #endregion

        #region Methods
        protected void RaiseAccentColorChanged()
        {
            AccentColorChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
