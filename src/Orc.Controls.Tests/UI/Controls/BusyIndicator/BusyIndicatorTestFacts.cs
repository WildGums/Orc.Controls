namespace Orc.Controls.Tests.UI
{
    using System.Windows.Media;
    using NUnit.Framework;
    using Orc.Automation;

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
