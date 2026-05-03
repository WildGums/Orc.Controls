namespace Orc.Controls.Automation;

using System.Windows;
using System.Windows.Navigation;
using Orc.Automation;

public class LinkLabelAutomationPeer : AutomationControlPeerBase<Orc.Controls.LinkLabel>
{
    public LinkLabelAutomationPeer(Orc.Controls.LinkLabel owner) 
        : base(owner)
    {
        var control = Control;

        control.Click += OnClick;
        control.RequestNavigate += OnRequestNavigate;
    }
        
    private void OnClick(object sender, RoutedEventArgs e)
    {
        RaiseEvent(nameof(Orc.Controls.LinkLabel.Click), null);
    }

    private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        RaiseEvent(nameof(Orc.Controls.LinkLabel.RequestNavigate), null);
    }
}
