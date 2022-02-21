namespace Orc.Controls.Automation;

using System.Windows.Media;
using Orc.Automation;

[AutomationAccessType]
public class ColorBoardModel : FrameworkElementModel
{
    public ColorBoardModel(AutomationElementAccessor accessor)
        : base(accessor)
    {
    }

    public Color Color { get; set; }
}
