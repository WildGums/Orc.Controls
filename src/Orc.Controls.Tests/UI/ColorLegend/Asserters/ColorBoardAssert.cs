namespace Orc.Controls.Tests
{
    using System.Linq;
    using System.Windows.Media;
    using Catel;
    using NUnit.Framework;
    using Automation;

    public static class ColorBoardAssert
    {
        /// <summary>
        /// View and Control api should be in correct state
        /// </summary>
        /// <param name="colorBoard">Verified color picker</param>
        /// <param name="expectedColor">Expected color</param>
        public static void Color(ColorBoard colorBoard, Color expectedColor)
        {
            Argument.IsNotNull(() => colorBoard);

            //Color from api should be expected
            var apiColor = colorBoard.Color;
            Assert.That(apiColor, Is.EqualTo(expectedColor));

            var view = colorBoard.View;

            //Check ArgbColor
            Assert.That(view.ArgbColor, Is.EqualTo(expectedColor)); 
           //Check ArgbColorAlt
            Assert.That(view.ArgbColorAlt, Is.EqualTo(expectedColor));
            
            //Check HsvColor
            Assert.That(view.HsvColor, Is.EqualTo(expectedColor));

            //Check ColorName
            var colorName = view.ColorName;
            var predefinedColorName = view.PredefinedColorName;
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
            if (view.ThemeColors.Any(x => Equals(x.Color, expectedColor)))
            {
                Assert.That(view.SelectedThemeColor, Is.EqualTo(expectedColor));
            }

            //If color contains in recent colors list it should be selected here
            var recentColors = view.RecentColors;
            if (recentColors.Any(x => Equals(x.Color, expectedColor)))
            {
                Assert.That(view.SelectedRecentColor, Is.EqualTo(expectedColor));
            }
        }
    }
}
