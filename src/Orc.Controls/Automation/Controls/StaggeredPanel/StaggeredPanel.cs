namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;


[AutomatedControl(Class = typeof(Controls.StaggeredPanel))]
public class StaggeredPanel : FrameworkElement<StaggeredPanelModel, StaggeredPanelMap>
{
    public StaggeredPanel(AutomationElement element)
        : base(element)
    {
    }
}
