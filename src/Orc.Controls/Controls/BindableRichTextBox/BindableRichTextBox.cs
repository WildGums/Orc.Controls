namespace Orc.Controls;

using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Documents;
using Automation;

public partial class BindableRichTextBox : RichTextBox
{
    public BindableRichTextBox()
    {
        TextChanged += OnTextChanged;
    }

    #region Dependency properties
    public FlowDocument? BindableDocument
    {
        get { return (FlowDocument?)GetValue(BindableDocumentProperty); }
        set { SetValue(BindableDocumentProperty, value); }
    }

    public static readonly DependencyProperty BindableDocumentProperty = DependencyProperty.Register(nameof(BindableDocument),
        typeof(FlowDocument), typeof(BindableRichTextBox), new PropertyMetadata((sender, args) => ((BindableRichTextBox)sender).OnBindableDocumentChanged(args)));


    public bool AutoScrollToEnd
    {
        get { return (bool)GetValue(AutoScrollToEndProperty); }
        set { SetValue(AutoScrollToEndProperty, value); }
    }

    public static readonly DependencyProperty AutoScrollToEndProperty = DependencyProperty.Register(nameof(AutoScrollToEnd),
        typeof(bool), typeof(BindableRichTextBox), new PropertyMetadata(default(bool)));
    #endregion

    private void OnBindableDocumentChanged(DependencyPropertyChangedEventArgs args)
    {
        Document = args.NewValue is null ? new FlowDocument() : (FlowDocument)args.NewValue;
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (AutoScrollToEnd)
        {
            ScrollToEnd();
        }
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new BindableRichTextBoxAutomationPeer(this);
    }
}
