namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation.Controls;

public class FontImageControl : FrameworkElement<FontImageControlModel>
{
    public FontImageControl(AutomationElement element) 
        : base(element)
    {
    }
}
