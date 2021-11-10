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
            _element.SetFocus();

            var tabItem = _element.FindFirstWithDelay(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, header));
            if (tabItem?.GetCurrentPattern(SelectionItemPattern.Pattern) is not SelectionItemPattern selectionPattern)
            {
                return null;
            }

            selectionPattern.Select();
            return tabItem;
        }

        public AutomationElement FindFirstDescendant(string automationId)
        {
            return _element.FindDescendantByAutomationId(automationId);
        }
    }
}
