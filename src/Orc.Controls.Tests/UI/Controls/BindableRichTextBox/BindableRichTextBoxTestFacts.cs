namespace Orc.Controls.Tests.UI
{
    using System.Threading;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;
    using NUnit.Framework;
    using Orc.Automation;

    [Explicit]
    [TestFixture(TestOf = typeof(BindableRichTextBox))]
    [Category("UI Tests")]
    public class BindableRichTextBoxTestFacts : StyledControlTestFacts<BindableRichTextBox>
    {
        [Target]
        public Automation.BindableRichTextBox Target { get; set; }

        [Apartment(ApartmentState.STA)]
        [TestCase("This is highlighted text!", "This is highlighted text!\r\n")]
        public void CorrectlySetDocument(string testText, string expectedText)
        {
            var target = Target;
            var model = target.Current;

            model.BindableDocument = CreateFlowDocument(testText);

            Wait.UntilResponsive();

            target.SetFocus();

            Wait.UntilResponsive();

            KeyboardInputEx.SelectAll();
            KeyboardInputEx.Copy();

            Wait.UntilResponsive();

            var text = Clipboard.GetText(TextDataFormat.Text);

            Assert.That(text, Is.EqualTo(expectedText));
        }

        [TestCase("This text", "This text\r\n")]
        public void CorrectlyInputText(string inputText, string expectedText)
        {
            var target = Target;
            var model = target.Current;

            model.BindableDocument = new FlowDocument();

            target.SetFocus();
            Wait.UntilResponsive();

            KeyboardInput.Type(inputText);

            var document = model.BindableDocument;
            var documentText = new TextRange(document.ContentStart, document.ContentEnd).Text;

            Assert.That(documentText, Is.EqualTo(expectedText));
        }

        private static FlowDocument CreateFlowDocument(string text = null)
        {
            var flowDoc = new FlowDocument();
            var exampleParagraph = new Paragraph(new Run(text ?? string.Empty))
            {
                Foreground = Brushes.Blue
            };

            flowDoc.Blocks.Add(exampleParagraph);

            return flowDoc;
        }
    }
}
