namespace Orc.Automation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Automation;
    using Catel;
    using Catel.IoC;
    using Catel.Reflection;

    public static partial class AutomationElementExtensions
    {
        public static TTemplate CreateControlMap<TTemplate>(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            return (TTemplate)element.CreateControlMap(typeof(TTemplate));
        }

        public static object CreateControlMap(this AutomationElement element, Type controlMapType)
        {
            Argument.IsNotNull(() => element);

            var template = element.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(controlMapType);

            InitializeControlMap(element, template);

            return template;
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
            if (targetElementMapProperty is null)
            {
                return;
            }

            var elementMap = element.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(targetElementMapProperty.PropertyType);
            if (elementMap is null)
            {
                return;
            }

            targetElementMapProperty.SetValue(host, elementMap);

            element.InitializeControlMap(elementMap);
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
