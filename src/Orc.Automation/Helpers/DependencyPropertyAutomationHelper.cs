namespace Orc.Automation
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using Catel;

    public static class DependencyPropertyAutomationHelper
    {
        public static bool TryGetDependencyPropertyValue(DependencyObject element, string propertyName, out object propertyValue)
        {
            Argument.IsNotNull(() => element);

            propertyValue = null;

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return false;
            }

            var dependencyProperty = GetDependencyPropertyByName(element, propertyName);
            if (dependencyProperty is null)
            {
                return false;
            }

            var dependencyPropertyValue = element.GetValue(dependencyProperty);

            //TODO:Vladimir:find better way to do this
            if (dependencyPropertyValue is IEnumerable enumerableValue && dependencyPropertyValue.GetType().Name.StartsWith("IEnumerable"))
            {
                dependencyPropertyValue = enumerableValue.OfType<string>().ToList();
            }

            propertyValue = dependencyPropertyValue;

            return true;
        }

        public static bool SetDependencyPropertyValue(DependencyObject element, string propertyName, object value)
        {
            Argument.IsNotNull(() => element);

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return false;
            }

            var dependencyProperty = GetDependencyPropertyByName(element, propertyName);
            if (dependencyProperty is null)
            {
                return false;
            }

            element.SetCurrentValue(dependencyProperty, value);

            return true;
        }

        private static DependencyProperty GetDependencyPropertyByName(DependencyObject dependencyObject, string propertyName)
        {
            return GetDependencyPropertyByName(dependencyObject.GetType(), propertyName);
        }

        private static DependencyProperty GetDependencyPropertyByName(Type dependencyObjectType, string propertyName)
        {
            var dependencyProperty = DependencyPropertyDescriptor.FromName(propertyName, dependencyObjectType, dependencyObjectType)?.DependencyProperty;
            return dependencyProperty;
        }
    }
}
