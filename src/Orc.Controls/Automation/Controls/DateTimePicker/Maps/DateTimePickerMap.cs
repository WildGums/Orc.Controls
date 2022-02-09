namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;

    public class DateTimePickerMap : AutomationBase
    {
        public DateTimePickerMap(AutomationElement element) 
            : base(element)
        {
        }

        public DropDownButton OptionDropDownButton => By.Id("PART_DatePickerIconDropDownButton").One<DropDownButton>();
    }
}
