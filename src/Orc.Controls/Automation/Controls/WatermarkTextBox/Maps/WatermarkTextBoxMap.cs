#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class WatermarkTextBoxMap : AutomationBase
{
    public WatermarkTextBoxMap(AutomationElement element) 
        : base(element)
    {
    }

    public Text WatermarkText => By.One<Text>();
    public Edit Edit => Element.As<Edit>();
}
#nullable enable
