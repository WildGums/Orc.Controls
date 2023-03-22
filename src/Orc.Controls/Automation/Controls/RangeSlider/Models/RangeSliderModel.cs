#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Controls;
using Orc.Automation;

[ActiveAutomationModel]
public class RangeSliderModel : FrameworkElementModel
{
    public RangeSliderModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public double Minimum { get; set; }
    public double Maximum { get; set; }
    public double LowerValue { get; set; }
    public double UpperValue { get; set; }
    public bool HighlightSelectedRange { get; set; }
    public Orientation Orientation { get; set; }
}
#nullable enable
