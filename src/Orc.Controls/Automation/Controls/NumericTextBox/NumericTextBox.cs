namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation.Controls;

    public class NumericTextBox : FrameworkElement<NumericTextBoxModel>
    {
        public NumericTextBox(AutomationElement element) 
            : base(element)
        {
        }
    }
}
