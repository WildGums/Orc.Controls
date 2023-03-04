namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation.Controls;

public class FontImage : FrameworkElement<FontImageModel>
{
    public FontImage(AutomationElement element) 
        : base(element)
    {
    }
}
