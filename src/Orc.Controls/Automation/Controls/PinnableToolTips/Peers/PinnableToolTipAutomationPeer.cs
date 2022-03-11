namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;

    public class PinnableToolTipAutomationPeer : ControlRunMethodAutomationPeerBase<Controls.PinnableToolTip>
    {
        public PinnableToolTipAutomationPeer(Controls.PinnableToolTip owner)
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
}
