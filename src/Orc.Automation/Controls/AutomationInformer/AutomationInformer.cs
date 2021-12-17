namespace Orc.Automation
{
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;

    public class AutomationInformer : ContentControl
    {
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new AutomationInformerPeer(this);
        }
    }
}
