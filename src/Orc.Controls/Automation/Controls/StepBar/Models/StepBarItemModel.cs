namespace Orc.Controls.Automation
{
    using System.Windows.Controls;
    using Orc.Automation;

    [ActiveAutomationModel]
    public class StepBarItemModel : FrameworkElementModel
    {
        public StepBarItemModel(AutomationElementAccessor accessor)
            : base(accessor)
        {
        }

        public Orientation Orientation { get; set; }
    }
}
