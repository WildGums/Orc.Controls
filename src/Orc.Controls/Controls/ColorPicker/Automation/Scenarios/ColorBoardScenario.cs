namespace Orc.Controls.Automation
{
    using Orc.Automation;
    using Orc.Automation.Tests;

    public class ColorBoardScenario : AutomationControlScenario
    {
        public ColorBoardScenario(ColorBoardAutomationControl target)
            : base(target)
        {
        }

        [ControlPart]
        protected SliderAutomationControl HSVSlider { get; set; }

        [ControlPart]
        protected SliderAutomationControl ASlider { get; set; }

        [ControlPart]
        protected SliderAutomationControl RSlider { get; set; }

        [ControlPart]
        protected SliderAutomationControl GSlider { get; set; }

        [ControlPart]
        protected SliderAutomationControl BSlider { get; set; }

        [ControlPart]
        protected ButtonAutomationControl SelectButton { get; set; }

        [ControlPart]
        protected ButtonAutomationControl CancelButton { get; set; }


        public bool Apply()
        {
            return SelectButton.TryInvoke();
        }

        public bool Cancel()
        {
            return CancelButton.TryInvoke();
        }


        public void SelectARgbColor(int a, int r, int g, int b)
        {
            ASlider.Value = a;
            Wait.UntilResponsive();

            RSlider.Value = r;
            Wait.UntilResponsive();

            GSlider.Value = g;
            Wait.UntilResponsive();

            BSlider.Value = b;
            Wait.UntilResponsive();
        }
    }
}
