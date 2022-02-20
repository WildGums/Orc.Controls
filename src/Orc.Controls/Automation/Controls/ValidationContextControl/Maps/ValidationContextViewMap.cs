namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class ValidationContextViewMap : AutomationBase
    {
        public ValidationContextViewMap(AutomationElement element) 
            : base(element)
        {
        }

        public LogMessageCategoryToggleButton ShowErrorsButton => By.Id("ShowErrorsButtonId").One<LogMessageCategoryToggleButton>();
        public LogMessageCategoryToggleButton ShowWarningButton => By.Id("ShowWarningButtonId").One<LogMessageCategoryToggleButton>();

        public Button ExpandAllButton => By.Id("ExpandAllButtonId").One<Button>();
        public Button CollapseAllButton => By.Id("CollapseAllButtonId").One<Button>();
        public FilterBox FilterBox => By.Id("FilterBoxId").One<FilterBox>();

        public Button CopyButton => By.Id("CopyButtonId").One<Button>();
        public Button OpenButton => By.Id("OpenButtonId").One<Button>();
        public ValidationContextTree Tree => By.One<ValidationContextTree>();
    }
}
