// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Extensions
{
    using System.Windows;
    using System.Windows.Media;

    internal static class ColorExtensions
    {
        #region Constants
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

            resourceDictionary.Add("FilterBoxHighlightBrush", new SolidColorBrush((Color)resourceDictionary["FilterBoxHighlightColor"]));
            resourceDictionary.Add("FilterBoxAccentBrush", new SolidColorBrush((Color)resourceDictionary["FilterBoxAccentColor"]));
            //Todo: replace FilterBox brushes with Controls brushes
            resourceDictionary.Add("ControlsHighlightBrush", new SolidColorBrush((Color)resourceDictionary["ControlsHighlightColor"]));
            resourceDictionary.Add("ControlsAccentBrush", new SolidColorBrush((Color)resourceDictionary["ControlsAccentColor"]));

            var application = Application.Current;
            var applicationResources = application.Resources;
            applicationResources.MergedDictionaries.Insert(0, resourceDictionary);

            _accentColorResourceDictionary = resourceDictionary;
            return applicationResources;
        }

        private static Color CalculateHighlightColor(this Color color)
        {
            return Color.FromArgb(51, color.R, color.G, color.B);
        }
        #endregion
    }
}