namespace Orc.Controls.Automation
{
    using Orc.Automation;

    [ActiveAutomationModel]
    public class HeaderBarModel : FrameworkElementModel
    {
        public HeaderBarModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public string Header { get; set; }
    }
}
