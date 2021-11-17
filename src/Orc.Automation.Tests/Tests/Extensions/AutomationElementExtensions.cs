namespace Orc.Automation.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Windows.Automation;
    using System.Windows.Input;
    using Catel;
    using Catel.IoC;

    public static class AutomationElementExtensions
    {
        public static void MouseClick(this AutomationElement element, MouseButton mouseButton = MouseButton.Left)
        {
            Argument.IsNotNull(() => element);

            var rect = element.Current.BoundingRectangle;

            MouseInput.MoveTo(rect.GetClickablePoint().ToDrawingPoint());
            MouseInput.Click(mouseButton);
        }

        public static bool IsVisible(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return !IsOffscreen(element);
        }

        public static bool IsOffscreen(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return (bool)element.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty);
        }

        public static TElement FindDescendantByAutomationId<TElement>(this AutomationElement parent, string automationId, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
            where TElement : AutomationElementBase
        {
            var element = FindDescendantByAutomationId(parent, automationId, scope, numberOfWaits);
            if (element is null)
            {
                return null;
            }

            return (TElement) parent.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(typeof(TElement), element);
        }

        public static TElement FindDescendantByName<TElement>(this AutomationElement parent, string automationId, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
            where TElement : AutomationElementBase
        {
            var element = FindDescendantByAutomationId(parent, automationId, scope, numberOfWaits);
            if (element is null)
            {
                return null;
            }

            return (TElement)parent.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(typeof(TElement), element);
        }

        public static TElement Find<TElement>(this AutomationElement parent, string id = null, string name = null, string className = null, ControlType controlType = null, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
            where TElement : AutomationElementBase
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

            return FindFirstWithDelay(parent, resultCondition, scope, numberOfWaits);
        }

        public static AutomationElement FindDescendantByAutomationId(this AutomationElement parent, string automationId, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
        {
            return FindFirstWithDelay(parent, new PropertyCondition(AutomationElement.AutomationIdProperty, automationId), scope, numberOfWaits);
        }

        public static AutomationElement FindDescendantByName(this AutomationElement parent, string name, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
        {
            return FindFirstWithDelay(parent, new PropertyCondition(AutomationElement.NameProperty, name), scope, numberOfWaits);
        }

        public static AutomationElement FindFirstWithDelay(this AutomationElement parent, Condition condition, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
        {
            var numWaits = 0;

            AutomationElement element;
            do
            {
                element = parent.FindFirst(scope, condition);

                ++numWaits;
                Thread.Sleep(200);
            } 
            while (element is null && numWaits < numberOfWaits);

            return element;
        }

        public static IList<AutomationElement> FindAllWithDelay(this AutomationElement parent, TreeScope scope, Condition condition, int numberOfWaits = 10)
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

        public static bool TrySetValue(this AutomationElement element, string value)
        {
            return element.TryRunPatternFunc<ValuePattern>(x => x.SetValue(value));
        }
        
        public static bool TryInvoke(this AutomationElement element)
        {
            return element.TryRunPatternFunc<InvokePattern>(x => x.Invoke());
        }

        public static bool TryExpand(this AutomationElement element)
        {
            return element.TryRunPatternFunc<ExpandCollapsePattern>(x => x.Expand());
        }

        public static bool TryCloseWindow(this AutomationElement element)
        {
            return element.TryRunPatternFunc<WindowPattern>(x => x.Close());
        }

        public static bool TryGetPropertyValue<TPropertyValueType>(this AutomationElement element, string propertyName, out TPropertyValueType value)
        {
            value = default;

            if (TryGetPropertyValue(element, propertyName, out var propertyValue))
            {
                value = (TPropertyValueType)propertyValue;
                return true;
            }

            return false;
        }

        public static bool TryGetPropertyValue(this AutomationElement element, string propertyName, out object value)
        {
            value = null;

            try
            {
                var commandAutomationElement = new CommandAutomationElement(element);
                value = commandAutomationElement.GetValue(propertyName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }

            return true;
        }

        public static bool TrySetPropertyValue(this AutomationElement element, string propertyName, object value)
        {
            try
            {
                var commandAutomationElement = new CommandAutomationElement(element);
                commandAutomationElement.SetValue(propertyName, value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }

            return true;
        }

        public static bool TryExecute<TResult>(this AutomationElement element, string methodName, object parameter, out TResult result)
        {
            result = default;

            if (!TryExecute(element, methodName, parameter, out var objResult))
            {
                return false;
            }

            result = (TResult) objResult;

            return true;
        }

        public static bool TryExecute<TResult>(this AutomationElement element, string methodName, out TResult result)
        {
            result = default;

            if (!TryExecute(element, methodName, out var objResult))
            {
                return false;
            }

            result = (TResult)objResult;

            return true;
        }

        public static bool TryExecute(this AutomationElement element, string methodName, object parameter, out object result)
        {
            result = null;

            try
            {
                var commandAutomationElement = new CommandAutomationElement(element);
                result = commandAutomationElement.Execute(methodName, parameter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }

            return true;
        }

        public static bool TryExecute(this AutomationElement element, string methodName, out object result)
        {
            result = null;

            try
            {
                var testHostCommand = new CommandAutomationElement(element);
                result = testHostCommand.Execute(methodName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }

            return true;
        }

        public static bool TrySelectItem(this AutomationElement containerElement, string name, out AutomationElement selectItem)
        {
            Argument.IsNotNull(() => containerElement);

            //containerElement.SetFocus();

            selectItem = containerElement.FindFirstWithDelay(new PropertyCondition(AutomationElement.NameProperty, name));
            return selectItem?.TrySelect() == true;
        }

        public static bool TrySelect(this AutomationElement element)
        {
            return element.TryRunPatternFunc<SelectionItemPattern>(x => x.Select());
        }
        
        public static bool TryToggle(this AutomationElement element)
        {
            return element.TryRunPatternFunc<TogglePattern>(x => x.Toggle());
        }
    }
}
