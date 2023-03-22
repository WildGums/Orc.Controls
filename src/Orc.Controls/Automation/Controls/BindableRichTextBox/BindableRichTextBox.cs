namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(Controls.BindableRichTextBox))]
public class BindableRichTextBox : FrameworkElement<BindableRichTextBoxModel>
{
    public BindableRichTextBox(AutomationElement element) 
        : base(element)
    {
    }
}
