namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation.Controls;

public class HeaderBar : FrameworkElement<HeaderBarModel>
{
    public HeaderBar(AutomationElement element)
        : base(element)
    {
    }
}
