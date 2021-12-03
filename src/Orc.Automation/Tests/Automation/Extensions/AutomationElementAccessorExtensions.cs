namespace Orc.Automation
{
    using Catel;

    public static class AutomationElementAccessorExtensions
    {
        public static T GetValue<T>(this AutomationElementAccessor automationElementAccessor)
        {
            Argument.IsNotNull(() => automationElementAccessor);

            return (T)automationElementAccessor.GetValue();
        }

        public static T GetValue<T>(this AutomationElementAccessor automationElementAccessor, string propertyName)
        {
            Argument.IsNotNull(() => automationElementAccessor);

            return (T) automationElementAccessor.GetValue(propertyName);
        }

        public static object GetValue(this AutomationElementAccessor automationElementAccessor)
        {
            Argument.IsNotNull(() => automationElementAccessor);

            var propertyName = AutomationHelper.GetCallingProperty();
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new AutomationException("Can't find calling property");
            }

            return automationElementAccessor.GetValue(propertyName);
        }

        public static void SetValue(this AutomationElementAccessor automationElementAccessor, object value)
        {
            Argument.IsNotNull(() => automationElementAccessor);

            var propertyName = AutomationHelper.GetCallingProperty();
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new AutomationException("Can't find calling property");
            }

            automationElementAccessor.SetValue(propertyName, value);
        }
        
        public static T Execute<T>(this AutomationElementAccessor automationElementAccessor, string methodName, params object[] parameters)
        {
            Argument.IsNotNull(() => automationElementAccessor);

            return (T)automationElementAccessor.Execute(methodName, parameters);
        }
    }
}
