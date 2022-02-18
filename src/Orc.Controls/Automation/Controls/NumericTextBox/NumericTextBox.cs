namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class NumericTextBox : Edit
    {
        public NumericTextBox(AutomationElement element) 
            : base(element)
        {
        }

        public NumericTextBoxModel Current => Model<NumericTextBoxModel>();
    }
}
