namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;

    public class ExpanderView : AutomationBase
    {
        public ExpanderView(AutomationElement element) 
            : base(element)
        {
        }

        public bool IsExpanded
        {
            get => Element.GetIsExpanded();
            set => Element.SetIsExpanded(value);
        }
    }
}
