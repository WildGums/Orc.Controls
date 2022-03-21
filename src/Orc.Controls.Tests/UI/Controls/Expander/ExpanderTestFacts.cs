namespace Orc.Controls.Tests
{
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [TestFixture(TestOf = typeof(Expander))]
    [Category("UI Tests")]
    public class ExpanderTestFacts : StyledControlTestFacts<Expander>
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
