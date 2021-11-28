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

        //Vladimir:TODO
        public AutomationElement CheckBox => By.ControlType(ControlType.CheckBox).One();
        public AutomationElement Button => By.ControlType(ControlType.Button).One();
        public AutomationElement AdditionalDataTextBlock => By.Id("AdditionalDataTextBlock").One();
        public AutomationElement DescriptionTextBlock => By.Id( "DescriptionTextBlock").One();

        public bool TrySelect()
        {
            return Element.TrySelect();
        }
    }
}
