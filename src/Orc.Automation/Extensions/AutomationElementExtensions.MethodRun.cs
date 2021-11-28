namespace Orc.Automation
{
    using System;
    using System.Windows.Automation;

    public static partial class AutomationElementExtensions
    {
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
                var commandAutomationElement = new AutomationElementAccessor(element);
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
                var commandAutomationElement = new AutomationElementAccessor(element);
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
                var commandAutomationElement = new AutomationElementAccessor(element);
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
                var testHostCommand = new AutomationElementAccessor(element);
                result = testHostCommand.Execute(methodName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }

            return true;
        }
    }
}
