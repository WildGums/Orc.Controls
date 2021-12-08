namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.TabItem))]
    public class TabItem : FrameworkElement
    {
        public TabItem(AutomationElement element) 
            : base(element, ControlType.TabItem)
        {
        }

        public string Header => Element.Current.Name;

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
