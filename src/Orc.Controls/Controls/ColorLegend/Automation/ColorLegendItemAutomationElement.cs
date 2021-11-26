namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;

    public class ColorLegendItemAutomationElement : AutomationControl
    {
        public ColorLegendItemAutomationElement(AutomationElement element)
            : base(element)
        {
        }

        //public AutomationElement CheckBox => GetPart(controlType: ControlType.CheckBox);
        //public AutomationElement Button => GetPart(controlType: ControlType.Button);
        //public AutomationElement AdditionalDataTextBlock => GetPart(id: "AdditionalDataTextBlock");
        //public AutomationElement DescriptionTextBlock => GetPart(id: "DescriptionTextBlock");

        public bool TrySelect()
        {
            return Element.TrySelect();
        }
    }
}
