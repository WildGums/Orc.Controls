#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(ControlTypeName = nameof(ControlType.Edit), Class = typeof(Controls.NumericTextBox))]
public class NumericTextBox : Edit
{
    public NumericTextBox(AutomationElement element) 
        : base(element)
    {
    }

    public new NumericTextBoxModel Current => Model<NumericTextBoxModel>();
}
#nullable enable
