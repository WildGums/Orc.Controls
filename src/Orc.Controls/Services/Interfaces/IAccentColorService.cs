namespace Orc.Controls.Services
{
    using System;
    using System.Windows.Media;

    public interface IAccentColorService
    {
        Color GetAccentColor();

        void SetAccentColor(Color color);

        event EventHandler<EventArgs> AccentColorChanged;
    }
}
