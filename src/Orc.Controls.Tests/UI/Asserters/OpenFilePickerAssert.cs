namespace Orc.Controls.Tests
{
    using NUnit.Framework;

    public static class OpenFilePickerAssert
    {
        public static void SelectedFile(Automation.OpenFilePicker target, string displayedFilePath, string selectedFilePath = null)
        {
            var model = target.Current;

            if (selectedFilePath is null)
            {
                selectedFilePath = displayedFilePath;
            }

            Assert.That(target.SelectedFileDisplayPath, Is.EqualTo(displayedFilePath));
            Assert.That(model.SelectedFile, Is.EqualTo(selectedFilePath));
        }
    }
}
