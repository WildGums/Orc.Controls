namespace Orc.Controls.Tests.UI
{
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [Explicit]
    [TestFixture(TestOf = typeof(LinkLabel))]
    [Category("UI Tests")]
    public class LinkLabelTestFacts : StyledControlTestFacts<LinkLabel>
    {
        [Target]
        public Automation.LinkLabel Target { get; set; }

        [TestCase("Test content")]
        [TestCase("")]
        public void CorrectlySetContent(string content)
        {
            var target = Target;
            var model = target.Current;

            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(target, model, nameof(target.Content), true, content);
        }

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
