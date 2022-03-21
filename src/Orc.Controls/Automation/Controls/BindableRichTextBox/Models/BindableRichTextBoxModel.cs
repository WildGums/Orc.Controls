namespace Orc.Controls.Automation;

using System.Windows.Documents;
using Orc.Automation;

[ActiveAutomationModel]
public class BindableRichTextBoxModel : FrameworkElementModel
{
    public BindableRichTextBoxModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public FlowDocument BindableDocument { get; set; }

    public bool AutoScrollToEnd { get; set; }
}
