#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Media;
using Orc.Automation;

[ActiveAutomationModel]
public class FontImageControlModel : FrameworkElementModel
{
    public FontImageControlModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public string ItemName { get; set; }
    public SolidColorBrush Foreground { get; set; }
    public string FontFamily { get; set; }
}
#nullable enable
