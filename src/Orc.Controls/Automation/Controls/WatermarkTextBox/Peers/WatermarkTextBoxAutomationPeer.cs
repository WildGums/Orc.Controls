#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation.Peers;

public class WatermarkTextBoxAutomationPeer : TextBoxAutomationPeer<Orc.Controls.WatermarkTextBox>
{
    public WatermarkTextBoxAutomationPeer(Orc.Controls.WatermarkTextBox owner) 
        : base(owner)
    {
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Edit;
    }
}
#nullable enable
