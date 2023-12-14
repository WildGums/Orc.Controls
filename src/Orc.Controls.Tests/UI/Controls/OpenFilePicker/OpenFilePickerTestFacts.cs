namespace Orc.Controls.Tests.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using Catel.IO;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Controls;
    using Orc.Controls.UI;
    using Tests;

    [Explicit]
    [TestFixture(TestOf = typeof(OpenFilePicker))]
    [Category("UI Tests")]
    public class OpenFilePickerTestFacts : StyledControlTestFacts<OpenFilePicker>
    {
        [Target]
        public Orc.Controls.Automation.OpenFilePicker Target { get; set; }

        [Test]
        public void CorrectlyOpenContainingDirectory()
        {
            var target = Target;
            var model = target.Current;

            var filePath = TestDataFileSystemPathHelper.File22Path;
            var folderName = Path.GetDirectoryName(filePath)
                .Split('\\').LastOrDefault();

            //Close already opened file explorer windows with Folder21 opened
            var alreadyOpenedWindows = AutomationElement.RootElement.FindAll<Window>(name: folderName, scope: TreeScope.Children);
            foreach (var alreadyOpenedWindow in alreadyOpenedWindows ?? Enumerable.Empty<Window>())
            {
                alreadyOpenedWindow.Close();
            }

            model.SelectedFile = filePath;

            //open directory
            target.OpenContainingDirectory();

            Wait.UntilResponsive(200);

            var directoryWindow = AutomationElement.RootElement.Find<Window>(name: folderName, scope: TreeScope.Children);

            Assert.That(directoryWindow, Is.Not.Null);

            directoryWindow.Close();
        }

        [TestCaseSource(typeof(FilterTestSource))]
        public void CorrectlySetFileFilter(string filterStr, List<string> expectedFilterList)
        {
            var target = Target;
            var model = target.Current;

            model.Filter = filterStr;

            Assert.That(expectedFilterList, Is.EquivalentTo(target.Filters));
        }
        
        [TestCaseSource(typeof(TestFilePathsBaseDirectoryExpectedResultPathSource))]
        public void CorrectlySetBaseDirectory(string absoluteFilePath, string baseDirectory, string expectedDisplayedFilePath)
        {
            var target = Target;
            var model = target.Current;

            target.SelectFile(absoluteFilePath);
            model.BaseDirectory = baseDirectory;

            Wait.UntilResponsive();

            OpenFilePickerAssert.SelectedFile(target, expectedDisplayedFilePath, absoluteFilePath);
        }

        [TestCaseSource(typeof(TestFilePathsSource))]
        public void CorrectlySetSelectedFile(string filePath)
        {
            var target = Target;

            target.SelectFile(filePath);

            Wait.UntilResponsive();

            OpenFilePickerAssert.SelectedFile(target, filePath);
        }

        [Test]
        public void CorrectlyClearSelectedFile()
        {
            var target = Target;

            target.SelectFile(TestDataFileSystemPathHelper.File21Path);

            Wait.UntilResponsive();

            target.Clear();

            OpenFilePickerAssert.SelectedFile(target, string.Empty);
        }

        private class TestFilePathsBaseDirectoryExpectedResultPathSource : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new [] { TestDataFileSystemPathHelper.File2Path, TestDataFileSystemPathHelper.Folder1Path, "..\\Folder2\\File2.txt" };
                yield return new [] { TestDataFileSystemPathHelper.File21Path, TestDataFileSystemPathHelper.Folder2Path, "Folder21\\File21.txt" };
            }
        }

        private class TestFilePathsSource : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return TestDataFileSystemPathHelper.File22Path;
                yield return TestDataFileSystemPathHelper.File1Path;
                yield return string.Empty;
            }
        }

        private class FilterTestSource : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                const string filterName1 = "Image files (*.bmp, *.jpg)";
                const string filter1 = "*.bmp;*.jpg";

                const string filterName2 = "All files (*.*)";
                const string filter2 = "*.*";

                yield return new object[] { $"{filterName1}|{filter1}|{filterName2}|{filter2}", new List<string> { filterName1, filterName2 } };
            }
        }
    }
}
