namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation.Controls;

    public class TimePicker : FrameworkElement<TimePickerModel>
    {
        public TimePicker(AutomationElement element) 
            : base(element)
        {
        }
    }
}
