namespace Orc.Controls.Automation
{
    using System.Windows.Automation;
    using Catel;
    using Tests;

    public class TabControlAutomationElement : AutomationElementBase
    {
        public TabControlAutomationElement(AutomationElement tabControl)
            : base(tabControl)
        {
        }

        public AutomationElement SelectItem(string header)
        {
            if (Element.TrySelectItem(header, out var item))
            {
                return item;
            }

            return null;
        }

        public AutomationElement FindFirstDescendant(string automationId)
        {
            return _element.FindDescendantByAutomationId(automationId);
        }
    }
}
