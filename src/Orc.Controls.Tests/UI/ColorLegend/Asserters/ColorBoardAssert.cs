namespace Orc.Controls.Tests
{
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
            var availableColorNames = view.AvailableColorNames;
            if (availableColorNames.Contains(colorName))
            {
                var predefinedColorName = view.PredefinedColorName;
                Assert.That(predefinedColorName, Is.EqualTo(PredefinedColor.GetColorName(expectedColor)));
            }
            else
            {
                Assert.That(colorName, Is.EqualTo(expectedColor.ToString()));
            }
        }
    }
}
