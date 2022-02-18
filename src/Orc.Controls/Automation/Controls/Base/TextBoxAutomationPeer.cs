namespace Orc.Controls.Automation;

using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using Orc.Automation;

public class TextBoxAutomationPeer<TControl> : ControlRunMethodAutomationPeerBase<TControl>
    where TControl : TextBox
{
    public TextBoxAutomationPeer(TControl owner) 
        : base(owner)
    {
    }

    public override object GetPattern(PatternInterface patternInterface)
    {
        if (patternInterface == PatternInterface.Text)
        {
            return new TextBoxAutomationPeer(Control).GetPattern(patternInterface);
        }

        return base.GetPattern(patternInterface);
    }

    protected override string GetValueFromPattern()
    {
        var valueProvider = GetValueProvider();
        return valueProvider?.Value;
    }

    protected override void SetValuePatternInvoke(string value)
    {
        var valueProvider = GetValueProvider();
        valueProvider?.SetValue(value);
    }

    private IValueProvider GetValueProvider()
    {
        var textBoxAutomationPeer = new TextBoxAutomationPeer(Control);
        return textBoxAutomationPeer.GetPattern(PatternInterface.Value) as IValueProvider;
    }
}
