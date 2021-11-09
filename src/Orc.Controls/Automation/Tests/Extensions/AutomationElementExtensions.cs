namespace Orc.Controls.Automation.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Windows.Automation;

    public static class AutomationElementExtensions
    {
        public static AutomationElement FindDescendantWithAutomationId(this AutomationElement parent, string automationId, int numberOfWaits = 50)
        {
            return FindFirstWithDelay(parent, TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, automationId));
        }

        public static AutomationElement FindFirstWithDelay(this AutomationElement parent, TreeScope scope, Condition condition, int numberOfWaits = 50)
        {
            var numWaits = numberOfWaits;

            AutomationElement element;
            do
            {
                element = parent.FindAll(scope, condition)
                    .OfType<AutomationElement>()
                    .FirstOrDefault();

                ++numWaits;
                Thread.Sleep(200);
            } while (element is null && numWaits < 50);

            return element;
        }

        public static IList<AutomationElement> FindAllWithDelay(this AutomationElement parent, TreeScope scope, Condition condition, int numberOfWaits = 50)
        {
            var numWaits = numberOfWaits;

            AutomationElementCollection elements;
            do
            {
                elements = parent.FindAll(scope, condition);

                ++numWaits;
                Thread.Sleep(200);
            } while (elements is null && numWaits < 50);

            return elements?.OfType<AutomationElement>().ToList();
        }

        public static void SetValue(this AutomationElement element, string value)
        {
            var vpTextBox1 = (ValuePattern)element.GetCurrentPattern(ValuePattern.Pattern);
            vpTextBox1.SetValue(value);
        }

        public static void Toggle(this AutomationElement element)
        {
            var tpToggle = (TogglePattern)element.GetCurrentPattern(TogglePattern.Pattern);
            tpToggle.Toggle();
        }

        public static void Invoke(this AutomationElement element)
        {
            var tpToggle = (InvokePattern)element.GetCurrentPattern(InvokePattern.Pattern);
            tpToggle.Invoke();
        }
    }
}
