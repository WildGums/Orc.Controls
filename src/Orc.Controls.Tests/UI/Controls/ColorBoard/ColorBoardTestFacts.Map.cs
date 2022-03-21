namespace Orc.Controls.Tests.UI
{
    using Automation;
    using NUnit.Framework;

    public partial class ColorBoardTestFacts
    {
        //TODO:Test color edit ranges
        [Test]
        public void VerifyHsvCanvas()
        {
            var map = Target.Map<ColorBoardMap>();

            //map.ColorEdit
        }

        [Test]
        public void VerifyHsvRanges()
        {
            var map = Target.Map<ColorBoardMap>();

            var slider = map.HSVSlider;

            var minimum = slider.Minimum;
            var maximum = slider.Maximum;

            Assert.That(minimum, Is.EqualTo(0));
            Assert.That(maximum, Is.EqualTo(359));
        }

        [Test]
        public void VerifyInputRangesArgbSliders()
        {
            var map = Target.Map<ColorBoardMap>();

            var slider = map.ASlider;

            var minimum = slider.Minimum;
            var maximum = slider.Maximum;

            Assert.That(minimum, Is.EqualTo(0));
            Assert.That(maximum, Is.EqualTo(255));


            slider = map.RSlider;

            minimum = slider.Minimum;
            maximum = slider.Maximum;

            Assert.That(minimum, Is.EqualTo(0));
            Assert.That(maximum, Is.EqualTo(255));

            slider = map.GSlider;

            minimum = slider.Minimum;
            maximum = slider.Maximum;

            Assert.That(minimum, Is.EqualTo(0));
            Assert.That(maximum, Is.EqualTo(255));

            slider = map.BSlider;

            minimum = slider.Minimum;
            maximum = slider.Maximum;

            Assert.That(minimum, Is.EqualTo(0));
            Assert.That(maximum, Is.EqualTo(255));
        }

        //Ordinary values
        [TestCase("AA", "AA", "AA")]
        [TestCase("12", "12", "12")]
        [TestCase("1B", "1B", "1B")]
        //Border values
        [TestCase("0", "0", "00")]
        [TestCase("000", "000", "00")]
        [TestCase("1", "1", "01")]
        [TestCase("FF", "FF", "FF")]
        [TestCase("FE", "FE", "FE")]
        //Wrong values
        [TestCase("GG", "", "")]
        [TestCase("-1", "", "")]
        [TestCase("FFF", "FF", "FF")]
        [TestCase("111", "11", "11")]
        public void VerifyArgbEditorsInput(string input, string expectedText, string expectedTextAfterLostFocus)
        {
            var map = Target.Map<ColorBoardMap>();

            //A edit
            map.AEdit.Text = input;
            Assert.That(map.AEdit.Text, Is.EqualTo(expectedText));

            map.SelectButton.SetFocus();
            Assert.That(map.AEdit.Text, Is.EqualTo(expectedTextAfterLostFocus));

            //R edit
            map.REdit.Text = input;
            Assert.That(map.REdit.Text, Is.EqualTo(expectedText));

            map.SelectButton.SetFocus();
            Assert.That(map.REdit.Text, Is.EqualTo(expectedTextAfterLostFocus));

            //G edit
            map.GEdit.Text = input;
            Assert.That(map.GEdit.Text, Is.EqualTo(expectedText));

            map.SelectButton.SetFocus();
            Assert.That(map.GEdit.Text, Is.EqualTo(expectedTextAfterLostFocus));

            //B edit
            map.BEdit.Text = input;
            Assert.That(map.BEdit.Text, Is.EqualTo(expectedText));

            map.SelectButton.SetFocus();
            Assert.That(map.BEdit.Text, Is.EqualTo(expectedTextAfterLostFocus));
        }
    }
}
