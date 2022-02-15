namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class ListTextBox : FrameworkElement<ListTextBoxModel>
    {
        public ListTextBox(AutomationElement element)
            : base(element)
        {
        }

        public string Text
        {
            get => Element.GetValue<string>();
            set => Element.SetValue(value);
        }
    }
}
