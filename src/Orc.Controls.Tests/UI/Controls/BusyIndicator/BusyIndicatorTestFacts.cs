namespace Orc.Controls.Tests
{
    using System.Windows.Media;
    using NUnit.Framework;
    using Orc.Automation;
    
    [TestFixture(TestOf = typeof(BusyIndicator))]
    [Category("UI Tests")]
    public class BusyIndicatorTestFacts : StyledControlTestFacts<BusyIndicator>
    {
        [Target]
        public Automation.BusyIndicator Target { get; set; }

        [Test]
        public void CorrectlyInitialize()
        {
            var target = Target;

            var model = target.Current;

            model.Foreground = Brushes.Red;
        }
    }
}
