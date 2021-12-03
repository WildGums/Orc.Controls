namespace Orc.Controls.Automation
{
    using System.Windows;
    using Orc.Automation;

    public class FilterBoxAutomationPeer : RunMethodAutomationPeerBase
    {
        public FilterBoxAutomationPeer(FrameworkElement owner)
            : base(owner)
        {
        }

        public override void SetValue(string value)
        {
            base.SetValue(value);
        }
    }
}
