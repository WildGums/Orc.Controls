#nullable disable
namespace Orc.Controls.Automation;

using System;
using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(Orc.Controls.DateTimePicker))]
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

    public void CopyDate()
    {
        SelectMenuOption("Copy");
    }

    public void PasteDate()
    {
        SelectMenuOption("Paste");
    }

    public void SelectDay(int dayInMonth)
    {
        Map.DaysToggleButton.Toggle();

        Wait.UntilResponsive();

        SelectDatePartFromList(dayInMonth);
    }

    public void SelectMonth(int month)
    {
        Map.MonthsToggleButton.Toggle();

        Wait.UntilResponsive();

        SelectDatePartFromList(month);
    }

    public void SelectHour(int hour)
    {
        Map.HourToggleButton.Toggle();

        Wait.UntilResponsive();

        SelectDatePartFromList(hour);
    }

    public void SelectMinute(int minute)
    {
        Map.MinuteToggleButton.Toggle();

        Wait.UntilResponsive();

        SelectDatePartFromList(minute);
    }

    public void SelectSecond(int second)
    {
        Map.SecondToggleButton.Toggle();

        Wait.UntilResponsive();

        SelectDatePartFromList(second);
    }

    private void SelectDatePartFromList(int partIndex)
    {
        var list = Element.GetHostWindow()
            .Find<List>(id: "DatePartListBoxId", controlType: ControlType.List);

        var virtualizedItem = list.TryGetVirtualizedItem(partIndex - 1);

        Wait.UntilResponsive();

        virtualizedItem.MouseClick();
    }

    public DateTime? Value
    {
        get
        {
            var map = Map;

            var year = map.YearTextBox.Current.Value;
            if (year is null)
            {
                return null;
            }

            var month = map.MonthTextBox.Current.Value;
            if (month is null)
            {
                return null;
            }

            var day = map.DaysTextBox.Current.Value;
            if (day is null)
            {
                return null;
            }

            var ampm = map.AmPmTextBox?.Current.Value;
            var hour = (int)(map.HourTextBox.Current.Value ?? 1);
            if (ampm == "PM" && hour != 12)
            {
                hour += 12;
            }
                
            return new DateTime((int)year, (int)month, (int)day,
                hour,
                (int)(map.MinuteTextBox.Current.Value ?? 1),
                (int)(map.SecondTextBox.Current.Value ?? 1));
        }

        set
        {
            var map = Map;

            map.YearTextBox.Current.Value = value?.Year;
            map.MonthTextBox.Current.Value = value?.Month;
            map.DaysTextBox.Current.Value = value?.Day;
            map.HourTextBox.Current.Value = value?.Hour;
            map.MinuteTextBox.Current.Value = value?.Minute;
            map.SecondTextBox.Current.Value = value?.Second;
        }
    }

    public Calendar OpenCalendar()
    {
        SelectMenuOption("Select date");

        Wait.UntilResponsive();

        var windowHost = Element.GetHostWindow();

        Wait.UntilResponsive();

        return windowHost?.Find(controlType: ControlType.Calendar, id: "CalendarId").As<Calendar>();
    }

    public void OpenSelectTimeDialog()
    {
        SelectMenuOption("Select time");
    }

    private void SelectMenuOption(string optionName)
    {
        var dropDownButton = Map.OptionDropDownButton;

        var menu = dropDownButton.OpenDropDown();

        var menuItem = menu.Element.Find(controlType: ControlType.MenuItem, name: optionName);

        menuItem?.Invoke();

        //     var todayMenuItem = menu.Items.FirstOrDefault(x => x.Element.TryGetDisplayText() == optionName); 

        // todayMenuItem?.Click();


        //dropDownButton.InvokeInDropDownScope(menu =>
        //{
        //    var todayMenuItem = menu.Items.FirstOrDefault(x => x.Element.TryGetDisplayText() == optionName);
        //    todayMenuItem?.Click();

        //    return true;
        //});
    }
}
#nullable enable
