namespace Orc.Automation
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Automation;

    [AttributeUsage(AttributeTargets.Property)]
    public class TargetAttribute : AutomationAttribute
    {
        public static object GetTarget(object source)
        {
            var targetControlProperty = GetTargetProperty(source);
            return targetControlProperty?.GetValue(source);
        }

        public static PropertyInfo GetTargetProperty(object source)
        {
            return source.GetType().GetProperties().FirstOrDefault(prop => IsDefined(prop, typeof(TargetAttribute)));
        }

        public static void ResolveTargetProperty(AutomationElement targetElement, object source)
        {
            var targetControlProperty = GetTargetProperty(source);
            if (targetControlProperty is null)
            {
                return;
            }

            var result = AutomationHelper.WrapAutomationObject(targetControlProperty.PropertyType, targetElement);
            targetControlProperty.SetValue(source, result);
        }
    }
}
