namespace Orc.Controls.Automation
{
    using Catel.Data;
    using Orc.Automation;

    [AutomationAccessType]
    public class ValidationContextViewModel : ControlModel
    {
        public ValidationContextViewModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }
        
        //[SerializationAutomationConverter(ConverterType = typeof(ValidationContextSerializableConverter))]
        public ValidationContext ValidationContext { get; set; }
        public bool ShowFilterBox { get; set; }
        public bool ShowButtons { get; set; }
        public bool IsExpandedAllOnStartup { get; set; }
    }
}
