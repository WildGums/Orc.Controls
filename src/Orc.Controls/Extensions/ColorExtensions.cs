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
        #region Methods
        public static ResourceDictionary CreateAccentColorResourceDictionary(this Color color)
        {
            var resourceDictionary = new ResourceDictionary();

            resourceDictionary.Add("HighlightColor", color.CalculateHighlightColor());
            resourceDictionary.Add("AccentColor", color);

            resourceDictionary.Add("HighlightBrush", new SolidColorBrush((Color) resourceDictionary["HighlightColor"]));
            resourceDictionary.Add("AccentColorBrush", new SolidColorBrush((Color) resourceDictionary["AccentColor"]));

            var application = Application.Current;
            var applicationResources = application.Resources;
            applicationResources.MergedDictionaries.Insert(0, resourceDictionary);
            return applicationResources;
        }
        private static Color CalculateHighlightColor(this Color color)
        {
            return Color.FromArgb(51, color.R, color.G, color.B);
        }
        #endregion
    }
}