﻿namespace Orc.Controls.Tests;

using System.Threading;
using System.Threading.Tasks;
using Catel.MVVM;
using Catel.Services;
using Moq;
using NUnit.Framework;

[TestFixture]
public class DialogWindowHostedToolBaseTests
{
    [Test]
    [Apartment(ApartmentState.STA)]
    public async Task After_Closing_Tool_Dialog_Tool_Must_Be_Closed_But_Not_Before_Async()
    {
        //Prepare
        const int windowLifeTime = 500;

        var uiVisualizerServiceMock = new Mock<IUIVisualizerService>();
        uiVisualizerServiceMock.Setup(x => x.ShowContextAsync(It.IsAny<UIVisualizerContext>()))
            .Callback<UIVisualizerContext>(x =>
            {
                Thread.Sleep(windowLifeTime);

                x.CompletedCallback?.Invoke(x, new UICompletedEventArgs(new UIVisualizerResult(true, x, null)));
            });
        var iuiVisualizerServiceMockObject = uiVisualizerServiceMock.Object;

        //Testing object
        var tool = new TestDialogWindowHostedTool(iuiVisualizerServiceMockObject);

        //Act
        var isOpened = false;

        //Assert that BEFORE closing dialog tool is OPENED
        tool.Opened += (_, _) =>
        {
            isOpened = true;
            Assert.That(tool.IsOpened, Is.True);
        };

        await tool.OpenAsync();

        Assert.That(isOpened, Is.True);

        //Wait for dialog to be closed
        await Task.Delay(windowLifeTime + 100);

        //Assert that tool is CLOSED
        Assert.That(tool.IsOpened, Is.False);
    }

    public class DummyViewModel : ViewModelBase
    {
    }

    public class TestDialogWindowHostedTool : DialogWindowHostedToolBase<DummyViewModel>
    {
        public TestDialogWindowHostedTool(IUIVisualizerService uiVisualizerService)
            : base(uiVisualizerService)
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
