namespace Orc.Controls.Tests
{
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    public class ExpanderTestFacts : ControlUiTestFactsBase<Expander>
    {
        [Target]
        public Automation.Expander Target { get; set; }

        [Test]
        public void CorrectlyExpand()
        {
            var target = Target;
            var view = target.View;

            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(target, view, nameof(target.IsExpanded), true, true, false);
        }
    }
}
