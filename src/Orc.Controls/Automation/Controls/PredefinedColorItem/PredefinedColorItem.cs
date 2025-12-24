#nullable disable
namespace Orc.Controls.Automation;

using System.Windows.Automation;
using System.Windows.Media;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(Orc.Controls.PredefinedColorItem))]
public class PredefinedColorItem : AutomationControl
{
    public PredefinedColorItem(AutomationElement element) 
        : base(element)
    {
    }

    public Color Color
    {
        get => Access.GetValue<Color>();
    }

    public string Text
    {
        get => Access.GetValue<string>();
    }

    public bool IsSelected
    {
        get => Element.GetParent().As<ListItem>().IsSelected;
        set => Element.GetParent().As<ListItem>().IsSelected = value;
    }
}
#nullable enable
