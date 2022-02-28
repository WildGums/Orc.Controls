namespace Orc.Controls.Automation
{
    using System.Windows.Controls;
    using Orc.Automation;

    [AutomationAccessType]
    public class StepBarItemModel : FrameworkElementModel
    {
        public StepBarItemModel(AutomationElementAccessor accessor)
            : base(accessor)
        {
        }

        public Orientation Orientation { get; set; }
    }
}
