namespace Orc.Controls.Automation
{
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
            if (patternInterface is PatternInterface.Toggle)
            {
                return new ToggleButtonAutomationPeer((ToggleButton)Owner);
            }

            return base.GetPattern(patternInterface);
        }
    }
}
