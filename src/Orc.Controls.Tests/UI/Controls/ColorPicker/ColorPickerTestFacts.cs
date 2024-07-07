namespace Orc.Controls.Tests.UI
{
    using System.Collections;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using Automation;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [Explicit]
    [TestFixture(TestOf = typeof(Orc.Controls.ColorPicker))]
    [Category("UI Tests")]
    public class ColorPickerTestFacts : StyledControlTestFacts<Orc.Controls.ColorPicker>
    {
        [Target]
        public ColorPicker Target { get; set; }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        private class PositiveSetRGBColorTestCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return Color.FromArgb(0x00, 0x00, 0x00, 0x00);
                yield return Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
                yield return Color.FromArgb(0x00, 0xFF, 0x00, 0xFF);
                yield return Color.FromArgb(0xFF, 0x00, 0xFF, 0x00);
                yield return Color.FromArgb(0x12, 0x34, 0x56, 0x78);
            }
        }

        [Test]
        public void Navigate_Through_Rectangle_Pane_Without_Changing_Color()
        {
            var target = Target;
            var targetModel = target.Current;

            target.Current.HorizontalAlignment = HorizontalAlignment.Center;
            target.Current.VerticalAlignment = VerticalAlignment.Center;

            var colorBoard = target.OpenColorBoard();

            var rect = colorBoard.BoundingRectangle;

            var topLeft = rect.TopLeft;
            var bottomRight = rect.BottomRight;

            MouseInput.MoveTo(topLeft);

            //Smoothly moving pointer over surface of Color-rect
            const int movingIterations = 20;
            for (var i = 0; i < movingIterations; i++)
            {
                var previousCurrentColor = targetModel.CurrentColor;

                var newPoint = new Point(topLeft.X + (bottomRight.X - topLeft.X) / movingIterations * i, 
                    topLeft.Y + (bottomRight.Y - topLeft.Y) / movingIterations * i);

                MouseInput.MoveTo(newPoint);

                Thread.Sleep(10);

                //...and color shouldn't be changing
                Assert.That(targetModel.CurrentColor, Is.EqualTo(previousCurrentColor));
            }
        }

        [TestCaseSource(typeof(PositiveSetRGBColorTestCases))]
        public void CorrectlySetColor(Color color)
        {
            var target = Target;
            var targetProperties = target.Current;

            target.Current.HorizontalAlignment = HorizontalAlignment.Center;
            target.Current.VerticalAlignment = VerticalAlignment.Center;

            var colorBoard = target.OpenColorBoard();
            Assert.That(colorBoard, Is.Not.Null);
            Assert.That(targetProperties.IsDropDownOpen, Is.True);

            colorBoard.ArgbColor = color;

            //colorPicker CurrentColor should have as expected color
            Assert.That(targetProperties.CurrentColor, Is.EqualTo(color));

            //check event invocation
            //if color different event should be raised otherwise shouldn't
            if (!Equals(target.Color, color))
            {
                EventAssert.Raised(target, nameof(target.ColorChanged), () => colorBoard.Apply());
            }
            else
            {
                EventAssert.NotRaised(target, nameof(target.ColorChanged), () => colorBoard.Apply());
            }

            Wait.UntilResponsive();

            //After apply drop down should be closed
            Assert.That(targetProperties.IsDropDownOpen, Is.False);

            //colorPicker should have expected color
            Assert.That(target.Color, Is.EqualTo(color));
        }
    }
}

