namespace Orc.Controls;

using System;
using Catel.Data;

public class DateRange : ModelBase
{
    public string? Name { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public TimeSpan Duration => End.Subtract(Start);
    public bool IsTemporary { get; internal set; }

    public override string ToString()
    {
        return $"{Name} ({Start} => {End})";
    }
}
