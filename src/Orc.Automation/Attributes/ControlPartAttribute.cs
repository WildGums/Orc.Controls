namespace Orc.Automation
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Windows.Automation;
    using Automation;
    using Catel.Reflection;


    [AttributeUsage(AttributeTargets.Property)]
    public class ControlPartAttribute : AutomationAttributeBase
    {
        private string _className;

        public ControlPartAttribute()
        {
        }

        public ControlPartAttribute(string automationId)
        {
            AutomationId = automationId;
        }
        
        public string AutomationId { get; set; }
        public string Name { get; set; }

        public string ClassName
        {
            get => Class?.FullName ?? _className;
            set => _className = value;
        }

        public Type Class { get; set; }

        public string ControlType { get; set; }
        public bool IsTransient { get; set; }

        public SearchContext GetSearchContext()
        {
            ControlType controlType = null;
            if (!string.IsNullOrWhiteSpace(ControlType))
            {
                controlType = typeof(ControlType).GetField(ControlType)?.GetValue(null) as ControlType;
            }

            return new SearchContext(AutomationId, Name, ClassName, controlType);
        }
    }
}
