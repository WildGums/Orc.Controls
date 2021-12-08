namespace Orc.Automation.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.Menu))]
    public class Menu : FrameworkElement
    {
        public Menu(AutomationElement element)
            : base(element, ControlType.Menu)
        {
        }

        public IList<MenuItem> Items => Element.GetChildElements()
            .ToList()
            .Select(x => x.As<MenuItem>())
            .ToList();
    }
}
