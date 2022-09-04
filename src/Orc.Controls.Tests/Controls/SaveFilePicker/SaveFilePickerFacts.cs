namespace Orc.Controls.Tests.Controls
{
    using System.Threading.Tasks;
    using Catel.Services;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SaveFilePickerFacts
    {
        [TestCase(null, null)]
        [TestCase("some_item.txt", "some_item.txt")]
        public async Task UsesDefaultFileName_NoFileSelected_Async(string input, string expectedOutput)
        {
            var isCalled = false;

            var saveFileServiceMock = new Mock<ISaveFileService>();
            saveFileServiceMock.Setup(x => x.DetermineFileAsync(It.IsAny<DetermineSaveFileContext>()))
                .Returns<DetermineSaveFileContext>(async x =>
                {
                    isCalled = true;

                    Assert.AreEqual(expectedOutput, x.FileName);

                    return new DetermineSaveFileResult();
                });

            var processServiceMock = new Mock<IProcessService>();

            var vm = new SaveFilePickerViewModel(saveFileServiceMock.Object, processServiceMock.Object);

            vm.InitialFileName = input;

            var taskCommand = vm.SelectFile;
            taskCommand.Execute();

            await taskCommand.Task;

            Assert.IsTrue(isCalled);
        }

        [TestCase(null, "existing_file.txt")]
        [TestCase("some_item.txt", "existing_file.txt")]
        public async Task NotUsesDefaultFileName_FileSelected_Async(string input, string expectedOutput)
        {
            var isCalled = false;

            var saveFileServiceMock = new Mock<ISaveFileService>();
            saveFileServiceMock.Setup(x => x.DetermineFileAsync(It.IsAny<DetermineSaveFileContext>()))
                .Returns<DetermineSaveFileContext>(async x =>
                {
                    isCalled = true;

                    Assert.AreEqual(expectedOutput, x.FileName);

                    return new DetermineSaveFileResult();
                });

            var processServiceMock = new Mock<IProcessService>();

            var vm = new SaveFilePickerViewModel(saveFileServiceMock.Object, processServiceMock.Object);

            vm.InitialFileName = input;
            vm.SelectedFile = "existing_file.txt";

            var taskCommand = vm.SelectFile;
            taskCommand.Execute();

            await taskCommand.Task;

            Assert.IsTrue(isCalled);
        }
    }
}
