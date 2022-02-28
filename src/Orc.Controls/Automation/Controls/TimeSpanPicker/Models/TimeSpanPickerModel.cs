namespace Orc.Controls.Automation;

using System;
using Orc.Automation;

[AutomationAccessType]
public class TimeSpanPickerModel : FrameworkElementModel
{
    public TimeSpanPickerModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public TimeSpan? Value { get; set; }

    [ApiProperty(nameof(Controls.TimeSpanPicker.IsReadOnly))]
    public bool CanEdit { get; set; }
}
