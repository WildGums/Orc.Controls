namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation.Controls;

    public class ListTextBox : Edit
    {
        public ListTextBox(AutomationElement element)
            : base(element)
        {
        }

        public ListTextBoxModel Current => Model<ListTextBoxModel>();
    }
}
