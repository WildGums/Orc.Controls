namespace Orc.Automation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Automation;
    using Catel;
    using Catel.IoC;

    public static class AutomationHelper
    {
        public static object WrapAutomationObject(Type type, object value)
        {
            Argument.IsNotNull(() => value);

            var typeFactory = value.GetTypeFactory();

            if (typeof(AutomationControl).IsAssignableFrom(type))
            {
                return typeFactory.CreateInstanceWithParametersAndAutoCompletion(type, value);
            }

            if (type == typeof(AutomationElement))
            {
                return value;
            }

            var collectionElementType = type.GetAnyElementType();
            if (collectionElementType is null)
            {
                return null;
            }

            if (value is not IEnumerable<AutomationElement> valueElements)
            {
                return null;
            }

            if (typeFactory.CreateInstanceWithParametersAndAutoCompletion(type) is not IList elementCollection)
            {
                return null;
            }

            if (typeof(AutomationControl).IsAssignableFrom(collectionElementType))
            {
                foreach (var automationElement in valueElements)
                {
                    elementCollection.Add(typeFactory.CreateInstanceWithParametersAndAutoCompletion(collectionElementType, automationElement));
                }

                return elementCollection;
            }

            if (type == typeof(AutomationElement))
            {
                foreach (var automationElement in valueElements)
                {
                    elementCollection.Add(automationElement);
                }

                return elementCollection;
            }

            return null;
        }
    }
}
