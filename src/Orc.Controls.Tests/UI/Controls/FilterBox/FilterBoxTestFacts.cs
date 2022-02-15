namespace Orc.Controls.Tests
{
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [TestFixture(TestOf = typeof(Expander))]
    [Category("UI Tests")]
    public class FilterBoxTestFacts : ControlUiTestFactsBase<FilterBox>
    {
        [Target]
        public Automation.FilterBox Target { get; set; }

        [Test]
        public void CorrectlyExpand()
        {
            var target = Target;
            var model = target.Current;

            model.Watermark = "TestWatermark";
        }
    }
}
