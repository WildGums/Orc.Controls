namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation.Controls;

    public class PinnableToolTip : FrameworkElement<PinnableToolTipModel, PinnableToolTipMap>
    {
        public PinnableToolTip(AutomationElement element)
            : base(element)
        {
        }
    }
}
