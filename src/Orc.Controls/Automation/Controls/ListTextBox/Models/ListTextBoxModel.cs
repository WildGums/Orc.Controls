#nullable disable
namespace Orc.Controls.Automation;

using System.Collections.Generic;
using Orc.Automation;

[ActiveAutomationModel]
public class ListTextBoxModel : FrameworkElementModel
{
    public ListTextBoxModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public string Text { get; set; }
    public List<string> ListOfValues { get; set; }
    public string Value { get; set; }
}
#nullable enable
