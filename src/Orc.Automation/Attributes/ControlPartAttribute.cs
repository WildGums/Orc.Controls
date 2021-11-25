namespace Orc.Automation
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Windows.Automation;
    using Catel.IoC;
    using Catel.Reflection;

    public interface IControlPartSearchInfo
    {
        public string AutomationId { get; }
        public string Name { get; }
        public string ClassName { get; }
        public ControlType ControlType { get; }
        public bool IsTransient { get; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ControlPartAttribute : AutomationAttributeBase, IControlPartSearchInfo
    {
        public ControlPartAttribute()
        {
            
        }

        public ControlPartAttribute(string automationId)
        {
            AutomationId = automationId;
        }

        public string AutomationId { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string ControlType { get; set; }
        public bool IsTransient { get; set; }

        ControlType IControlPartSearchInfo.ControlType 
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ControlType))
                {
                    return null;
                }

                return typeof(ControlType).GetField(ControlType)?.GetValue(null) as ControlType;
            }
        }


        public static void ResolvePartProperties(AutomationElement targetElement, object controlMap)
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

                var wrappedObject = AutomationHelper.WrapAutomationObject(property.PropertyType, propertyValue);
                property.SetValue(controlMap, wrappedObject);
            }
        }
    }
}
