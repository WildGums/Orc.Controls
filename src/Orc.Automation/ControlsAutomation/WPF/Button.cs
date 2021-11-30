namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

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
