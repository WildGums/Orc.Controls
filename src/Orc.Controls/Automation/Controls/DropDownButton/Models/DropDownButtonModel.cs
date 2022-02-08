namespace Orc.Controls.Automation;

using System.Windows;
using Orc.Automation;

[AutomationAccessType]
public class DropDownButtonModel : ControlModel
{
    public DropDownButtonModel(AutomationElementAccessor accessor)
        : base(accessor)
    {
    }

    public bool IsChecked { get; set; }
    public object Header { get; set; }
    public DropdownArrowLocation ArrowLocation { get; set; }
    public Thickness ArrowMargin { get; set; }
    public bool IsArrowVisible { get; set; }
}