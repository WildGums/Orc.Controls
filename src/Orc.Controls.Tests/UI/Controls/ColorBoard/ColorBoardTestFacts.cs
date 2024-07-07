namespace Orc.Controls.Tests.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;
    using Automation;
    using NUnit.Framework;
    using Orc.Automation;

    [Explicit]
    [TestFixture(TestOf = typeof(Orc.Controls.ColorBoard))]
    [Category("UI Tests")]
    public partial class ColorBoardTestFacts : StyledControlTestFacts<Orc.Controls.ColorBoard>
    {
        private bool _cancelClicked;
        private bool _doneClicked;

        [Target]
        public ColorBoard Target { get; set; }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
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
            var current = target.Current;

            var margin = current.Margin;
            var desiredSize = current.DesiredSize;

            ColorBoardAssert.Color(target, target.Current.Color);
        }

        [TestCaseSource(typeof(PositiveSetRGBColorTestCases))]
        public void CorrectlySetHsvColor(Color color)
        {
            var target = Target;
            target.HsvColor = color;

            ColorBoardAssert.Color(target, color);
        }

        [TestCaseSource(typeof(PositiveSetRGBColorTestCases))]
        public void CorrectlySetArgbColor(Color color)
        {
            var target = Target;

            target.ArgbColor = color;

            ColorBoardAssert.Color(target, color);
        }

        [TestCaseSource(typeof(PositiveSetRGBColorTestCases))]
        public void CorrectlySetArgbAltColor(Color color)
        {
            var target = Target;

            target.ArgbColorAlt = color;

            ColorBoardAssert.Color(target, color);
        }

        [TestCaseSource(typeof(PositiveSetRGBColorTestCases))]
        public void CorrectlySetColorByName(Color color)
        {
            var target = Target;

            target.ColorName = color.ToString();

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

            target.ColorName = predefinedColorName;

            ColorBoardAssert.Color(target, expectedColor);
        }

        [TestCaseSource(typeof(PredefinedColorsTestCases))]
        public void CorrectlySetPredefinedColor(string predefinedColorName, Color expectedColor)
        {
            var target = Target;

            target.PredefinedColorName = predefinedColorName;

            ColorBoardAssert.Color(target, expectedColor);
        }

        private class PredefinedThemeColorsTestCases : IEnumerable
        {
            //Took border values
            public IEnumerator GetEnumerator()
            {
                var themeColors = PredefinedColor.AllThemeColors;
                var colorCount = themeColors.Count;

                yield return themeColors[0].Value;
                yield return themeColors[colorCount - 1].Value;
                yield return themeColors[1].Value;
                yield return themeColors[colorCount - 2].Value;
            }
        }

        [TestCaseSource(typeof(PredefinedThemeColorsTestCases))]
        public void CorrectlySetThemeColor(Color color)
        {
            var target = Target;

            target.SelectedThemeColor = color;

            ColorBoardAssert.Color(target, color);
        }

        private class RecentColorsListTestCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new object[] { new List<Color> { Colors.Red, Colors.Green, Colors.Blue }, new List<Color> { Colors.Blue, Colors.Green, Colors.Red } };
                yield return new object[] { new List<Color> { Colors.Red, Colors.Green, Colors.Blue, Colors.Green, Colors.Red }, new List<Color> { Colors.Red, Colors.Green, Colors.Blue } };
                yield return new object[] { new List<Color> { Colors.Green, Colors.Green, Colors.Green }, new List<Color> { Colors.Green } };
            }
        }

        [TestCaseSource(typeof(RecentColorsListTestCases))]
        public void CorrectlyGenerateRecentColorList(List<Color> colorList, List<Color> expectedColorList)
        {
            var target = Target;
            var model = target.Current;

            foreach (var color in colorList)
            {
                model.Color = color;

                Wait.UntilResponsive();

                target.Apply();

                Wait.UntilResponsive();
            }

            Assert.That(expectedColorList, Is.EqualTo(target.RecentColors.Select(x => x.Color)).AsCollection);
        }

        private class RecentColorsTestCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                var allThemeColors = PredefinedColor.AllThemeColors;

                yield return new object[] { new List<Color> { Colors.Red, Colors.Green, Colors.Blue },  Colors.Blue };
                yield return new object[] { new List<Color> { Colors.Red, Colors.Green, Colors.Blue },  Colors.Green };
                yield return new object[] { new List<Color> { Colors.Red, Colors.Green, Colors.Blue },  Colors.Red };
                yield return new object[] { new List<Color> { allThemeColors[0].Value, Colors.Green, allThemeColors[2].Value }, allThemeColors[2].Value };
            }
        }

        [TestCaseSource(typeof(RecentColorsTestCases))]
        public void CorrectlySetRecentColor(List<Color> recentColorList, Color recentColor)
        {
            var target = Target;
            var model = target.Current;

            //Init recent color list
            foreach (var color in recentColorList)
            {
                model.Color = color;

                Wait.UntilResponsive();

                target.Apply();

                Wait.UntilResponsive();
            }

            target.SelectedRecentColor = recentColor;

            ColorBoardAssert.Color(target, recentColor);
        }
        
        [Test]
        public void CorrectlyApply()
        {
            var target = Target;
            var model = target.Current;

            target.DoneClicked += OnDoneClicked;

            target.Apply();

            Wait.UntilResponsive();

            //event should be fired
            Assert.That(_doneClicked, Is.True);

            var recentColors = target.RecentColors;

            //current color should be in a recent list
            Assert.That(recentColors.Count, Is.EqualTo(1));
            Assert.That(recentColors[0].Color, Is.EqualTo(model.Color));

            target.DoneClicked -= OnDoneClicked;
        }

        [Test]
        public void CorrectlyCancel()
        {
            var target = Target;

            target.CancelClicked += OnCancelClicked;

            target.Cancel();

            Wait.UntilResponsive();

            //event should be fired
            Assert.That(_cancelClicked, Is.True);

            target.CancelClicked -= OnCancelClicked;
        }
        
        private void OnDoneClicked(object sender, EventArgs e)
        {
            _doneClicked = true;
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            _cancelClicked = true;
        }
    }
}
