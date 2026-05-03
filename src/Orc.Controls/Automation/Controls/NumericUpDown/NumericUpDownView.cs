#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;

public class NumericUpDownView : AutomationBase
{
    private readonly NumericUpDownMap _map;

    public NumericUpDownView(AutomationElement element) 
        : base(element)
    {
        _map = new NumericUpDownMap(element);
    }

    public string Value
    {
        get => _map.Edit.Text;
        set => _map.Edit.Text = value;
    }

    public void IncreaseNumber()
    {
        _map.SpinButton.Increase();
    }

    public void DecreaseNumber()
    {
        _map.SpinButton.Decrease();
    }
}
#nullable enable
