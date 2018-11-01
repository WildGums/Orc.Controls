namespace Orc.Controls
{
    using System;
    using System.Windows.Media;
    using Catel.Caching;
    using Catel.IoC;
    using Orc.Controls.Services;

    public enum AccentColorStyle
    {
        AccentColor = 0,
        AccentColor1 = 1,
        AccentColor2 = 2,
        AccentColor3 = 3,
        AccentColor4 = 4,
        AccentColor5 = 5,

        DarkHighlight = AccentColor3,
        Highlight = AccentColor4,
    }

    public static class ThemeHelper
    {
        private static readonly CacheStorage<AccentColorStyle, Color> _accentColorsCache = new CacheStorage<AccentColorStyle, Color>();
        private static readonly CacheStorage<AccentColorStyle, SolidColorBrush> _accentColorBrushesCache = new CacheStorage<AccentColorStyle, SolidColorBrush>();

        private static SolidColorBrush _accentColorBrushCache;

        public static Color GetAccentColor(AccentColorStyle colorStyle = AccentColorStyle.AccentColor)
        {
            return _accentColorsCache.GetFromCacheOrFetch(colorStyle, () =>
            {
                var color = GetAccentColorBrush().Color;

                switch (colorStyle)
                {
                    case AccentColorStyle.AccentColor:
                        return Color.FromArgb(255, color.R, color.G, color.B);

                    case AccentColorStyle.AccentColor1:
                        return Color.FromArgb(204, color.R, color.G, color.B);

                    case AccentColorStyle.AccentColor2:
                        return Color.FromArgb(153, color.R, color.G, color.B);

                    case AccentColorStyle.AccentColor3:
                        return Color.FromArgb(102, color.R, color.G, color.B);

                    case AccentColorStyle.AccentColor4:
                        return Color.FromArgb(51, color.R, color.G, color.B);

                    case AccentColorStyle.AccentColor5:
                        return Color.FromArgb(20, color.R, color.G, color.B);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(colorStyle));
                }
            });
        }

        public static SolidColorBrush GetAccentColorBrush(AccentColorStyle colorStyle)
        {
            return _accentColorBrushesCache.GetFromCacheOrFetch(colorStyle, () =>
            {
                var color = GetAccentColor(colorStyle);
                return GetSolidColorBrush(color);
            });
        }

        public static SolidColorBrush GetAccentColorBrush()
        {
            if (_accentColorBrushCache != null)
            {
                return _accentColorBrushCache;
            }

            var accentColorService = ServiceLocator.Default.ResolveType<IAccentColorService>();
            _accentColorBrushCache = GetSolidColorBrush(accentColorService.GetAccentColor());

            return _accentColorBrushCache;
        }

        public static SolidColorBrush GetSolidColorBrush(this Color color, double opacity = 1d)
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
