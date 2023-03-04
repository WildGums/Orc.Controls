#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class DateTimePickerMap : AutomationBase
{
    public DateTimePickerMap(AutomationElement element) 
        : base(element)
    {
    }

    public DropDownButton OptionDropDownButton => By.Id("PART_DatePickerIconDropDownButton").One<DropDownButton>();
    public NumericTextBox DaysTextBox => By.Id("PART_DaysNumericTextBox").One<NumericTextBox>();
    public NumericTextBox MonthTextBox => By.Id("PART_MonthNumericTextBox").One<NumericTextBox>();
    public NumericTextBox YearTextBox => By.Id("PART_YearNumericTextBox").One<NumericTextBox>();
    public NumericTextBox HourTextBox => By.Id("PART_HourNumericTextBox").One<NumericTextBox>();
    public NumericTextBox MinuteTextBox => By.Id("PART_MinuteNumericTextBox").One<NumericTextBox>();
    public NumericTextBox SecondTextBox => By.Id("PART_SecondNumericTextBox").One<NumericTextBox>();

    public ListTextBox AmPmTextBox => By.Id("PART_AmPmListTextBox").One<ListTextBox>();

    public ToggleButton DaysToggleButton
    {
        get
        {
            Element.MouseHover();

            return By.Id("PART_DaysToggleButton").One<ToggleButton>();
        }
    }
        
    public ToggleButton MonthsToggleButton => By.Id("PART_MonthToggleButton").One<ToggleButton>();
    public ToggleButton YearToggleButton => By.Id("PART_YearToggleButton").One<ToggleButton>();
    public ToggleButton HourToggleButton => By.Id("PART_HourToggleButton").One<ToggleButton>();
    public ToggleButton MinuteToggleButton => By.Id("PART_MinuteToggleButton").One<ToggleButton>();
    public ToggleButton SecondToggleButton => By.Id("PART_SecondToggleButton").One<ToggleButton>();
    public ToggleButton AmPmToggleButton => By.Id("PART_AmPmToggleButton").One<ToggleButton>();
}
#nullable enable
