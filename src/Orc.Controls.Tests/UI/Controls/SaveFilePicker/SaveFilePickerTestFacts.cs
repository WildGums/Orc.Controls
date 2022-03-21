namespace Orc.Controls.UI
{
    using NUnit.Framework;
    using Orc.Automation;
    using Tests;

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
