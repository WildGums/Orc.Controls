namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(ControlTypeName = nameof(ControlType.Edit))]
public class ListTextBox : Edit
{
    public ListTextBox(AutomationElement element)
        : base(element)
    {
    }

    public new ListTextBoxModel Current => Model<ListTextBoxModel>();
}
