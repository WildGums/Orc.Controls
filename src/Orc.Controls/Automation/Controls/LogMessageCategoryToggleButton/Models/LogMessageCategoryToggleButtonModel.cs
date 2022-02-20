namespace Orc.Controls.Automation
{
    using Orc.Automation;

    [AutomationAccessType]
    public class LogMessageCategoryToggleButtonModel : ControlModel
    {
        public LogMessageCategoryToggleButtonModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public bool IsChecked { get; set; }
        public int EntryCount { get; set; }
        public string Category { get; set; }
    }
}
