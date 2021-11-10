namespace Orc.Controls.Automation.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Windows.Automation;
    using Catel.IoC;

    public static class AutomationElementExtensions
    {
        public static TElement FindDescendantByAutomationId<TElement>(this AutomationElement parent, string automationId, int numberOfWaits = 50)
            where TElement : AutomationElementBase
        {
            var element = FindDescendantByAutomationId(parent, automationId, numberOfWaits);
            if (element is null)
            {
                return null;
            }

            return (TElement) parent.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(typeof(TElement), element);
        }

        public static TElement FindDescendantByName<TElement>(this AutomationElement parent, string automationId, int numberOfWaits = 50)
            where TElement : AutomationElementBase
        {
            var element = FindDescendantByAutomationId(parent, automationId, numberOfWaits);
            if (element is null)
            {
                return null;
            }

            return (TElement)parent.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(typeof(TElement), element);
        }

        public static TElement Find<TElement>(this AutomationElement parent, string id = null, string name = null, string className = null, int numberOfWaits = 50)
            where TElement : AutomationElementBase
        {
            var element = Find(parent, id, name, className, numberOfWaits);
            if (element is null)
            {
                return null;
            }

            return (TElement)parent.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(typeof(TElement), element);
        }

        public static AutomationElement Find(this AutomationElement parent, string id = null, string name = null, string className = null, int numberOfWaits = 50)
        {
            var conditions = new List<Condition>();
            if (!string.IsNullOrWhiteSpace(id))
            {
                conditions.Add(new PropertyCondition(AutomationElement.AutomationIdProperty, id));
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                conditions.Add(new PropertyCondition(AutomationElement.NameProperty, name));
            }

            if (!string.IsNullOrWhiteSpace(className))
            {
                conditions.Add(new PropertyCondition(AutomationElement.ClassNameProperty, className));
            }

            if (!conditions.Any())
            {
                return null;
            }

            var resultCondition = conditions.Count == 1 
                ? conditions[0] 
                : new AndCondition(conditions.ToArray());

            return FindFirstWithDelay(parent, TreeScope.Descendants, resultCondition, numberOfWaits);
        }

        public static AutomationElement FindDescendantByAutomationId(this AutomationElement parent, string automationId, int numberOfWaits = 50)
        {
            return FindFirstWithDelay(parent, TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, automationId), numberOfWaits);
        }

        public static AutomationElement FindDescendantByName(this AutomationElement parent, string name, int numberOfWaits = 50)
        {
            return FindFirstWithDelay(parent, TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, name), numberOfWaits);
        }

        public static AutomationElement FindFirstWithDelay(this AutomationElement parent, TreeScope scope, Condition condition, int numberOfWaits = 50)
        {
            var numWaits = 0;

            AutomationElement element;
            do
            {
                element = parent.FindAll(scope, condition)
                    .OfType<AutomationElement>()
                    .FirstOrDefault();

                ++numWaits;
                Thread.Sleep(200);
            } 
            while (element is null && numWaits < numberOfWaits);

            return element;
        }

        public static IList<AutomationElement> FindAllWithDelay(this AutomationElement parent, TreeScope scope, Condition condition, int numberOfWaits = 50)
        {
            var numWaits = 0;

            AutomationElementCollection elements;
            do
            {
                elements = parent.FindAll(scope, condition);

                ++numWaits;
                Thread.Sleep(200);
            } 
            while (elements is null && numWaits < numberOfWaits);

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
