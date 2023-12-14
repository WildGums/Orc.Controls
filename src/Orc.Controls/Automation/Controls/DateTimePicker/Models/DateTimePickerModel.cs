#nullable disable
namespace Orc.Controls.Automation;

using System;
using System.Globalization;
using Enums;
using Orc.Automation;

[ActiveAutomationModel]
public class DateTimePickerModel : FrameworkElementModel
{
    public DateTimePickerModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public DayOfWeek? FirstDayOfWeek { get; set; }
    public CultureInfo Culture { get; set; }
    public DateTime? Value { get; set; }
    public bool ShowOptionsButton { get; set; }
    public bool AllowNull { get; set; }
    public bool AllowCopyPaste { get; set; }
    public bool HideTime { get; set; }
    public bool HideSeconds { get; set; }

    [ActiveAutomationProperty(nameof(Controls.DateTimePicker.IsReadOnly))]
    public bool IsControlReadOnly { get; set; }
    public string Format { get; set; }
    public bool IsYearShortFormat { get; }
    public bool IsHour12Format { get; }
    public bool IsAmPmShortFormat { get; }
    public TimeSpan? TimeValue { get; set; }
    public Meridiem AmPmValue { get; set; }
}
#nullable enable
