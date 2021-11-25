namespace Orc.Controls.Automation
{
    using Orc.Automation;
    using Orc.Automation.Tests;

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
}
