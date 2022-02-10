namespace Orc.Controls.Tests
{
    using System.Windows.Automation;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [TestFixture(TestOf = typeof(AnimatingTextBlock))]
    [Category("UI Tests")]
    public class AnimatingTextBlockTestFacts : StyledControlTestFacts<AnimatingTextBlock>
    {
        [Target]
        public Orc.Controls.Automation.AnimatingTextBlock Target { get; set; }

        [TestCase("Test text")]
        public void CorrectlySetText(string testText)
        {
            var target = Target;

            var model = target.Current;

            model.Text = testText;

            Wait.UntilResponsive();

            var textBlocks = target.By.ControlType(ControlType.Text).Many<Text>();

            Assert.That(textBlocks, Has.One.With.Property(nameof(Text.Value)).EqualTo(testText));
        }
    }
}
