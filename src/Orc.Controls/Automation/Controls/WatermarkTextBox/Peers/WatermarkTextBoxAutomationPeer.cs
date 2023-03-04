#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation.Peers;

public class WatermarkTextBoxAutomationPeer : TextBoxAutomationPeer<Controls.WatermarkTextBox>
{
    public WatermarkTextBoxAutomationPeer(Controls.WatermarkTextBox owner) 
        : base(owner)
    {
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Edit;
    }
}
#nullable enable
