namespace Orc.Controls.Automation
{
    using System.Windows;
    using System.Windows.Navigation;
    using Orc.Automation;

    public class LinkLabelAutomationPeer : AutomationControlPeerBase<Controls.LinkLabel>
    {
        public LinkLabelAutomationPeer(Controls.LinkLabel owner) 
            : base(owner)
        {
            var control = Control;

            control.Click += OnClick;
            control.RequestNavigate += OnRequestNavigate;
        }
        
        private void OnClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(nameof(Controls.LinkLabel.Click), null);
        }

        private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            RaiseEvent(nameof(Controls.LinkLabel.RequestNavigate), null);
        }
    }
}
