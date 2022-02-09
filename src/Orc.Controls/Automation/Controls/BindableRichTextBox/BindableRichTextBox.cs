namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation.Controls;

    public class BindableRichTextBox : FrameworkElement<BindableRichTextBoxModel>
    {
        public BindableRichTextBox(AutomationElement element) 
            : base(element)
        {
        }
    }
}
