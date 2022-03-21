namespace Orc.Controls.Tests
{
    using System.Windows.Media;
    using NUnit.Framework;
    using Orc.Automation;
    
    [TestFixture(TestOf = typeof(FluidProgressBar))]
    [Category("UI Tests")]
    public class FluidProgressBarTestFacts : StyledControlTestFacts<FluidProgressBar>
    {
        [Target]
        public Automation.FluidProgressBar Target { get; set; }

        [Test]
        public void CorrectlyInitialize()
        {
            var target = Target;

            var model = target.Current;

            model.Foreground = Brushes.Red;

            model.DotWidth = 30d;
        }
    }
}
