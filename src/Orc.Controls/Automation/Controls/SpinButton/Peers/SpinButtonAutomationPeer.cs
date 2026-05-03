#nullable disable
namespace Orc.Controls.Automation;

using System.Windows;
using Orc.Automation;

public class SpinButtonAutomationPeer : AutomationControlPeerBase<Orc.Controls.SpinButton>
{
    public SpinButtonAutomationPeer(Orc.Controls.SpinButton owner) 
        : base(owner)
    {
        owner.Increased += OnIncreased;
        owner.Decreased += OnDecreased;
        owner.Canceled += OnCanceled;
    }
        
    private void OnIncreased(object sender, RoutedEventArgs e)
    {
        RaiseEvent(nameof(Orc.Controls.SpinButton.Increased), null);
    }

    private void OnDecreased(object sender, RoutedEventArgs e)
    {
        RaiseEvent(nameof(Orc.Controls.SpinButton.Decreased), null);
    }

    private void OnCanceled(object sender, RoutedEventArgs e)
    {
        RaiseEvent(nameof(Orc.Controls.SpinButton.Canceled), null);
    }
}
#nullable enable
