namespace Orc.Controls.Automation;

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Orc.Automation;

[AutomationAccessType]
public class PinnableToolTipModel : FrameworkElementModel
{
    public PinnableToolTipModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }
    
    [ApiProperty]
    public bool AllowCloseByUser { get; set; }
    [ApiProperty]
    public double HorizontalOffset { get; set; }
    [ApiProperty]
    public bool IsPinned { get; set; }
    [ApiProperty]
    public Color GripColor { get; set; }
    [ApiProperty]
    public double VerticalOffset { get; set; }
    [ApiProperty]
    public ICommand OpenLinkCommand { get; set; }
    [ApiProperty]
    public ResizeMode ResizeMode { get; set; }

    public object Owner
    {
        get => _accessor.Execute(nameof(PinnableToolTipAutomationPeer.GetOwner));
    }
}
