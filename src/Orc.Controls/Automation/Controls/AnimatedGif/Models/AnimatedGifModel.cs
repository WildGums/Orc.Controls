namespace Orc.Controls.Automation
{
    using Orc.Automation;

    [ActiveAutomationModel]
    public class AnimatedGifModel : FrameworkElementModel
    {
        public AnimatedGifModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public string? GifSource { get; set; }
    }
}
