namespace Orc.Controls.Tests
{
    using System.Windows.Media;
    using Automation;
    using Catel.IoC;
    using Catel.Runtime.Serialization;
    using NUnit.Framework;
    using Orc.Automation;
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

            var serializationManager = this.GetServiceLocator().ResolveType<ISerializationManager>();

            serializationManager.AddSerializerModifier(typeof(Color), typeof(ColorSerializerModifier));

            //var colorStr = XmlSerializerHelper.SerializeValue(Colors.Red);
            //var newColor = this.GetTypeFactory().CreateInstance(typeof(Color));

            //var color = XmlSerializerHelper.DeserializeValue(colorStr, typeof(Color));

            //Target.Color = newColor;
        }

        [Test]
        public void CorrectlySetColor()
        {

        }
    }
}
