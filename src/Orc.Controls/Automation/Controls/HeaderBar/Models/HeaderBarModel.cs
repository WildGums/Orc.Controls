namespace Orc.Controls.Automation
{
    using Orc.Automation;

    [AutomationAccessType]
    public class HeaderBarModel : ControlModel
    {
        public HeaderBarModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public string Header { get; set; }
    }
}
