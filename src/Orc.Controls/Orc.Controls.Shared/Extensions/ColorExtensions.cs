// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Media;

    internal static class ColorExtensions
    {
        #region Constants
        private const string AccentBrush = "AccentBrush";
        private const string AccentColor = "AccentColor";
        private const string HighlightBrush = "HighlightBrush";
        private const string HighlightColor = "HighlightColor";
        private static ResourceDictionary _accentColorResourceDictionary;
        #endregion

        #region Methods
        public static ResourceDictionary CreateAccentColorResourceDictionary(this Color color)
        {
            if (_accentColorResourceDictionary != null)
            {
                return _accentColorResourceDictionary;
            }

            var resourceDictionary = new ResourceDictionary();

            resourceDictionary.Add("FilterBoxHighlightColor", color.CalculateHighlightColor());
            resourceDictionary.Add("FilterBoxAccentColor", color);
            //Todo: replace FilterBox colors with Controls colors
            resourceDictionary.Add("ControlsHighlightColor", color.CalculateHighlightColor());
            resourceDictionary.Add("ControlsAccentColor", color);

            resourceDictionary.Add("FilterBoxHighlightBrush", new SolidColorBrush((Color) resourceDictionary["FilterBoxHighlightColor"]));
            resourceDictionary.Add("FilterBoxAccentBrush", new SolidColorBrush((Color) resourceDictionary["FilterBoxAccentColor"]));
            //Todo: replace FilterBox brushes with Controls brushes
            resourceDictionary.Add("ControlsHighlightBrush", new SolidColorBrush((Color) resourceDictionary["ControlsHighlightColor"]));
            resourceDictionary.Add("ControlsAccentBrush", new SolidColorBrush((Color) resourceDictionary["ControlsAccentColor"]));

            var application = Application.Current;
            var applicationResources = application.Resources;
            applicationResources.MergedDictionaries.Insert(0, resourceDictionary);

            _accentColorResourceDictionary = resourceDictionary;
            return applicationResources;
        }

        public static void CreateAccentColorResourceDictionary(this Color color, string controlName)
        {
            var accentColorResource = Application.Current.TryFindResource(controlName.GetAccentBrushName()) as SolidColorBrush;

            if (accentColorResource != null )
            {
                return;
            }

            if (_accentColorResourceDictionary != null)
            {
                _accentColorResourceDictionary.AddResources(color, controlName);
            }

            var resourceDictionary = new ResourceDictionary();
            resourceDictionary.AddResources(color, controlName);
            
            var application = Application.Current;
            var applicationResources = application.Resources;
            applicationResources.MergedDictionaries.Insert(0, resourceDictionary);

            _accentColorResourceDictionary = resourceDictionary;
        }

        private static void AddResources(this ResourceDictionary resourceDictionary, Color color, string name)
        {
            resourceDictionary.Add(name.GetHighlightColorName(), color.CalculateHighlightColor());
            resourceDictionary.Add(name.GetAccentColorName(), color);

            resourceDictionary.Add(name.GetHighlightBrushName(), new SolidColorBrush((Color)resourceDictionary[name.GetHighlightColorName()]));
            resourceDictionary.Add(name.GetAccentBrushName(), new SolidColorBrush((Color)resourceDictionary[name.GetAccentColorName()]));
        }

        private static string GetAccentBrushName(this string controlName)
        {
            return controlName + AccentBrush;
        }

        private static string GetAccentColorName(this string controlName)
        {
            return controlName + AccentColor;
        }

        private static string GetHighlightBrushName(this string controlName)
        {
            return controlName + HighlightBrush;
        }

        private static string GetHighlightColorName(this string controlName)
        {
            return controlName + HighlightColor;
        }

        private static Color CalculateHighlightColor(this Color color)
        {
            return Color.FromArgb(51, color.R, color.G, color.B);
        }
        #endregion
    }
}