namespace Orc.Controls.Automation;

using System.Windows.Automation.Peers;

public class FilterBoxAutomationPeer : TextBoxAutomationPeer<Orc.Controls.FilterBox>
{
    public FilterBoxAutomationPeer(Controls.FilterBox owner) 
        : base(owner)
    {
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Edit;
    }
}
