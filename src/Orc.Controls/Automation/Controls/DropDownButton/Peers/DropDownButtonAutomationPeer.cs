namespace Orc.Controls.Automation;

using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using Orc.Automation;

public class DropDownButtonAutomationPeer : AutomationControlPeerBase<Orc.Controls.DropDownButton>
{
    public DropDownButtonAutomationPeer(Controls.DropDownButton owner)
        : base(owner)
    {
           
    }

    public override object GetPattern(PatternInterface patternInterface)
    {
        return patternInterface is PatternInterface.Toggle 
            ? new ToggleButtonAutomationPeer((ToggleButton)Owner) 
            : base.GetPattern(patternInterface);
    }
}
