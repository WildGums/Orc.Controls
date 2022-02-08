namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation.Controls;

    public class AnimatedGif : FrameworkElement<AnimatedGifModel>
    {
        public AnimatedGif(AutomationElement element) 
            : base(element)
        {
        }
    }
}
