namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation.Controls;

public class FrameRateCounter : FrameworkElement<FrameRateCounterModel>
{
    public FrameRateCounter(AutomationElement element)
        : base(element)
    {
    }
}
