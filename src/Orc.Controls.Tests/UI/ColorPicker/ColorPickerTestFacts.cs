namespace Orc.Controls.Tests
{
    using System.Threading;
    using System.Windows.Media;
    using Automation;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [TestFixture(TestOf = typeof(ColorPicker))]
    [NUnit.Framework.Category("UI Tests")]
    public class ColorPickerTestFacts : ControlUiTestFactsBase<ColorPicker>
    {
        [Target]
        public ColorPickerAutomationElement Target { get; set; }

        [SetUp]
        public override void SetUpTest()
        {
            base.SetUpTest();

            var color = Target.Color;

            Thread.Sleep(200);

            Target.Color = Colors.Red;
        }

        [Test]
        public void CorrectlySetColor()
        {
            var target = Target;

            var scenario = target.CreateScenario<ColorPickerScenario>();

            var colorBoard = scenario.ShowColorEditDropDown();
            Assert.That(colorBoard, Is.Not.Null);
            Assert.That(target.IsDropDownOpen, Is.True);


        }
    }
}

