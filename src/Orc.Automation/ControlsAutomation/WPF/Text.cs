namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.Text))]
    public class Text : FrameworkElement
    {
        public Text(AutomationElement element) 
            : base(element, ControlType.Text)
        {
        }

        public string Value => Element.Current.Name;
    }
}
