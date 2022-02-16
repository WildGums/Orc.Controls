namespace Orc.Controls.Tests
{
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [TestFixture(TestOf = typeof(LinkLabel))]
    [NUnit.Framework.Category("UI Tests")]
    public class LinkLabelTestFacts : StyledControlTestFacts<LinkLabel>
    {
        [Target]
        public Orc.Controls.Automation.LinkLabel Target { get; set; }

        [Test]
        public void CorrectlyRespondToClick()
        {
            var target = Target;
            var model = target.Current;

            //Fill content to provide clickable space 
            model.Content = "Link";
            
            EventAssert.Raised(target, nameof(target.Click), () => target.Invoke());
        }
    }
}
