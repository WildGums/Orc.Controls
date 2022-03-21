namespace Orc.Controls.Tests.UI
{
    using System.Windows.Input;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [Explicit]
    [TestFixture(TestOf = typeof(SpinButton))]
    [Category("UI Tests")]
    public partial class SpinButtonFacts : StyledControlTestFacts<SpinButton>
    {
        [Target]
        public Automation.SpinButton Target { get; set; }

        [Test]
        public void CorrectlyIncrease()
        {
            var target = Target;

            EventAssert.Raised(target, nameof(target.Increased), () => target.Increase());
        }

        [Test]
        public void CorrectlyDecrease()
        {
            var target = Target;

            EventAssert.Raised(target, nameof(target.Decreased), () => target.Decrease());
        }

        [Test]
        public void CorrectlyCancel()
        {
            var target = Target;

            target.SetFocus();

            EventAssert.Raised(target, nameof(target.Canceled), () => target.MouseClick(MouseButton.Right));
        }
    }
}
