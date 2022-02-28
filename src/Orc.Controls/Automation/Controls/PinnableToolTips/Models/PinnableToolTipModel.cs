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
    
    public bool AllowCloseByUser { get; set; }
    public double HorizontalOffset { get; set; }
    public bool IsPinned { get; set; }
    public Color GripColor { get; set; }
    public double VerticalOffset { get; set; }
    public ICommand OpenLinkCommand { get; set; }
    public ResizeMode ResizeMode { get; set; }
}
