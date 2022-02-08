namespace Orc.Controls.Tests.UI
{
    using System.Windows.Input;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    public partial class SpinButtonFacts : ControlUiTestFactsBase<SpinButton>
    {
        [Target]
        public Automation.SpinButton Target { get; set; }

        [Test]
        public void CorrectlyIncrease()
        {
            var target = Target;
            var view = target.View;

            var beenIncreased = false;

            target.Increased += delegate { beenIncreased = true; };

            view.Increase();

            Wait.UntilResponsive(200);

            Assert.That(beenIncreased, Is.True);
        }

        [Test]
        public void CorrectlyDecrease()
        {
            var target = Target;
            var view = target.View;

            var beenDecreased = false;

            target.Decreased += delegate { beenDecreased = true; };

            view.Decrease();

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
