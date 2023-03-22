#nullable disable
namespace Orc.Controls.Automation;

using System;
using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(Controls.SpinButton))]
public class SpinButton : FrameworkElement
{
    private readonly SpinButtonMap _map;

    public SpinButton(AutomationElement element) 
        : base(element)
    {
        _map = new SpinButtonMap(element);
    }
        
    public void Increase()
    {
        _map.IncreaseButton.Click();
    }

    public void Decrease()
    {
        _map.DecreaseButton.Click();
    }

#pragma warning disable CS0067
    public event EventHandler<EventArgs> Increased;
    public event EventHandler<EventArgs> Decreased;
    public event EventHandler<EventArgs> Canceled;
#pragma warning restore CS0067
}
#nullable enable
