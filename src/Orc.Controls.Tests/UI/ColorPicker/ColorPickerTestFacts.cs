namespace Orc.Controls.Tests
{
    using System.Threading;
    using System.Windows.Media;
    using Automation;
    using Catel.IoC;
    using Catel.Runtime.Serialization;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Converters;
    using Orc.Automation.Tests;

    public class ColorPickerElementMap
    {

    }

    [TestFixture(TestOf = typeof(ColorPicker))]
    [Category("UI Tests")]
    public class ColorPickerTestFacts : ControlUiTestFactsBase<ColorPicker>
    {
        [Target]
        public ColorPickerAutomationElement Target { get; set; }

        [TargetControlMap]
        public ColorPickerElementMap TargetMap { get; set; }

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

        }
    }
}
