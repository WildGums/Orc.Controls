namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(Orc.Controls.AnimatingTextBlock))]
public class AnimatingTextBlock : FrameworkElement<AnimatingTextBlockModel>
{
    public AnimatingTextBlock(AutomationElement element) 
        : base(element)
    {
    }
}
