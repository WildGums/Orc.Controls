namespace Orc.Controls.Automation
{
    using System;
    using System.Linq;
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(Class = typeof(Controls.DateTimePicker))]
    public class DateTimePicker : FrameworkElement<DateTimePickerModel, DateTimePickerMap>
    {
        public DateTimePicker(AutomationElement element)
            : base(element)
        {
            
        }

        public void SetTodayValue()
        {
            SelectMenuOption("Today");
        }

        public void SetNowValue()
        {
            SelectMenuOption("Now");
        }

        public void SelectDateFromCalendar(DateTime? date)
        {
            var calendar = OpenCalendar();
            if (calendar is null)
            {
                return;
            }

            calendar.SelectedDate = date;
        }

        public void CopyDate()
        {
            SelectMenuOption("Copy");
        }

        public void PasteDate()
        {
            SelectMenuOption("Paste");
        }

        private Calendar OpenCalendar()
        {
            SelectMenuOption("Select date");

            Wait.UntilResponsive();

            var windowHost = Element.Find<Window>(controlType: ControlType.Window, scope: TreeScope.Ancestors);
            if (windowHost is null)
            {
                return null;
            }

            Wait.UntilResponsive();

            return windowHost.Find(controlType: ControlType.Calendar, id: "CalendarId").As<Calendar>();
        }

        public void OpenSelectTimeDialog()
        {
            SelectMenuOption("Select time");
        }

        private void SelectMenuOption(string optionName)
        {
            var dropDownButton = Map.OptionDropDownButton;

            var menu = dropDownButton.OpenDropDown();
            var todayMenuItem = menu.Items.FirstOrDefault(x => x.Element.TryGetDisplayText() == optionName); 
            
            todayMenuItem?.Click();


            //dropDownButton.InvokeInDropDownScope(menu =>
            //{
            //    var todayMenuItem = menu.Items.FirstOrDefault(x => x.Element.TryGetDisplayText() == optionName);
            //    todayMenuItem?.Click();

            //    return true;
            //});
        }
    }
}
