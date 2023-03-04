#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class StepBarItemMap : AutomationBase
{
    public StepBarItemMap(AutomationElement element) 
        : base(element)
    {
    }

    public Text TitleTextBlock => By.Id("TitleTextBlock").Raw().One<Text>();
    public Text EllipseTextBlock => By.Id("EllipseTextBlock").Raw().One<Text>();
    public Button ExecuteButton => By.Raw().One<Button>();
}
#nullable enable
