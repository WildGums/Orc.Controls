namespace Orc.Controls.Tests.UI
{
    using NUnit.Framework;
    using Orc.Automation;
    using Tests;

    [Explicit]
    [TestFixture(TestOf = typeof(SaveFilePicker))]
    [Category("UI Tests")]
    public class SaveFilePickerTestFacts : StyledControlTestFacts<SaveFilePicker>
    {
        [Target]
        public Automation.SaveFilePicker Target { get; set; }

        [Test]
        public void CorrectlySaveContainingDirectory()
        {
            var target = Target;
            var model = target.Current;

            model.LabelText = "Text";
        }
    }
}
