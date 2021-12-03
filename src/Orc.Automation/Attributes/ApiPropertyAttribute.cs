namespace Orc.Automation
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class ApiPropertyAttribute : AutomationAttribute
    {
        public ApiPropertyAttribute()
        {
            
        }

        public ApiPropertyAttribute(string originalName)
        {
            OriginalName = originalName;
        }

        public string OriginalName { get; set; }
       // public string ConnectedViewProperty { get; set; }
    }
}
