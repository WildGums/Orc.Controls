namespace Orc.Automation
{
    using System;
    using System.Linq;
    using System.Windows.Automation;

    [AttributeUsage(AttributeTargets.Property)]
    public class TargetAttribute : AutomationAttributeBase
    {
        public static void ResolveTargetProperty(AutomationElement targetElement, object template)
        {
            var targetControlProperty = template.GetType().GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(TargetAttribute)));
            if (targetControlProperty is null)
            {
                return;
            }

            var result = AutomationHelper.WrapAutomationObject(targetControlProperty.PropertyType, targetElement);
            targetControlProperty.SetValue(template, result);
        }
    }
}
