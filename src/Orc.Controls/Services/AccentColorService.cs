namespace Orc.Controls.Services
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using Catel;

    public class AccentColorService : IAccentColorService
    {
        public virtual Color GetAccentColor()
        {
            var accentColorBrush = Application.Current.TryFindResource("AccentColorBrush") as SolidColorBrush;
            var finalBrush = accentColorBrush ?? Brushes.Orange;
            return finalBrush.Color;
        }

        public event EventHandler<EventArgs> AccentColorChanged;

        protected void RaiseAccentColorChanged()
        {
            AccentColorChanged.SafeInvoke(this);
        }
    }
}
