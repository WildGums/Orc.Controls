namespace Orc.Controls;

using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using Automation;

public class HeaderBar : Control
{
    public HeaderBar()
    {
        DefaultStyleKey = typeof(HeaderBar);
    }
    
    public string? Header
    {
        get { return (string?)GetValue(HeaderProperty); }
        set { SetValue(HeaderProperty, value); }
    }

    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header),
        typeof(string), typeof(HeaderBar), new PropertyMetadata(string.Empty));

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new HeaderBarAutomationPeer(this);
    }
}
