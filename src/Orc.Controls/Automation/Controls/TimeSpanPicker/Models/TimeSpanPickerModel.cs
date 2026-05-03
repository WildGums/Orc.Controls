namespace Orc.Controls.Automation;

using System;
using Orc.Automation;

[ActiveAutomationModel]
public class TimeSpanPickerModel : FrameworkElementModel
{
    public TimeSpanPickerModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public TimeSpan? Value { get; set; }

    [ActiveAutomationProperty(nameof(Orc.Controls.TimeSpanPicker.IsReadOnly))]
    public bool CanEdit { get; set; }
}
