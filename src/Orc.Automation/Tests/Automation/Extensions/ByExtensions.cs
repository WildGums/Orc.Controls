namespace Orc.Automation
{
    using System.Collections.Generic;
    using System.Reflection;
    using Catel;

    

    public static class ByExtensions
    {
        public static By Id(this By by)
        {
            Argument.IsNotNull(() => by);

            var property = AutomationHelper.GetCallingProperty();
            if (property is null)
            {
                throw new AutomationException("Can't find calling property");
            }

            return by.Id(property);
        }

        public static By Name(this By by)
        {
            Argument.IsNotNull(() => by);

            var property = AutomationHelper.GetCallingProperty();
            if (property is null)
            {
                throw new AutomationException("Can't find calling property");
            }

            return by.Name(property);
        }

        public static T One<T>(this By by)
        {
            Argument.IsNotNull(() => by);

            PrepareSearch<T>(by);

            var result = by.One();
            if (result is null)
            {
                return default;
            }

            return (T) AutomationHelper.WrapAutomationObject(typeof(T), result);
        }

        public static List<T> Many<T>(this By by)
        {
            Argument.IsNotNull(() => by);

            PrepareSearch<T>(by);

            var result = by.Many();
            if (result is null)
            {
                return default;
            }

            return (List<T>)AutomationHelper.WrapAutomationObject(typeof(List<T>), result);
        }

        private static void PrepareSearch<T>(By by)
        {
            var type = typeof(T);

            var controlType = AutomationHelper.GetControlType(type.Name);
            if (controlType is not null)
            {
                by.ControlType(controlType);
            }

            var automatedControlAttribute = type.GetCustomAttribute<AutomatedControlAttribute>();
            if (automatedControlAttribute is not null)
            {
                var className = automatedControlAttribute.ClassName;
                if (!string.IsNullOrWhiteSpace(className))
                {
                    by.ClassName(automatedControlAttribute.ClassName);
                }

                controlType = automatedControlAttribute.ControlType;
                if (controlType is not null)
                {
                    by.ControlType(controlType);
                }
            }
        }
    }
}
