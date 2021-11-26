namespace Orc.Controls.Tests
{
    using System.Collections;
    using System.Threading;
    using System.Windows.Media;
    using Automation;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [TestFixture(TestOf = typeof(ColorPicker))]
    [NUnit.Framework.Category("UI Tests")]
    public class ColorPickerTestFacts : ControlUiTestFactsBase<ColorPicker>
    {
        [Target]
        public ColorPickerAutomationControl Target { get; set; }

        [SetUp]
        public override void SetUpTest()
        {
            base.SetUpTest();

            var color = Target.Color;

            Thread.Sleep(200);

            Target.Color = Colors.Red;
        }
        

        private class PositiveSetRGBColorTestCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return GenerateColorTestCase(0x00, 0x00, 0x00, 0x00);
                yield return GenerateColorTestCase(0xFF, 0xFF, 0xFF, 0xFF);
                yield return GenerateColorTestCase(0x00, 0xFF, 0x00, 0xFF);
                yield return GenerateColorTestCase(0xFF, 0x00, 0xFF, 0x00);
                yield return GenerateColorTestCase(0x12, 0x34, 0x56, 0x78);
            }
        }

        private class NegativeSetRGBColorTestCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new object[] { 256, 256, 256, 256, Color.FromArgb(255, 255, 255, 255) };
                yield return new object[] { -1, -2, -1, -1, Color.FromArgb(0, 0, 0, 0) };
            }
        }

        [TestCaseSource(typeof(PositiveSetRGBColorTestCases))]
        //[TestCaseSource(typeof(NegativeSetRGBColorTestCases))]

        public void CorrectlySetRGBColor(int a, int r, int g, int b, Color expectedColor)
        {
            var target = Target;

            var scenario = target.CreateScenario<ColorPickerScenario>();

            var colorBoard = scenario.OpenColorBoard();
            Assert.That(colorBoard, Is.Not.Null);
            Assert.That(target.IsDropDownOpen, Is.True);

            var colorBoardScenario = colorBoard.CreateScenario<ColorBoardScenario>();
            colorBoardScenario.SelectARgbColor(a, r, g, b);

            //Color board should have expected color
            Assert.That(colorBoard.Color, Is.EqualTo(expectedColor));

            //...and ColorPicker CurrentColor should have as expected color
            Assert.That(target.CurrentColor, Is.EqualTo(expectedColor));

            //Correctly apply button
            Assert.That(colorBoardScenario.Apply(), Is.True);

            Wait.UntilResponsive();

            //ColorPicker should have expected color
            Assert.That(target.Color, Is.EqualTo(expectedColor));
        }

        private static object[] GenerateColorTestCase(byte a, byte r, byte g, byte b)
        {
            return new object[] { a, r, g, b, Color.FromArgb(a, r, g, b) };
        }
    }
}

