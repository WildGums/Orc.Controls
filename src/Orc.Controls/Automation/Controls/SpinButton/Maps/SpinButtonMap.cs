#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class SpinButtonMap : AutomationBase
{
    public SpinButtonMap(AutomationElement element) 
        : base(element)
    {
    }

    public Button IncreaseButton => By.Id("PART_IncreaseButton").One<Button>();
    public Button DecreaseButton => By.Id("PART_DecreaseButton").One<Button>();
}
#nullable enable
