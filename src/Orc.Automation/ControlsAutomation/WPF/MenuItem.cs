namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    public class MenuItem : FrameworkElement
    {
        public MenuItem(AutomationElement element) 
            : base(element, ControlType.MenuItem)
        {
        }

        public void Select()
        {
            Element.TryClick();
        }
    }
}
