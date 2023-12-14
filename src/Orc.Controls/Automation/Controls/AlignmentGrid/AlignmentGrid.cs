namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(Controls.AlignmentGrid))]
public class AlignmentGrid : FrameworkElement<AlignmentGridModel>
{
    public AlignmentGrid(AutomationElement element) 
        : base(element)
    {
    }
}
