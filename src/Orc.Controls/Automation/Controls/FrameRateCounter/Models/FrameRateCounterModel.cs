namespace Orc.Controls.Automation
{
    using Orc.Automation;

    [AutomationAccessType]
    public class FrameRateCounterModel : FrameworkElementModel
    {
        public FrameRateCounterModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public string Prefix { get; set; }
    }
}
