namespace Orc.Controls;

using Catel;
using Catel.Logging;
using Microsoft.Extensions.Logging;

public class DateTimeFormatInfo
{
    public int DayPosition { get; set; }
    public int MonthPosition { get; set; }
    public int YearPosition { get; set; }
    public int? HourPosition { get; set; }
    public int? MinutePosition { get; set; }
    public int? SecondPosition { get; set; }
    public int? AmPmPosition { get; set; }
    public string? DayFormat { get; set; }
    public string? MonthFormat { get; set; }
    public string? YearFormat { get; set; }
    public string? HourFormat { get; set; }
    public string? MinuteFormat { get; set; }
    public string? SecondFormat { get; set; }
    public string? AmPmFormat { get; set; }
    public string? Separator0 { get; set; }
    public string? Separator1 { get; set; }
    public string? Separator2 { get; set; }
    public string? Separator3 { get; set; }
    public string? Separator4 { get; set; }
    public string? Separator5 { get; set; }
    public string? Separator6 { get; set; }
    public string? Separator7 { get; set; }
    public bool IsDateOnly => HourPosition is null && MinutePosition is null && SecondPosition is null && AmPmPosition is null;
    public bool IsYearShortFormat { get; set; }
    public bool? IsHour12Format { get; set; }
    public bool? IsAmPmShortFormat { get; set; }
    public int MaxPosition { get; set; }
    public bool IsCorrect { get; set; } = true;

    public string? GetSeparator(int position)
    {
        Argument.IsNotOutOfRange(() => position, 0, 7);

        return position switch
        {
            0 => Separator0,
            1 => Separator1,
            2 => Separator2,
            3 => Separator3,
            4 => Separator4,
            5 => Separator5,
            6 => Separator6,
            7 => Separator7,
            _ => null
        };
    }
}
