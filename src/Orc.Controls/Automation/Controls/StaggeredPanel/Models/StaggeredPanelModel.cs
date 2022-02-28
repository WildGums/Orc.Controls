namespace Orc.Controls.Automation;

using System.Windows;
using Orc.Automation;

[AutomationAccessType]
public class StaggeredPanelModel : FrameworkElementModel
{
    public StaggeredPanelModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public double DesiredColumnWidth { get; set; }
    public Thickness Padding { get; set; }
    public double ColumnSpacing { get; set; }
    public double RowSpacing { get; set; }
}
