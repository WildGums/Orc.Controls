namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class NumericTextBox : FrameworkElement<NumericTextBoxModel>
    {
        public NumericTextBox(AutomationElement element) 
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
