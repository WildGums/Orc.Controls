namespace Orc.Controls.Tests.UI
{
    using NUnit.Framework;
    using Orc.Automation;

    [Explicit]
    [TestFixture(TestOf = typeof(ValidationContextTree))]
    [Category("UI Tests")]
    public class ValidationContextTreeTestFacts : StyledControlTestFacts<ValidationContextTree>
    {
        [Target]
        public Automation.ValidationContextTree Target { get; set; }
            
        [Test]
        public void VerifyApi()
        {
            var target = Target;
            var model = target.Current;
        }
    }
} 
