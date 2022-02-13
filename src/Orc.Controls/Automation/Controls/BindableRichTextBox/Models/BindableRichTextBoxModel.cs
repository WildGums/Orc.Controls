namespace Orc.Controls.Automation;

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Orc.Automation;

[AutomationAccessType]
public class BindableRichTextBoxModel : ControlModel
{
    public BindableRichTextBoxModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public FlowDocument BindableDocument { get; set; }

    public bool AutoScrollToEnd { get; set; }
}
