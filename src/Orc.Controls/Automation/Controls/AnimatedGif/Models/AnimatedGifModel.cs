namespace Orc.Controls.Automation
{
    using Orc.Automation;

    [AutomationAccessType]
    public class AnimatedGifModel : ControlModel
    {
        public AnimatedGifModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public string GifSource { get; set; }
    }
}
