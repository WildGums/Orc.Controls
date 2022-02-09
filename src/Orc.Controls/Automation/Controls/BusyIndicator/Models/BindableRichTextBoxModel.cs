namespace Orc.Controls.Automation;

using System.Windows.Media;
using Orc.Automation;

[AutomationAccessType]
public class BusyIndicatorModel : ControlModel
{
    public BusyIndicatorModel(AutomationElementAccessor accessor)
        : base(accessor)
    {
    }

    public SolidColorBrush Foreground { get; set; }

    public int IgnoreUnloadedEventCount { get; set; }
}
