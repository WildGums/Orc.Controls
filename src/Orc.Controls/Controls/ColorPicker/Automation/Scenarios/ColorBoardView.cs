namespace Orc.Controls.Automation
{
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class ColorBoardView : AutomationBase
    {
        public ColorBoardView(ColorBoardAutomationControl target)
            : base(target.Element)
        {
        }

        protected Slider HSVSlider => By.Id().One<Slider>();
        protected Slider ASlider => By.Id().One<Slider>();
        protected Slider RSlider => By.Id().One<Slider>();
        protected Slider GSlider => By.Id().One<Slider>();
        protected Slider BSlider => By.Id().One<Slider>();
        protected Button SelectButton => By.Id().One<Button>();
        protected Button CancelButton => By.Id().One<Button>();


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
