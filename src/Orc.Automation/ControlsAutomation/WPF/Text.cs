namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    public class Text : FrameworkElement
    {
        public Text(AutomationElement element) 
            : base(element, ControlType.Text)
        {
        }

        public string Value => Element.Current.Name;
    }
}
