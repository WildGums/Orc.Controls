namespace Orc.Controls.Example.ViewModels;

using System;
using System.Windows.Documents;
using Catel.MVVM;

public class BindableRichTextBoxViewModel : ViewModelBase
{
    public BindableRichTextBoxViewModel(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        FlowDoc = CreateFlowDocument("This is example text colored with AccentColor");

        ClearText = new Command(serviceProvider, OnClearText);
    }

    public FlowDocument FlowDoc { get; set; }

    public bool UseAccentText { get; set; }

    public Command ClearText { get; set; }

    private void OnClearText()
    {
        FlowDoc = CreateFlowDocument();
    }

    private FlowDocument CreateFlowDocument(string text = null)
    {
        var flowDoc = new FlowDocument();
        var exampleParagraph = new Paragraph(new Run(text ?? string.Empty));

        if (UseAccentText)
        {
            exampleParagraph.Foreground = Theming.ThemeManager.Current.GetAccentColorBrush().Clone();
        }

        flowDoc.Blocks.Add(exampleParagraph);

        return flowDoc;
    }

    private void OnUseAccentTextChanged()
    {
        FlowDoc = CreateFlowDocument("This is example text colored with AccentColor");
    }
}
