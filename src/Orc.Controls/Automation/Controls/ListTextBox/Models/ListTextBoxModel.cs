namespace Orc.Controls.Automation;

using System.Collections.Generic;
using Orc.Automation;

[AutomationAccessType]
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
