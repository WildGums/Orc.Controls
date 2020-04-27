namespace Orc.Controls
{
    using System.Windows.Media;

    public static class ColorExtensions
    {
        public static SolidColorBrush ToSolidColorBrush(this Color color, double opacity = 1d)
        {
            var brush = new SolidColorBrush(color)
            {
                Opacity = opacity
            };

            brush.Freeze();

            return brush;
        }
    }
}
