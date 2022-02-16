namespace Orc.Controls.Automation
{
    using Orc.Automation;

    [AutomationAccessType]
    public class FrameCounterModel : ControlModel
    {
        public FrameCounterModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public string Prefix { get; set; }
        public int ResetCount { get; set; }
    }
}
