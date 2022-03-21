namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Orc.Automation;
    using Orc.Automation.Controls;

    public class Expander : FrameworkElement<ExpanderModel>
    {
        public Expander(AutomationElement element) 
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
