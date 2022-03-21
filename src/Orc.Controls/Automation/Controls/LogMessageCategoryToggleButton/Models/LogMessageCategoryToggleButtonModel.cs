namespace Orc.Controls.Automation
{
    using Orc.Automation;

    [ActiveAutomationModel]
    public class LogMessageCategoryToggleButtonModel : FrameworkElementModel
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
