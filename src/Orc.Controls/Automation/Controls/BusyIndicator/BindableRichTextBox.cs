namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(Class = typeof(Controls.BusyIndicator))]
    public class BusyIndicator : FrameworkElement<BusyIndicatorModel>
    {
        public BusyIndicator(AutomationElement element) 
            : base(element)
        {
        }
    }
}
