namespace Orc.Controls.Automation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using Orc.Automation;

    public class ColorLegendAutomationElementMap
    {
        [Target]
        public ColorLegendAutomationElement Target { get; set; }

        [ControlPart(ControlType = nameof(ControlType.Edit))]
        public AutomationElement SearchBoxPart { get; set; }

        [ControlPart(AutomationId = "SettingsButton")]
        public AutomationElement SettingsButtonPart { get; set; }

        [ControlPart(AutomationId = "AllVisibleCheckBox")]
        public AutomationElement AllVisibleCheckBoxPart { get; set; }

        [ControlPart(AutomationId = "UnselectAllButton")]
        public AutomationElement UnselectAllButtonPart { get; set; }

        [ControlPart(AutomationId = "List")]
        public AutomationElement ListPart { get; set; }

        public List<ColorLegendItemAutomationElement> Items => Target?.Element.FindAll(controlType: ControlType.ListItem)
            .Select(x => new ColorLegendItemAutomationElement(x))
            .ToList();
    }
}