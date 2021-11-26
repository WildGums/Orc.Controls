namespace Orc.Automation.Tests
{
    using System.Windows.Automation;

    public sealed class ButtonAutomationControl : ProtectedByControlTypeAutomationControl
    {
        public ButtonAutomationControl(AutomationElement element) 
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
