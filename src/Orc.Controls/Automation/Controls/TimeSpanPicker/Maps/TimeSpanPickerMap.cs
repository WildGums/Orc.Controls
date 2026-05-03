#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

public class TimeSpanPickerMap : AutomationBase
{
    public TimeSpanPickerMap(AutomationElement element) 
        : base(element)
    {
            
    }

    public NumericTextBox DaysNumericTextBox => By.Id("PART_DaysNumericTextBox").One<NumericTextBox>();
    public NumericTextBox HoursNumericTextBox => By.Id("PART_HoursNumericTextBox").One<NumericTextBox>();
    public NumericTextBox MinutesNumericTextBox => By.Id("PART_MinutesNumericTextBox").One<NumericTextBox>();
    public NumericTextBox SecondsNumericTextBox => By.Id("PART_SecondsNumericTextBox").One<NumericTextBox>();
    public NumericTextBox EditorNumericTextBox => By.Id("PART_EditorNumericTextBox").One<NumericTextBox>();
        
    public Text DaysHoursSeparatorTextBlock => By.Id("PART_DaysHoursSeparatorTextBlock").Raw().One<Text>();
    public Text DaysAbbreviationTextBlock => By.Id("PART_DaysAbbreviationTextBlock").Raw().One<Text>();
    public Text HoursMinutesSeparatorTextBlock => By.Id("PART_HoursMinutesSeparatorTextBlock").Raw().One<Text>();
    public Text HoursAbbreviationTextBlock => By.Id("PART_HoursAbbreviationTextBlock").Raw().One<Text>();
    public Text MinutesSecondsSeparatorTextBlock => By.Id("PART_MinutesSecondsSeparatorTextBlock").Raw().One<Text>();
    public Text MinutesAbbreviationTextBlock => By.Id("PART_MinutesAbbreviationTextBlock").Raw().One<Text>();
    public Text SecondsAbbreviationTextBlock => By.Id("PART_SecondsAbbreviationTextBlock").Raw().One<Text>();
    public Text EditedUnitTextBlock => By.Id("PART_EditedUnitTextBlock").Raw().One<Text>();
}
#nullable enable
