#nullable disable
namespace Orc.Controls.Automation;

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Orc.Automation;

[ActiveAutomationModel]
public class PinnableToolTipModel : FrameworkElementModel
{
    public PinnableToolTipModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }
    
    [ActiveAutomationProperty]
    public bool AllowCloseByUser { get; set; }
    [ActiveAutomationProperty]
    public double HorizontalOffset { get; set; }
    [ActiveAutomationProperty]
    public bool IsPinned { get; set; }
    [ActiveAutomationProperty]
    public Color GripColor { get; set; }
    [ActiveAutomationProperty]
    public double VerticalOffset { get; set; }
    [ActiveAutomationProperty]
    public ICommand OpenLinkCommand { get; set; }
    [ActiveAutomationProperty]
    public ResizeMode ResizeMode { get; set; }

    public object Owner
    {
        get => _accessor.Execute(nameof(PinnableToolTipAutomationPeer.GetOwner));
    }
}
#nullable enable
