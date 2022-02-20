namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class LogMessageCategoryToggleButtonMap : AutomationBase
    {
        public LogMessageCategoryToggleButtonMap(AutomationElement element) 
            : base(element)
        {
            
        }

        public ToggleButton Toggle => By.Id("PART_Toggle").One<ToggleButton>();
        public Text EntryCountText => By.Id("PART_EntryCountTextBlock").Raw().One<Text>();
        public Text CategoryText => By.Id("PART_CategoryTextBlock").Raw().One<Text>();
    }
}
