namespace Orc.Controls.Automation
{
    using Orc.Automation;
    using Orc.Automation.Tests;

    public class ColorBoardUserActions : AutomationBase
    {
        public ColorBoardUserActions(ColorBoardAutomationControl target)
            : base(target.Element)
        {
        }

        protected SliderAutomationControl HSVSlider => By.Id().One<SliderAutomationControl>();
        protected SliderAutomationControl ASlider => By.Id().One<SliderAutomationControl>();
        protected SliderAutomationControl RSlider => By.Id().One<SliderAutomationControl>();
        protected SliderAutomationControl GSlider => By.Id().One<SliderAutomationControl>();
        protected SliderAutomationControl BSlider => By.Id().One<SliderAutomationControl>();
        protected ButtonAutomationControl SelectButton => By.Id().One<ButtonAutomationControl>();
        protected ButtonAutomationControl CancelButton => By.Id().One<ButtonAutomationControl>();


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
