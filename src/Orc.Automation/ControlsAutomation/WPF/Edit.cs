namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.Edit))]
    public class Edit : FrameworkElement
    {
        public Edit(AutomationElement element) 
            : base(element, ControlType.Edit)
        {
        }

        public string Text
        {
            get => Element.GetValue<string>();
            set => Element.SetValue(value);
        }
    }
}
