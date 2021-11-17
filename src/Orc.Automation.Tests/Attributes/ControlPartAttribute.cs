namespace Orc.Automation.Tests
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class ControlPartAttribute : Attribute
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
    }
}
