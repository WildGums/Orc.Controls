namespace Orc.Controls.Automation
{
    using System.Collections.Generic;
    using System.Windows.Automation;
    using Orc.Automation;

    public class ColorLegendMap : AutomationBase
    {
        public ColorLegendMap(AutomationElement element) 
            : base(element)
        {
        }

        private AutomationElement SearchBoxPart => By.ControlType(ControlType.Edit).One();
        public AutomationElement SettingsButtonPart => By.Id("SettingsButton").One();
        public AutomationElement AllVisibleCheckBoxPart => By.Id("AllVisibleCheckBox").One();
        public AutomationElement UnselectAllButtonPart => By.Id("UnselectAllButton").One();
        public AutomationElement ListPart => By.Id("List").One();
        public List<ColorLegendItemAutomationControl> Items => By.ControlType(ControlType.ListItem).Many<ColorLegendItemAutomationControl>();
    }
}
