namespace Orc.Controls.Tests
{
    using NUnit.Framework;
    using Orc.Automation;

    [TestFixture(TestOf = typeof(FrameCounter))]
    [Category("UI Tests")]
    public class FrameCounterTestFacts : StyledControlTestFacts<FrameCounter>
    {
        [Target]
        public Orc.Controls.Automation.FrameRateCounter Target { get; set; }

        [Test]
        public void CorrectlySetPrefix()
        {
            var target = Target;
            var model = target.Current;

            const string prefix = "Test prefix: ";

            model.Prefix = prefix;

            //var textBlock = target.Find(controlType: ControlType.Text);

            //var currentValue = textBlock.TryGetDisplayText();
            
            //Assert.That(currentValue, Does.StartWith(prefix));

            //Wait.UntilResponsive(200);

            //var rateValue = currentValue.Replace(prefix, string.Empty);

            //Assert.That(int.TryParse(rateValue, out _), Is.True);
        }
    }
}
