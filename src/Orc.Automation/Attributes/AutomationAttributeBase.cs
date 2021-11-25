namespace Orc.Automation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Automation;
    using Catel.IoC;

    public class AutomationAttributeBase : Attribute
    {
        public static void InitializePropertyWithValue(object controlMap, PropertyInfo property, object value)
        {
            var typeFactory = value.GetTypeFactory();

            var propertyType = property.PropertyType;
            if (typeof(AutomationControl).IsAssignableFrom(propertyType))
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

            if (typeof(AutomationControl).IsAssignableFrom(collectionElementType))
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
