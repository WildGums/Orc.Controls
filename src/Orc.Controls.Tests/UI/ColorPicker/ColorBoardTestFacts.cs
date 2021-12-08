namespace Orc.Controls.Tests
{
    using System.Collections;
    using System.Windows.Media;
    using Automation;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [TestFixture(TestOf = typeof(Orc.Controls.ColorBoard))]
    [NUnit.Framework.Category("UI Tests")]
    public class ColorBoardTestFacts : ControlUiTestFactsBase<Orc.Controls.ColorBoard>
    {
        [Target]
        public ColorBoard Target { get; set; }

        [SetUp]
        public override void SetUpTest()
        {
            base.SetUpTest();
        }

        private class PositiveSetRGBColorTestCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                //Random values
                yield return Colors.Red;
                yield return Color.FromArgb(0x12, 0x34, 0x56, 0x78);
                yield return Color.FromArgb(0x43, 0x88, 0xFA, 0xEE);

                //Border values
                yield return Color.FromArgb(0x00, 0x00, 0x00, 0x00);
                yield return Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
                yield return Color.FromArgb(0x00, 0xFF, 0x00, 0xFF);
                yield return Color.FromArgb(0xFF, 0x00, 0xFF, 0x00);
            }
        }

        [TestCaseSource(typeof(PositiveSetRGBColorTestCases))]
        public void VerifyInitialColorState()
        {
            var target = Target;

            ColorBoardAssert.Color(target, target.Color);
        }

        [TestCaseSource(typeof(PositiveSetRGBColorTestCases))]
        public void CorrectlySetHsvColor(Color color)
        {
            var target = Target;
            var view = target.View;

            view.HsvColor = color;

            ColorBoardAssert.Color(target, color);
        }

        [TestCaseSource(typeof(PositiveSetRGBColorTestCases))]
        public void CorrectlySetArgbColor(Color color)
        {
            var target = Target;
            var view = target.View;

            view.ArgbColor = color;

            ColorBoardAssert.Color(target, color);
        }

        [TestCaseSource(typeof(PositiveSetRGBColorTestCases))]
        public void CorrectlySetColorByText(Color color)
        {
            var target = Target;
            var view = target.View;

            view.ColorName = color.ToString();

            var colorName = view.ColorName;

            ColorBoardAssert.Color(target, color);
        }

        [TestCaseSource(typeof(PositiveSetRGBColorTestCases))]
        public void CorrectlySetArgbAltColor(Color color)
        {
            var target = Target;
            var view = target.View;

            view.ArgbColorAlt = color;

            ColorBoardAssert.Color(target, color);
        }
    }
}
