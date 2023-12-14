namespace Orc.Controls.Automation;

using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(Controls.AnimatedGif))]
public class AnimatedGif : FrameworkElement<AnimatedGifModel>
{
    public AnimatedGif(AutomationElement element) 
        : base(element)
    {
    }
}
