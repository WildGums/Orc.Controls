namespace Orc.Controls.Services
{
    using System;
    using System.Windows.Media;

    public interface IAccentColorService
    {
        Color GetAccentColor();

        event EventHandler<EventArgs> AccentColorChanged;
    }
}
