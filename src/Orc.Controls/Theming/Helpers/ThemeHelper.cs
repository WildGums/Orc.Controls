// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows.Media;
    using Catel.Caching;
    using Catel.IoC;
    using Catel.Logging;
    using Services;

    [ObsoleteEx(ReplacementTypeOrMember = "ThemeColorStyle", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
    public enum AccentColorStyle
    {
        AccentColor = 0,
        AccentColor1 = 1,
        AccentColor2 = 2,
        AccentColor3 = 3,
        AccentColor4 = 4,
        AccentColor5 = 5,

        // Aliases
        DarkHighlight = AccentColor3,
        Highlight = AccentColor4,
    }

    public enum ThemeColorStyle
    {
        AccentColor = 0,
        AccentColor1 = 1,
        AccentColor2 = 2,
        AccentColor3 = 3,
        AccentColor4 = 4,
        AccentColor5 = 5,

        BorderColor = 6,
        BorderColor1 = 7,
        BorderColor2 = 8,
        BorderColor3 = 9,
        BorderColor4 = 10,
        BorderColor5 = 11,

        ForegroundColor = 12,

        // Aliases - highlights
        DarkHighlight = AccentColor3,
        Highlight = AccentColor4,

        // Aliases - borders
        BorderLight = BorderColor3,
        BorderMedium = BorderColor2,
        BorderDark = BorderColor1,
        BorderMouseOver = BorderDark,
        BorderPressed = BorderColor,

        // Aliases - disabled state
        BackgroundDisabled = AccentColor5,
        BorderDisabled = BorderColor5,
    }

    public static class ThemeHelper
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static readonly CacheStorage<ThemeColorStyle, Color> _themeColorsCache = new CacheStorage<ThemeColorStyle, Color>();
        private static readonly CacheStorage<ThemeColorStyle, SolidColorBrush> _themeColorBrushesCache = new CacheStorage<ThemeColorStyle, SolidColorBrush>();

        private static readonly IAccentColorService _accentColorService;
        private static SolidColorBrush _accentColorBrushCache;
        #endregion

        #region Constructors
        static ThemeHelper()
        {
            var serviceLocator = ServiceLocator.Default;

            _accentColorService = serviceLocator.ResolveType<IAccentColorService>();
            _accentColorService.AccentColorChanged += OnAccentColorServiceAccentColorChanged;
        }
        #endregion

        #region Methods
        [ObsoleteEx(ReplacementTypeOrMember = "GetThemeColor", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
        public static Color GetAccentColor(AccentColorStyle colorStyle = AccentColorStyle.AccentColor)
        {
            return GetThemeColor((ThemeColorStyle)(int)colorStyle);
        }

        public static Color GetThemeColor(ThemeColorStyle colorStyle = ThemeColorStyle.AccentColor)
        {
            return _themeColorsCache.GetFromCacheOrFetch(colorStyle, () =>
            {
                var accentColor = GetAccentColorBrush().Color;

                // For now use a fixed values, we might change in the future
                var borderColor = Colors.LightGray;
                var foreground = Colors.Black;

                const int Alpha0 = 255;
                const int Alpha1 = 204;
                const int Alpha2 = 153;
                const int Alpha3 = 102;
                const int Alpha4 = 51;
                const int Alpha5 = 20;

                switch (colorStyle)
                {
                    // Accent color
                    case ThemeColorStyle.AccentColor:
                        return CreateColor(Alpha0, accentColor);

                    case ThemeColorStyle.AccentColor1:
                        return CreateColor(Alpha1, accentColor);

                    case ThemeColorStyle.AccentColor2:
                        return CreateColor(Alpha2, accentColor);

                    case ThemeColorStyle.AccentColor3:
                        return CreateColor(Alpha3, accentColor);

                    case ThemeColorStyle.AccentColor4:
                        return CreateColor(Alpha4, accentColor);

                    case ThemeColorStyle.AccentColor5:
                        return CreateColor(Alpha5, accentColor);

                    // Border
                    case ThemeColorStyle.BorderColor:
                        return CreateColor(Alpha0, borderColor);

                    case ThemeColorStyle.BorderColor1:
                        return CreateColor(Alpha1, borderColor);

                    case ThemeColorStyle.BorderColor2:
                        return CreateColor(Alpha2, borderColor);

                    case ThemeColorStyle.BorderColor3:
                        return CreateColor(Alpha3, borderColor);

                    case ThemeColorStyle.BorderColor4:
                        return CreateColor(Alpha4, borderColor);

                    case ThemeColorStyle.BorderColor5:
                        return CreateColor(Alpha5, borderColor);

                    // Foreground
                    case ThemeColorStyle.ForegroundColor:
                        return CreateColor(Alpha0, foreground);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(colorStyle));
                }
            });
        }

        private static Color CreateColor(int alpha, Color color)
        {
            return Color.FromArgb((byte)alpha, color.R, color.G, color.B);
        }

        [ObsoleteEx(ReplacementTypeOrMember = "GetThemeColorBrush", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
        public static SolidColorBrush GetAccentColorBrush(AccentColorStyle colorStyle = AccentColorStyle.AccentColor)
        {
            return GetThemeColorBrush((ThemeColorStyle)(int)colorStyle);
        }

        public static SolidColorBrush GetThemeColorBrush(ThemeColorStyle colorStyle = ThemeColorStyle.AccentColor)
        {
            return _themeColorBrushesCache.GetFromCacheOrFetch(colorStyle, () =>
            {
                var color = GetThemeColor(colorStyle);
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

        private static void OnAccentColorServiceAccentColorChanged(object sender, EventArgs e)
        {
            Log.Debug("Accent color has changed, clearing current cache");

            _accentColorBrushCache = null;
            _themeColorBrushesCache.Clear();
            _themeColorsCache.Clear();
        }
        #endregion
    }
}
