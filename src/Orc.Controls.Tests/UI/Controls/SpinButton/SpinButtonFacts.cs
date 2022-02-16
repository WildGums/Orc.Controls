namespace Orc.Controls.Tests
{
    using System.Windows.Input;
    using NUnit.Framework;
    using Orc.Automation;

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

            var beenIncreased = false;

            target.Increased += delegate { beenIncreased = true; };

            target.Increase();

            Wait.UntilResponsive(200);

            Assert.That(beenIncreased, Is.True);
        }

        [Test]
        public void CorrectlyDecrease()
        {
            var target = Target;

            var beenDecreased = false;

            target.Decreased += delegate { beenDecreased = true; };

            target.Decrease();

            Wait.UntilResponsive(200);

            Assert.That(beenDecreased, Is.True);
        }

        [Test]
        public void CorrectlyCancel()
        {
            var target = Target;

            var beenCanceled = false;

            target.Canceled += delegate { beenCanceled = true; };

            target.SetFocus();
            target.MouseClick(MouseButton.Right);

            Wait.UntilResponsive(200);

            Assert.That(beenCanceled, Is.True);
        }
    }
}
