// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindableRichTextBox.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.ViewModels
{
    using System.Windows.Documents;
    using Catel.MVVM;

    public class BindableRichTextBoxViewModel : ViewModelBase
    {
        #region Constructors
        public BindableRichTextBoxViewModel()
        {
            FlowDoc = CreateFlowDocument("This is example text colored with AccentColor");

            ClearText = new Command(OnClearText);
        }
        #endregion

        #region Properties
        public FlowDocument FlowDoc { get; set; }

        public bool UseAccentText { get; set; }

        public Command ClearText { get; set; }
        #endregion

        #region Methods
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
        #endregion
    }
}
