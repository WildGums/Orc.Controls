namespace Orc.Controls.Tests
{
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [TestFixture(TestOf = typeof(NumericUpDown))]
    [Category("UI Tests")]
    public class ListTextBoxTestFacts : ControlUiTestFactsBase<ListTextBox>
    {
        [Target]
        public Automation.ListTextBox Target { get; set; }

        [Test]
        public void Correctly()
        {

        }
    }
}
