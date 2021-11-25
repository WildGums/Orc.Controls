namespace Orc.Controls.Tests
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows.Automation;
    using System.Windows.Media;
    using Automation;
    using Catel;
    using Catel.Data;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;
    using Window = Orc.Automation.Tests.WindowAutomationElement;

   // public static class ColorPicerMap

    public class ColorBoardMap
    {
        [ControlPart(AutomationId = "HSVSlider")]
        public SliderAutomationElement HsvSlider { get; set; }
        [ControlPart(AutomationId = "ASlider")]
        public SliderAutomationElement ASlider { get; set; }
        [ControlPart(AutomationId = "RSlider")]
        public SliderAutomationElement RSlider { get; set; }
        [ControlPart(AutomationId = "GSlider")]
        public SliderAutomationElement GSlider { get; set; }
        [ControlPart(AutomationId = "BSlider")]
        public SliderAutomationElement BSlider { get; set; }
    }

    public class ColorPickerMap
    {
        [Target]
        public ColorPickerAutomationElement Target { get; set; }

        [ControlPart(ControlType = nameof(ControlType.Button))]
        public ToggleButtonAutomationElement ToggleDropDown { get; set; }
    }

    public class ColorPickerScenario : AutomationControlScenario
    {
        private readonly ColorPickerAutomationElement _element;
        private readonly ColorPickerMap _map;
        private readonly ToggleButtonAutomationElement _toggleDropDown;

        [ControlPart(ControlType = nameof(ControlType.Button))]
        protected virtual ToggleButtonAutomationElement ToggleDropDown { get; set; }
        
        public ColorPickerScenario(ColorPickerAutomationElement element)
            : base(element)
        {
            _element = element;
            _map = _element.CreateControlMap<ColorPickerMap>();
            _toggleDropDown = _element.GetPart(controlType: ControlType.Button).As<ToggleButtonAutomationElement>();
        }

        public ColorBoardAutomationElement ShowColorEditDropDown()
        {
            var windowHost = _element.FindAncestor<Window>(x => Equals(x.Current.ControlType, ControlType.Window));
            if (windowHost is null)
            {
                return null;
            }

            ToggleDropDown.Toggle();

            Wait.UntilResponsive();

            var colorPopup = windowHost.Find(className: "Popup", controlType: ControlType.Window);
            var colorBoard = colorPopup?.Find(className: typeof(ColorBoard).FullName);
            if (colorBoard is null)
            {
                return null;
            }

            return new ColorBoardAutomationElement(colorBoard);
        }
    }

    [TestFixture(TestOf = typeof(ColorPicker))]
    [NUnit.Framework.Category("UI Tests")]
    public class ColorPickerTestFacts : ControlUiTestFactsBase<ColorPicker>
    {
        [Target]
        public ColorPickerAutomationElement Target { get; set; }

        [TargetControlMap]
        public ColorPickerMap TargetMap { get; set; }

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

