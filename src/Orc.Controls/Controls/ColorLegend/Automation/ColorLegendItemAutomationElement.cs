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
        public AutomationElement CheckBox => By.ControlType(ControlType.CheckBox).Get();
        public AutomationElement Button => By.ControlType(ControlType.Button).Get();
        public AutomationElement AdditionalDataTextBlock => By.Id("AdditionalDataTextBlock").Get();
        public AutomationElement DescriptionTextBlock => By.Id( "DescriptionTextBlock").Get();

        public bool TrySelect()
        {
            return Element.TrySelect();
        }
    }
}
