namespace Orc.Controls.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using Tests;

    public class ComboBoxAutomationElement : AutomationElementBase
    {
        private readonly ValuePattern _valuePattern;
        private readonly ExpandCollapsePattern _expandCollapsePattern;
        private readonly SelectionPattern _selectionItemPattern;
        private readonly ScrollPattern _scrollPattern;

        public ComboBoxAutomationElement(AutomationElement element) 
            : base(element)
        {
            _valuePattern = element.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
            _expandCollapsePattern = element.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
            _scrollPattern = element.GetCurrentPattern(ScrollPattern.Pattern) as ScrollPattern;
            _selectionItemPattern = element.GetCurrentPattern(SelectionPattern.Pattern) as SelectionPattern;
        }

        public void Expand()
        {
            _expandCollapsePattern.Expand();
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
    }
}
