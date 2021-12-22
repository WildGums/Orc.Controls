namespace Orc.Controls.Tests
{
    using System;
    using System.Collections;
    using System.Threading;
    using System.Windows.Media;
    using Automation;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [TestFixture(TestOf = typeof(Orc.Controls.ColorPicker))]
    [NUnit.Framework.Category("UI Tests")]
    public class ColorPickerTestFacts : ControlUiTestFactsBase<Orc.Controls.ColorPicker>
    {
        [Target]
        public ColorPicker Target { get; set; }

        [SetUp]
        public override void SetUpTest()
        {
            base.SetUpTest();
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

        private bool _isColorChangedEventInvoked;

        [TestCaseSource(typeof(PositiveSetRGBColorTestCases))]
        public void CorrectlySetColor(Color color)
        {
            var target = Target;
            var view = target.View;

            var colorBoard = view.OpenColorBoard();
            Assert.That(colorBoard, Is.Not.Null);
            Assert.That(target.IsDropDownOpen, Is.True);

            var colorBoardView = colorBoard.View;
            colorBoardView.ArgbColor = color;

            //colorPicker CurrentColor should have as expected color
            Assert.That(target.CurrentColor, Is.EqualTo(color));

            //check event invocation
            if (!Equals(target.Color, color))
            {
                target.ColorChanged += OnColorChanged;
            }
            else
            {
                _isColorChangedEventInvoked = true;
            }

            //Before apply drop down should be opened
            Assert.That(target.IsDropDownOpen, Is.True);

            //correctly apply button
            Assert.That(colorBoard.View.Apply(), Is.True);

            Wait.UntilResponsive();

            //After apply drop down should be closed
            Assert.That(target.IsDropDownOpen, Is.False);

            Assert.That(_isColorChangedEventInvoked, Is.True);

            //colorPicker should have expected color
            Assert.That(target.Color, Is.EqualTo(color));

            target.ColorChanged -= OnColorChanged;
        }

        private void OnColorChanged(object sender, EventArgs e)
        {
            _isColorChangedEventInvoked = true;
        }
    }
}

