namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    public class ListItem : FrameworkElement
    {
        public ListItem(AutomationElement element)
            : base(element, ControlType.ListItem)
        {
        }

        public bool IsSelected
        {
            get => Element.GetIsSelected();
            set => Element.TrySetSelection(value);
        }

        public bool TrySelect()
        {
            return Element.TrySelect();
        }
    }
}
