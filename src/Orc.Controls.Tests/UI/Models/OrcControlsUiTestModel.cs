namespace Orc.Controls.Tests
{
    using Orc.Automation;

    public class OrcControlsUiTestModel<TAutomationElement> : UiTestModel
        where TAutomationElement : AutomationElementBase
    {
        public TAutomationElement TargetControl { get; set; }
    }
}
