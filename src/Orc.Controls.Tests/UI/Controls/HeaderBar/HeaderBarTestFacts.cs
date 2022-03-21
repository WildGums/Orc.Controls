namespace Orc.Controls.Tests.UI
{
    using NUnit.Framework;
    using Orc.Automation;

    [Explicit]
    [TestFixture(TestOf = typeof(FrameRateCounter))]
    [Category("UI Tests")]
    public class HeaderBarTestFacts : StyledControlTestFacts<HeaderBar>
    {
        [Target]
        public Orc.Controls.Automation.HeaderBar Target { get; set; }

        [Test]
        public void CorrectlySetHeader()
        {
            var target = Target;
            var model = target.Current;

            const string header = "Test header";

            model.Header = header;

            Wait.UntilResponsive(500);

            var text = target.Element.TryGetDisplayText();

            Assert.That(text, Is.EqualTo(header));
        }
    }
}
