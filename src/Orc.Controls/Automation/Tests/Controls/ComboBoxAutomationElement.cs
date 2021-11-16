namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Tests;

    public class ComboBoxAutomationElement : AutomationElementBase
    {
        public ComboBoxAutomationElement(AutomationElement element) 
            : base(element)
        {

        }

        public bool TryExpand()
        {
            return Element.TryExpand();
        }

        public AutomationElement SelectItem(string header)
        {
            _element.SetFocus();

            var listItem = Element.FindFirstWithDelay(new PropertyCondition(AutomationElement.NameProperty, header));
            return listItem?.TrySelect() == true ? null : listItem;
        }
    }
}
