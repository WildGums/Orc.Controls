#nullable disable
namespace Orc.Controls.Automation;

using Catel.Data;
using Orc.Automation;

[ActiveAutomationModel]
public class ValidationContextViewModel : FrameworkElementModel
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
#nullable enable
