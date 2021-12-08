namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.Button))]
    public sealed class Button : FrameworkElement
    {
        public Button(AutomationElement element) 
            : base(element, ControlType.Button)
        {

        }

        public string Content => Element.Current.Name;

        public bool TryInvoke()
        {
            return Element.TryInvoke();
        }
    }
}
