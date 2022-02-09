namespace Orc.Controls.Automation;

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Orc.Automation;


//public class SetBindableDocumentAutomationMethodRun : NamedAutomationMethodRun
//{
//    public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
//    {
//        result = AutomationValue.FromValue(10);

//        if (owner is not Controls.BindableRichTextBox textBox)
//        {
//            return false;
//        }

//        var flowDocument = CreateFlowDocument("Emphasised text");
//        textBox.BindableDocument = flowDocument;

//        return true;
//    }

//    private FlowDocument CreateFlowDocument(string text = null)
//    {
//        var flowDoc = new FlowDocument();
//        var exampleParagraph = new Paragraph(new Run(text ?? string.Empty))
//        {
//            Foreground = Brushes.Blue
//        };

//        flowDoc.Blocks.Add(exampleParagraph);

//        return flowDoc;
//    }
//}

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
