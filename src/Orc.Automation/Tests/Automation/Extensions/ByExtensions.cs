namespace Orc.Automation
{
    using System.Collections.Generic;
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

            var result = by.Many();
            if (result is null)
            {
                return default;
            }

            return (List<T>)AutomationHelper.WrapAutomationObject(typeof(List<T>), result);
        }
    }
}