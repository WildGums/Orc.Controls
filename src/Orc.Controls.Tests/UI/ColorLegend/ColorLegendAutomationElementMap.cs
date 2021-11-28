namespace Orc.Controls.Automation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using Orc.Automation;

    public class ColorLegendAutomationElementMap : AutomationBase
    {
        public ColorLegendAutomationElementMap(AutomationElement element) 
            : base(element)
        {
        }

        [ControlPart(ControlType = nameof(ControlType.Edit))]
        public AutomationElement SearchBoxPart => By.ControlType(ControlType.Edit).One();

        [ControlPart(AutomationId = "SettingsButton")]
        public AutomationElement SettingsButtonPart => By.Id("SettingsButton").One();

        [ControlPart(AutomationId = "AllVisibleCheckBox")]
        public AutomationElement AllVisibleCheckBoxPart => By.Id("AllVisibleCheckBox").One();

        [ControlPart(AutomationId = "UnselectAllButton")]
        public AutomationElement UnselectAllButtonPart => By.Id("UnselectAllButton").One();

        [ControlPart(AutomationId = "List")]
        public AutomationElement ListPart => By.Id("List").One();

        public List<ColorLegendItemAutomationElement> Items => By.ControlType(ControlType.ListItem).Many<ColorLegendItemAutomationElement>();
    }
}
