namespace Orc.Controls.Tests
{
    using NUnit.Framework;
    using Orc.Automation;

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
