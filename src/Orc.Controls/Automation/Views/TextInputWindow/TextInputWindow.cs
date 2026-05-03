#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[Control(ControlTypeName = nameof(ControlType.Window))]
public class TextInputWindow : Window
{
    public TextInputWindow(AutomationElement element) 
        : base(element)
    {
    }

    private TextInputWindowMap Map => Map<TextInputWindowMap>();

    public string Text
    {
        get => Map.TextEdit.Text;
        set => Map.TextEdit.Text = value;
    }

    public void Accept() => Map.OkButton?.Click();
    public void Cancel() => Map.CancelButton?.Click();
}
#nullable enable
