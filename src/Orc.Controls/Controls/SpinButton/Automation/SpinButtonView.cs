namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;

    public class SpinButtonView : AutomationBase
    {
        private readonly SpinButtonMap _map;

        public SpinButtonView(AutomationElement element)
            : base(element)
        {
            _map = new SpinButtonMap(element);
        }

        public void Increase()
        {
            _map.IncreaseButton.Click();
        }

        public void Decrease()
        {
            _map.DecreaseButton.Click();
        }
    }
}
