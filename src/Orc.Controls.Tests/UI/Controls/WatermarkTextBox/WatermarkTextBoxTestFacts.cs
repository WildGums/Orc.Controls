namespace Orc.Controls.Tests.UI
{
    using System.Linq;
    using System.Windows.Input;
    using NUnit.Framework;
    using Orc.Automation;

    [Explicit]
    [TestFixture(TestOf = typeof(Expander))]
    [Category("UI Tests")]
    public class WatermarkTextBoxTestFacts : StyledControlTestFacts<WatermarkTextBox>
    {
        [Target]
        public Automation.WatermarkTextBox Target { get; set; }

        [TestCase("TestWatermark", "TestWatermark")]
        [TestCase("", "")]
        [TestCase(null, "")]
        public void CorrectlySetWatermark(string watermark, string expectedWatermark)
        {
            var target = Target;
            var model = target.Current;

            model.Watermark = watermark;

            KeyboardInput.PressRelease(Key.Tab);

            Assert.That(target.Watermark, Is.EqualTo(expectedWatermark));
        }

        [Test]
        public void CorrectlySelectAllTextOnFocus()
        {
            var target = Target;
            var model = target.Current;

            model.SelectAllOnGotFocus = false;

            const string text = "Some test text";

            target.Text = text;
            
            //Setting value to model object automatically capture focus on control...
            model.SelectAllOnGotFocus = true;
            
            //...so we need to move focus out of control before test
            KeyboardInput.PressRelease(Key.Tab);
            target.SetFocus();

            Wait.UntilResponsive(500);
            
            var selectedText = target.SelectedTextRanges.FirstOrDefault();

            Assert.That(selectedText, Is.EqualTo(text));
        }
    }
}
