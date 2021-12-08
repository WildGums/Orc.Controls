namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.CheckBox))]
    public class CheckBox : FrameworkElement
    {
        public CheckBox(AutomationElement element) 
            : base(element, ControlType.CheckBox)
        {
        }

        public bool? IsChecked
        {
            get => Element.GetToggleState();
            set => Element.TrySetToggleState(value);
        }
    }
}
