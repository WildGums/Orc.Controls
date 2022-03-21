namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation.Controls;

    public class FluidProgressBar : FrameworkElement<FluidProgressBarModel>
    {
        public FluidProgressBar(AutomationElement element) 
            : base(element)
        {
        }
    }
}
