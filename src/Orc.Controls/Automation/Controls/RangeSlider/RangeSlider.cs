namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation.Controls;

public class RangeSlider : FrameworkElement<RangeSliderModel, RangeSliderMap>
{
    public RangeSlider(AutomationElement element)
        : base(element)
    {
    }

    public double Minimum
    {
        get => Map.LowerSlider.Minimum;
    }

    public double Maximum
    {
        get => Map.UpperSlider.Maximum;
    }

    public double LowerValue
    {
        get => Map.LowerSlider.Value;
        set => Map.LowerSlider.Value = value;
    }

    public double UpperValue
    {
        get => Map.UpperSlider.Value;
        set => Map.UpperSlider.Value = value;
    }
}
