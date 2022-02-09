namespace Orc.Controls.Tests.UI
{
    using System.Windows.Media;
    using NUnit.Framework;
    using Orc.Automation;

    public class AnimatingTextBlockTestFacts : StyledControlTestFacts<AnimatingTextBlock>
    {
        [Target]
        public Automation.AnimatingTextBlock Target { get; set; }

        [Test]
        public void CorrectlyInitialize()
        {
            var target = Target;

            var model = target.Current;

            model.Text = "Test text";
        }
    }
}
