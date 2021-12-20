namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.ListItem))]
    public class ListItem : FrameworkElement
    {
        public ListItem(AutomationElement element)
            : base(element, ControlType.ListItem)
        {
        }

        public string DisplayText => Element.TryGetDisplayText();

        public bool IsSelected
        {
            get => Element.GetIsSelected();
            set => Element.TrySetSelection(value);
        }

        public void Select()
        {
            Element.Select();
        }
    }
}
