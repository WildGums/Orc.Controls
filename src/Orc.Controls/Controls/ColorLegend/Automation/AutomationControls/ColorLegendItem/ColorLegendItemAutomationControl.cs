namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;

    public class ColorLegendItemAutomationControl : AutomationControl, IColorLegendItemAutomationActions
    {
        public ColorLegendItemAutomationControl(AutomationElement element)
            : base(element)
        {
        }

        private AutomationElement CheckBox => By.ControlType(ControlType.CheckBox).One();
        public AutomationElement Button => By.ControlType(ControlType.Button).One();
        public AutomationElement AdditionalDataTextBlock => By.Id("AdditionalDataTextBlock").One();
        public AutomationElement DescriptionTextBlock => By.Id("DescriptionTextBlock").One();

        public IColorLegendItemAutomationActions Simulate => this;

        public bool? IsChecked
        {
            get => CheckBox.GetToggleState();
        }

        public bool TrySelect()
        {
            return Element.TrySelect();
        }

        bool IColorLegendItemAutomationActions.TrySetToggleState(bool desiredState)
        {
            return CheckBox.TrySetToggleState(desiredState);
        }
    }
}
