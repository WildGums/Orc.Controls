#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class FilterBoxMap : AutomationBase
{
    public FilterBoxMap(AutomationElement element) 
        : base(element)
    {
    }

    public Button ClearButton => By.One<Button>();
    public Text WatermarkText => By.One<Text>();
    public Edit Edit => Element.As<Edit>();
}
#nullable enable
