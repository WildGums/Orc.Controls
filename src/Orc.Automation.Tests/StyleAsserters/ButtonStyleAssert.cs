namespace Orc.Automation.Tests.StyleAsserters
{
    using System.Windows.Media;
    using Controls;
    using NUnit.Framework;

    public class ThemeAssert
    {
        public const string ControlDefaultBackgroundBrushResourceName = "Orc.Brushes.Control.Default.Background";
        public const string ControlDefaultMouseOverBrushResourceName = "Orc.Brushes.Control.MouseOver.Background";
        public const string ControlDefaultBorderBrushResourceName = "Orc.Brushes.Control.Default.Border";
    }

    public class FrameworkElementAssert : ThemeAssert
    {
        public static void BorderColor(FrameworkElement element)
        {
            var controlDefaultBorderBrush = element.TryFindResource(ControlDefaultBorderBrushResourceName) as SolidColorBrush;

            Assert.That(element.BorderBrush.Color, Is.EqualTo(controlDefaultBorderBrush?.Color));
        }

        public static void MouseOverBackground(FrameworkElement element)
        {
            var controlDefaultMouseOverBrush = element.TryFindResource(ControlDefaultMouseOverBrushResourceName) as SolidColorBrush;

            var rect = element.Element.Current.BoundingRectangle;
            MouseInput.MoveTo(rect.GetClickablePoint());

            var mouseOverBackgroundColor = element.Background.Color;

            Assert.That(mouseOverBackgroundColor, Is.EqualTo(controlDefaultMouseOverBrush?.Color));
        }

        public static void Background(FrameworkElement element)
        {
            var controlDefaultBackground = element.TryFindResource(ControlDefaultBackgroundBrushResourceName) as SolidColorBrush;

            Assert.That(element.Background.Color, Is.EqualTo(controlDefaultBackground?.Color));
        }
    }

    public class ButtonThemeAssert : FrameworkElementAssert
    {

    }
}
