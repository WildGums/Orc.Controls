#nullable disable
namespace Orc.Controls.Automation;

using System.Windows;
using System.Windows.Media;
using Orc.Automation;

[ActiveAutomationModel]
public class FluidProgressBarModel : FrameworkElementModel
{
    public FluidProgressBarModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public SolidColorBrush Foreground { get; set; }
    public Duration Delay { get; set; }
    public double DotWidth { get; set; }
    public double DotHeight { get; set; }
    public double DotRadiusX { get; set; }
    public double DotRadiusY { get; set; }
    public Duration DurationA { get; set; }
    public Duration DurationB { get; set; }
    public Duration DurationC { get; set; }
    public double KeyFrameA { get; set; }
    public double KeyFrameB { get; set; }
    public bool Oscillate { get; set; }
    public Duration ReverseDuration { get; set; }
    public Duration TotalDuration { get; set; }
}
#nullable enable
