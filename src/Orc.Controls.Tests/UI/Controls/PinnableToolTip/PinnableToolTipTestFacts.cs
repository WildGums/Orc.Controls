namespace Orc.Controls.UI
{
    using NUnit.Framework;
    using Orc.Automation;
    using Tests;

    public class PinnableToolTipTestFacts : StyledControlTestFacts<PinnableToolTip>
    {
        [Target]
        public Automation.PinnableToolTip Target { get; set; }

        [Test]
        public void CorrectlySaveContainingDirectory()
        {
            var target = Target;
            var model = target.Current;
        }
    }
}
