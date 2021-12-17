namespace Orc.Controls.Tests
{
    using System.Collections;
    using System.Linq;
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

            var map = target.Map<ColorBoardMap>();
            var selectButton = map.SelectButton;
            var background = selectButton.Background;

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
        public void CorrectlySetArgbAltColor(Color color)
        {
            var target = Target;
            var view = target.View;

            view.ArgbColorAlt = color;

            ColorBoardAssert.Color(target, color);
        }

        [TestCaseSource(typeof(PositiveSetRGBColorTestCases))]
        public void CorrectlySetColorByName(Color color)
        {
            var target = Target;
            var view = target.View;

            view.ColorName = color.ToString();

            ColorBoardAssert.Color(target, color);
        }

        private class PredefinedColorsTestCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new object[] { nameof(Colors.Red), Colors.Red };
                yield return new object[] { nameof(Colors.Blue), Colors.Blue };
                yield return new object[] { nameof(Colors.Green), Colors.Green };
                yield return new object[] { nameof(Colors.Lime), Colors.Lime };
            }
        }

        //TODO: issue: 
        [Ignore("Not implemented yet, see: https://sesolutions.atlassian.net/browse/ORCOMP-653 {5. - issue number}")]
        [TestCaseSource(typeof(PredefinedColorsTestCases))]
        public void CorrectlySetColorByPredifinedName(string predefinedColorName, Color expectedColor)
        {
            var target = Target;
            var view = target.View;

            view.ColorName = predefinedColorName;

            ColorBoardAssert.Color(target, expectedColor);
        }

        [TestCaseSource(typeof(PredefinedColorsTestCases))]
        public void CorrectlySetPredefinedColor(string predefinedColorName, Color expectedColor)
        {
            var target = Target;
            var view = target.View;

            view.PredefinedColorName = predefinedColorName;

            ColorBoardAssert.Color(target, expectedColor);
        }

        private class PredefinedThemeColorsTestCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                return PredefinedColor.AllThemeColors.GetEnumerator();
            }
        }

        [TestCaseSource(typeof(PredefinedThemeColorsTestCases))]
        public void CorrectlySetThemeColor(PredefinedColor expectedPredefinedColor)
        {
            var target = Target;
            var view = target.View;

            var themeColors = view.ThemeColors;

            var themeColor = themeColors.FirstOrDefault(x => Equals(x.Color, expectedPredefinedColor.Value));
            Assert.That(themeColor, Is.Not.Null);

            themeColor.IsSelected = true;

            ColorBoardAssert.Color(target, expectedPredefinedColor.Value);
        }
    }
}
