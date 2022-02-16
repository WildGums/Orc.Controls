namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation.Controls;

    public class FrameCounter : FrameworkElement<FrameRateCounterModel>
    {
        public FrameCounter(AutomationElement element)
            : base(element)
        {
        }
    }
}
