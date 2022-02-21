namespace Orc.Controls.Automation;

using System.Windows.Media;
using Orc.Automation;

[AutomationAccessType]
public class ColorPickerModel : FrameworkElementModel
{
    public ColorPickerModel(AutomationElementAccessor accessor)
        : base(accessor)
    {
    }

    //Model
    public Color Color { get; set; }
    public Color CurrentColor { get; set; }
    public bool IsDropDownOpen { get; set; }
}
