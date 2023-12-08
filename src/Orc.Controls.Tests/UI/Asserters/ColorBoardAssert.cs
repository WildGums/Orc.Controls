namespace Orc.Controls.Tests
{
    using System.Linq;
    using System.Windows.Media;
    using NUnit.Framework;
    using Automation;
    using System;

    public static class ColorBoardAssert
    {
        /// <summary>
        /// View and Control api should be in correct state
        /// </summary>
        /// <param name="colorBoard">Verified color picker</param>
        /// <param name="expectedColor">Expected color</param>
        public static void Color(ColorBoard colorBoard, Color expectedColor)
        {
            ArgumentNullException.ThrowIfNull(colorBoard);

            //Color from api should be expected
            var apiColor = colorBoard.Current.Color;
            Assert.That(apiColor, Is.EqualTo(expectedColor));

            //Check ArgbColor
            Assert.That(colorBoard.ArgbColor, Is.EqualTo(expectedColor)); 
           //Check ArgbColorAlt
            Assert.That(colorBoard.ArgbColorAlt, Is.EqualTo(expectedColor));
            
            //Check HsvColor
            Assert.That(colorBoard.HsvColor, Is.EqualTo(expectedColor));

            //Check ColorName
            var colorName = colorBoard.ColorName;
            var predefinedColorName = colorBoard.PredefinedColorName;
            if (!string.IsNullOrWhiteSpace(predefinedColorName))
            {
                Assert.That(predefinedColorName, Is.EqualTo(PredefinedColor.GetColorName(expectedColor)));
                Assert.That(colorName, Is.EqualTo(PredefinedColor.GetColorName(expectedColor)));
            }
            else
            {
                Assert.That(colorName, Is.EqualTo(expectedColor.ToString()));
            }

            //If color contains in theme colors list it should be selected here
            if (colorBoard.ThemeColors.Any(x => Equals(x.Color, expectedColor)))
            {
                Assert.That(colorBoard.SelectedThemeColor, Is.EqualTo(expectedColor));
            }

            //If color contains in recent colors list it should be selected here
            var recentColors = colorBoard.RecentColors;
            if (recentColors.Any(x => Equals(x.Color, expectedColor)))
            {
                Assert.That(colorBoard.SelectedRecentColor, Is.EqualTo(expectedColor));
            }
        }
    }
}
