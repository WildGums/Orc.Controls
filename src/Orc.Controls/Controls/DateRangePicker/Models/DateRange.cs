namespace Orc.Controls;

using System;
using Catel.Data;

public class DateRange : ModelBase
{
    public string? Name { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public TimeSpan Duration => End.Subtract(Start);
    public bool IsTemporary { get; internal set; }

    public override string ToString()
    {
        return $"{Name} ({Start} => {End})";
    }
}
