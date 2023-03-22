#nullable disable
namespace Orc.Controls.Automation;

using System.Globalization;
using Orc.Automation;

[ActiveAutomationModel]
public class NumericTextBoxModel : AutomationControlModel
{
    public NumericTextBoxModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }


    [ActiveAutomationProperty]
    public string Text { get; set; }

    [ActiveAutomationProperty]
    public string NullString { get; set; }

    [ActiveAutomationProperty]
    public CultureInfo CultureInfo { get; set; }

    [ActiveAutomationProperty]
    public bool IsChangeValueByUpDownKeyEnabled { get; set; }

    [ActiveAutomationProperty]
    public bool IsNullValueAllowed { get; set; }

    [ActiveAutomationProperty]
    public bool IsNegativeAllowed { get; set; }

    [ActiveAutomationProperty]
    public bool IsDecimalAllowed { get; set; }

    [ActiveAutomationProperty]
    public double MinValue { get; set; }

    [ActiveAutomationProperty]
    public double MaxValue { get; set; }

    [ActiveAutomationProperty]
    public string Format { get; set; }

    public double? Value
    {
        get => _accessor.GetValue<double?>(nameof(Value));
        set => _accessor.ExecuteAutomationMethod<SetNumericTextBoxValueRun>(value);
    }
}
#nullable enable
