namespace Orc.Controls.Automation
{
    using System;
    using System.Windows.Media;
    using Orc.Automation;

    [ActiveAutomationModel]
    public class LinkLabelModel : FrameworkElementModel
    {
        public LinkLabelModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public object Content { get; set; }

        public Uri Url { get; set; }
        public bool HasUrl { get; }
        public SolidColorBrush HoverForeground { get; set; }
        public LinkLabelBehavior LinkLabelBehavior { get; set; }
        public LinkLabelClickBehavior ClickBehavior { get; set; }
        public object CommandParameter { get; set; }
    }
}
