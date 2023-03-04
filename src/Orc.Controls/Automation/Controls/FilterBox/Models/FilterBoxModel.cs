#nullable disable
namespace Orc.Controls.Automation;

using System.Collections;
using Orc.Automation;

[ActiveAutomationModel]
public class FilterBoxModel : FrameworkElementModel
{
    public FilterBoxModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public string Text { get; set; }

    public bool AllowAutoCompletion { get; set; }
    public IEnumerable FilterSource { get; set; }
    public string PropertyName { get; set; }
    public string Watermark { get; set; }
}
#nullable enable
