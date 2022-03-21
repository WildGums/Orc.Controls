namespace Orc.Controls.Automation;

using System.Collections.Generic;
using System.Globalization;
using Orc.Automation;

public class CulturePickerModel : ControlModel
{
    public CulturePickerModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    [ActiveAutomationProperty]
    public CultureInfo SelectedCulture { get; set; }

    public List<CultureInfo> AvailableCultures =>
        (List<CultureInfo>)_accessor.Execute(nameof(CulturePickerAutomationPeer.GetAvailableCultures));
}
