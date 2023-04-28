namespace Orc.Controls.Tests;

using System.Threading;
using System.Threading.Tasks;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using Moq;
using NUnit.Framework;
using Orc.Automation.Tests;

[TestFixture]
public class DialogWindowHostedToolBaseTests
{
    [Test]
    [Apartment(ApartmentState.STA)]
    public async Task After_Closing_Tool_Dialog_Tool_Must_Be_Closed_But_Not_Before_Async()
    {
        //Prepare
        var typeFactoryMockObject = Mock.Of<ITypeFactory>();
        var iuiVisualizerServiceMock = new Mock<IUIVisualizerService>();
        iuiVisualizerServiceMock.Setup(x => x.ShowContextAsync(It.IsAny<UIVisualizerContext>()))
            .Callback<UIVisualizerContext>(async (x) =>
            {
                x.CompletedCallback.Invoke(x, new UICompletedEventArgs(new UIVisualizerResult(true, x, null)));
                var viewModel = (DummyViewModel)x.Data;
                //Closing dialog
                await viewModel.SaveAndCloseViewModelAsync();
            });
        var iuiVisualizerServiceMockObject = iuiVisualizerServiceMock.Object;

        //Testing object
        var tool = new TestDialogWindowHostedTool(typeFactoryMockObject, iuiVisualizerServiceMockObject);

        //Act
        //Assert that BEFORE closing dialog tool is OPENED
        tool.Opened += (_, _) => Assert.That(tool.IsOpened, Is.True);
        EventAssert.Raised(tool, nameof(tool.Opened), () => tool.Open());

        //Wait for dialog to be closed
        await Task.Delay(500);

        //Assert that tool is CLOSED
        Assert.That(tool.IsOpened, Is.False);
    }

    public class DummyViewModel : ViewModelBase
    {
    }

    public class TestDialogWindowHostedTool : DialogWindowHostedToolBase<DummyViewModel>
    {
        public TestDialogWindowHostedTool(ITypeFactory typeFactory, IUIVisualizerService uiVisualizerService)
            : base(typeFactory, uiVisualizerService)
        {
        }

        public override string Name => "Test tool";

        protected override void OnAccepted()
        {
          
        }

        protected override DummyViewModel InitializeViewModel()
        {
            return new DummyViewModel();
        }
    }
}
