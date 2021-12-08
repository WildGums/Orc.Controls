namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.List))]
    public class List : FrameworkElement
    {
        public List(AutomationElement element) 
            : base(element, ControlType.List)
        {
        }

        public bool CanSelectMultiply => Element.CanSelectMultiple();

        public AutomationElement Select(int index)
        {
            if (Element.TrySelectItem(index, out var element))
            {
                return element;
            }

            return null;
        }
    }
}
