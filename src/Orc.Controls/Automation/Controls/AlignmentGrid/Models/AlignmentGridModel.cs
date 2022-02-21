namespace Orc.Controls.Automation;

using System.Windows.Media;
using Orc.Automation;

[AutomationAccessType]
public class AlignmentGridModel : FrameworkElementModel
{
    public AlignmentGridModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public Brush LineBrush { get; set; }
    public double HorizontalStep { get; set; }
    public double VerticalStep { get; set; }
}
