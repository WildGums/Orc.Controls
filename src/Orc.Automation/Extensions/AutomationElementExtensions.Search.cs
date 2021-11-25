namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Windows.Automation;
    using Catel;
    using Catel.IoC;

    public static partial class AutomationElementExtensions
    {
        public static TElement FindAncestor<TElement>(this AutomationElement child, Func<AutomationElement, bool> searchFunc)
            where TElement : AutomationControl
        {
            Argument.IsNotNull(() => child);
            Argument.IsNotNull(() => searchFunc);

            var element = FindAncestor(child, searchFunc);
            if (element is null)
            {
                return null;
            }

            return (TElement)child.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(typeof(TElement), element);
        }

        public static AutomationElement FindAncestor(this AutomationElement child, Func<AutomationElement, bool> searchFunc)
        {
            Argument.IsNotNull(() => child);
            Argument.IsNotNull(() => searchFunc);

            var parent = child.GetParent();
            while (!searchFunc(parent) && parent is not null)
            {
                parent = parent.GetParent();
            }

            return parent;
        }

        public static TElement Find<TElement>(this AutomationElement parent, string id = null, string name = null, string className = null, ControlType controlType = null, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
            where TElement : AutomationControl
        {
            var element = Find(parent, id, name, className, controlType, scope, numberOfWaits);
            if (element is null)
            {
                return null;
            }

            return (TElement)parent.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(typeof(TElement), element);
        }

        public static AutomationElement Find(this AutomationElement parent, string id = null, string name = null, string className = null, ControlType controlType = null, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
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

            if (controlType is not null)
            {
                conditions.Add(new PropertyCondition(AutomationElement.ControlTypeProperty, controlType));
            }

            if (!conditions.Any())
            {
                return null;
            }

            var resultCondition = conditions.Count == 1
                ? conditions[0]
                : new AndCondition(conditions.ToArray());

            return Find(parent, resultCondition, scope, numberOfWaits);
        }

        public static AutomationElement Find(this AutomationElement parent, Condition condition, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
        {
            var numWaits = 0;

            AutomationElement element;
            do
            {
                element = parent.FindFirst(scope, condition);

                ++numWaits;
                Thread.Sleep(200);
            } while (element is null && numWaits < numberOfWaits);

            return element;
        }

        public static List<AutomationElement> FindAll(this AutomationElement parent, string id = null, string name = null, string className = null, ControlType controlType = null, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
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

            if (controlType is not null)
            {
                conditions.Add(new PropertyCondition(AutomationElement.ControlTypeProperty, controlType));
            }

            if (!conditions.Any())
            {
                return null;
            }

            var resultCondition = conditions.Count == 1
                ? conditions[0]
                : new AndCondition(conditions.ToArray());

            return FindAll(parent, resultCondition, scope, numberOfWaits);
        }

        public static List<AutomationElement> FindAll(this AutomationElement parent, Condition condition, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
        {
            var numWaits = 0;

            AutomationElementCollection elements;
            do
            {
                elements = parent.FindAll(scope, condition);

                ++numWaits;
                Thread.Sleep(200);
            } while (elements is null && numWaits < numberOfWaits);

            return elements?.OfType<AutomationElement>().ToList();
        }

        public static bool TryGetValue(this AutomationElement element, out string value)
        {
            string innerValue = null;
            value = null;
            if (element.TryRunPatternFunc<ValuePattern>(x => innerValue = x.Current.Value))
            {
                value = innerValue;

                return true;
            }

            return false;
        }
    }
}
