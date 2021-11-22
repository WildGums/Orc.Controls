namespace Orc.Automation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Automation;
    using System.Windows.Input;
    using Catel;
    using Catel.IoC;
    using Catel.Reflection;

    public static class AutomationElementExtensions
    {
        /// <summary>
        /// Try to Invoke, Toggle, Select...if this patterns not implemented, depends on useMouse parameter use Mouse Input
        /// </summary>
        /// <param name="element"></param>
        /// <param name="useMouse"></param>
        /// <returns></returns>
        public static bool TryClick(this AutomationElement element, bool useMouse = true)
        {
            Argument.IsNotNull(() => element);

            try
            {
                if (!element.TryInvoke() && !element.TryToggle() && !element.TrySelect() && useMouse)
                {
                    MouseClick(element);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

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

            return (TElement)parent.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(typeof(TElement), element);
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

            return FindAllWithDelay(parent, resultCondition, scope, numberOfWaits);
        }

        public static List<AutomationElement> FindAllWithDelay(this AutomationElement parent, Condition condition, TreeScope scope = TreeScope.Subtree, int numberOfWaits = 10)
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
                var commandAutomationElement = new RunMethodAutomationElement(element);
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
                var commandAutomationElement = new RunMethodAutomationElement(element);
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

            result = (TResult)objResult;

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
                var commandAutomationElement = new RunMethodAutomationElement(element);
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
                var testHostCommand = new RunMethodAutomationElement(element);
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

        public static bool TrySelectItem(this AutomationElement containerElement, int index, out AutomationElement selectItem)
        {
            Argument.IsNotNull(() => containerElement);

            selectItem = TryGetChildElement(containerElement, index);
            return selectItem?.TrySelect() == true;
        }

        public static AutomationElement TryGetChildElement(this AutomationElement containerElement, int index)
        {
            var item = TreeWalker.ControlViewWalker.GetFirstChild(containerElement);
            var currentIndex = 0;

            while (!Equals(index, currentIndex) && item is not null)
            {
                item = TreeWalker.ControlViewWalker.GetNextSibling(item);
                currentIndex++;
            }

            return item;
        }

        public static IEnumerable<AutomationElement> TryGetChildElements(this AutomationElement containerElement)
        {
            var item = TreeWalker.ControlViewWalker.GetFirstChild(containerElement);

            while (item is not null)
            {
                item = TreeWalker.ControlViewWalker.GetNextSibling(item);

                yield return item;
            }
        }

        public static bool TrySelect(this AutomationElement element)
        {
            return element.TryRunPatternFunc<SelectionItemPattern>(x => x.Select());
        }

        public static bool TryToggle(this AutomationElement element)
        {
            return element.TryRunPatternFunc<TogglePattern>(x => x.Toggle());
        }

        public static bool TryToggle(this AutomationElement element, out bool? toggleState)
        {
            toggleState = null;

            return TryToggle(element) && TryGetToggleState(element, out toggleState);
        }

        public static bool TryGetToggleState(this AutomationElement element, out bool? toggleState)
        {
            var state = ToggleState.Off;

            var result = element.TryRunPatternFunc<TogglePattern>(x => state = x.Current.ToggleState);

            toggleState = state switch
            {
                ToggleState.Off => false,
                ToggleState.On => true,
                ToggleState.Indeterminate => null,
                _ => throw new ArgumentOutOfRangeException()
            };

            return result;
        }

        public static object CreateControlMap(this AutomationElement element, Type controlMapType)
        {
            Argument.IsNotNull(() => element);

            var template = element.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(controlMapType);

            InitializeControlMap(element, template);

            return template;
        }

        public static TTemplate CreateControlMap<TTemplate>(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return (TTemplate)element.CreateControlMap(typeof(TTemplate));
        }

        public static void InitializeControlMap(this AutomationElement element, object controlMap)
        {
            Argument.IsNotNull(() => element);
            Argument.IsNotNull(() => controlMap);

            ResolveTargetProperty(element, controlMap);
            ResolvePartProperties(element, controlMap);
            ResolveTargetControlMapProperty(element, controlMap);
        }

        private static void ResolveTargetControlMapProperty(this AutomationElement element, object host)
        {
            var hostType = host.GetType();
            var targetElementMapProperty = hostType.GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(TargetControlMapAttribute)));
            if (targetElementMapProperty is not null)
            {
                var elementMap = element.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(targetElementMapProperty.PropertyType);
                if (elementMap is null)
                {
                    return;
                }

                targetElementMapProperty.SetValue(host, elementMap);

                element.InitializeControlMap(elementMap);
            }
        }

        private static void ResolveTargetProperty(AutomationElement targetElement, object template)
        {
            var targetControlProperty = template.GetType().GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(TargetAttribute)));
            if (targetControlProperty is null)
            {
                return;
            }

            InitializePropertyWithValue(template, targetControlProperty, targetElement);
        }

        private static void ResolvePartProperties(AutomationElement targetElement, object controlMap)
        {
            var controlPartProperties = controlMap.GetType()
                .GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(ControlPartAttribute)))
                .Select(x => new { PropertyInfo = x, Attribute = x.GetAttribute<ControlPartAttribute>() })
                .ToList();

            foreach (var controlPartProperty in controlPartProperties)
            {
                var property = controlPartProperty.PropertyInfo;
                var attribute = controlPartProperty.Attribute;

                ControlType controlType = null;
                if (!string.IsNullOrWhiteSpace(attribute.ControlType))
                {
                    controlType = typeof(ControlType).GetField(attribute.ControlType)?.GetValue(null) as ControlType;
                }


                object propertyValue = null;
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    propertyValue = targetElement.FindAll(id: attribute.AutomationId, name: attribute.Name, className: attribute.ClassName, controlType: controlType);
                }
                else
                {
                    propertyValue = targetElement.Find(id: attribute.AutomationId, name: attribute.Name, className: attribute.ClassName, controlType: controlType);
                }

                InitializePropertyWithValue(controlMap, property, propertyValue);
            }
        }

        private static void InitializePropertyWithValue(object controlMap, PropertyInfo property, object value)
        {
            var typeFactory = value.GetTypeFactory();

            var propertyType = property.PropertyType;
            if (typeof(AutomationElementBase).IsAssignableFrom(propertyType))
            {
                property.SetValue(controlMap, typeFactory.CreateInstanceWithParametersAndAutoCompletion(propertyType, value));

                return;
            }

            if (propertyType == typeof(AutomationElement))
            {
                property.SetValue(controlMap, value);

                return;
            }

            var collectionElementType = propertyType.GetAnyElementType();
            if (collectionElementType is null)
            {
                return;
            }

            if (value is not IEnumerable<AutomationElement> valueElements)
            {
                return;
            }

            if (typeFactory.CreateInstanceWithParametersAndAutoCompletion(propertyType) is not IList elementCollection)
            {
                return;
            }

            if (typeof(AutomationElementBase).IsAssignableFrom(collectionElementType))
            {
                foreach (var automationElement in valueElements)
                {
                    elementCollection.Add(typeFactory.CreateInstanceWithParametersAndAutoCompletion(collectionElementType, automationElement));
                }
            }

            if (propertyType == typeof(AutomationElement))
            {
                foreach (var automationElement in valueElements)
                {
                    elementCollection.Add(automationElement);
                }
            }

            property.SetValue(controlMap, elementCollection);
        }
    }
}
