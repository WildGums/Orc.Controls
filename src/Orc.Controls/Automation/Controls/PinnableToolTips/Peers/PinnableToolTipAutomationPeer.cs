#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;

public class PinnableToolTipAutomationPeer : AutomationControlPeerBase<Orc.Controls.PinnableToolTip>
{
    public PinnableToolTipAutomationPeer(Orc.Controls.PinnableToolTip owner)
        : base(owner)
    {
    }

    [AutomationMethod]
    public object GetOwner()
    {
        var owner = Control.Owner;

        var id = owner?.GetValue(AutomationProperties.AutomationIdProperty);

        return id;
    }
}
#nullable enable
