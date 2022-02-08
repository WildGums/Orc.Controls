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
            var model = target.Current;

            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(target, model, nameof(target.IsExpanded), true, true, false);
        }
    }
}
