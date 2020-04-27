// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Theming
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using Catel.Caching;
    using Catel.IoC;
    using Catel.Logging;
    using ControlzEx.Theming;
    using Services;

    public class ThemeManager
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ControlzEx.Theming.ThemeManager _controlzThemeManager;

        private readonly CacheStorage<ThemeColorStyle, Color> _themeColorsCache = new CacheStorage<ThemeColorStyle, Color>();
        private readonly CacheStorage<ThemeColorStyle, SolidColorBrush> _themeColorBrushesCache = new CacheStorage<ThemeColorStyle, SolidColorBrush>();

        private SolidColorBrush _accentColorBrushCache;

        private Theme _currentTheme;
        #endregion

        #region Constructors
        public ThemeManager(ControlzEx.Theming.ThemeManager controlzThemeManager)
        {
            _controlzThemeManager = controlzThemeManager;
            _controlzThemeManager.ThemeChanged += OnThemeManagerThemeChanged;
        }

        static ThemeManager()
        {
            Current = new ThemeManager(ControlzEx.Theming.ThemeManager.Current);
        }
        #endregion

        public static ThemeManager Current { get; set; }

        #region Methods
        public Color GetThemeColor(ThemeColorStyle colorStyle = ThemeColorStyle.AccentColor)
        {
            return _themeColorsCache.GetFromCacheOrFetch(colorStyle, () =>
            {
                var accentColor = GetAccentColorBrush().Color;

                // For now use a fixed values, we might change in the future
                var borderColor = Colors.LightGray;
                var backgroundColor = Colors.WhiteSmoke;
                var foregroundColor = Colors.Black;
                var foregroundAlternativeColor = Colors.White;

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

                    // Background
                    case ThemeColorStyle.BackgroundColor:
                        return CreateColor(Alpha0, backgroundColor);

                    // Foreground
                    case ThemeColorStyle.ForegroundColor:
                        return CreateColor(Alpha0, foregroundColor);

                    case ThemeColorStyle.ForegroundAlternativeColor:
                        return CreateColor(Alpha0, foregroundAlternativeColor);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(colorStyle));
                }
            });
        }

        private Color CreateColor(int alpha, Color color)
        {
            return Color.FromArgb((byte)alpha, color.R, color.G, color.B);
        }

        public SolidColorBrush GetThemeColorBrush(ThemeColorStyle colorStyle = ThemeColorStyle.AccentColor)
        {
            return _themeColorBrushesCache.GetFromCacheOrFetch(colorStyle, () =>
            {
                var color = GetThemeColor(colorStyle);
                return color.ToSolidColorBrush();
            });
        }

        public SolidColorBrush GetAccentColorBrush()
        {
            if (_accentColorBrushCache is null)
            {
                _accentColorBrushCache = _currentTheme?.PrimaryAccentColor.ToSolidColorBrush() ?? Application.Current?.TryFindResource("AccentColorBrush") as SolidColorBrush ?? Brushes.Green;
            }

            return _accentColorBrushCache;
        }

        private void OnThemeManagerThemeChanged(object sender, ThemeChangedEventArgs e)
        {
            Log.Debug("Theme has changed, clearing current cache");

            _accentColorBrushCache = null;
            _themeColorBrushesCache.Clear();
            _themeColorsCache.Clear();

            _currentTheme = e.NewTheme;
        }
        #endregion
    }
}
