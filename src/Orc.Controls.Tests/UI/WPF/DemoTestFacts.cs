namespace Orc.Controls.Tests.UI.WPF
{
    using System.Drawing;
    using System.Windows.Media;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Controls;
    using Orc.Automation.Tests;
    using Brushes = System.Windows.Media.Brushes;

    public class ButtonDemoTestFacts : ControlUiTestFactsBase<System.Windows.Controls.Button>
    {
        [Target]
        public Button Target { get; set; }

        public override void SetUp()
        {
            base.SetUp();
        }

        protected override bool TryLoadControl(TestHostAutomationControl testHost, out string testedControlAutomationId)
        {
            testedControlAutomationId = testHost.PutControl(typeof(System.Windows.Controls.Button).FullName);

            return true;
        }

        [Test]
        public void DemoTest()
        {
            var target = Target;
            var expectedBackgroundBrush = Brushes.Red;

            var background = target.Background;
            Assert.That(background, Is.Not.EqualTo(expectedBackgroundBrush));

            target.Background = expectedBackgroundBrush;
            Assert.That(target.Background.Color, Is.EqualTo(expectedBackgroundBrush.Color));
        }
    }
}
