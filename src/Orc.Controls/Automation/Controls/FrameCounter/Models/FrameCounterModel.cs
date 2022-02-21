namespace Orc.Controls.Automation
{
    using Orc.Automation;

    [AutomationAccessType]
    public class FrameCounterModel : AutomationControlModel
    {
        public FrameCounterModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public string Prefix { get; set; }
        public int ResetCount { get; set; }
    }
}
