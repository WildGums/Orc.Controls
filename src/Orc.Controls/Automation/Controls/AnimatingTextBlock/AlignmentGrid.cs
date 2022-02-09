namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation.Controls;

    public class AnimatingTextBlock : FrameworkElement<AnimatingTextBlockModel>
    {
        public AnimatingTextBlock(AutomationElement element) 
            : base(element)
        {
        }
    }
}
