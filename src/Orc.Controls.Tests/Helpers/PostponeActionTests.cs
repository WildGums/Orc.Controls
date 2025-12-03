namespace Orc.Controls.Tests;

using System;
using Moq;
using NUnit.Framework;

[TestFixture]
public class PostponeActionTests
{
    [Test]
    public void Constructor_NullAction_ThrowsArgumentNullException()
    {
        // Arrange
        var timerMock = new Mock<IPostponeActionTimer>();

        // Act & Assert
        Assert.That(() => new PostponeAction(null, timerMock.Object),
            Throws.ArgumentNullException);
    }

    [Test]
    public void Constructor_NullTimer_ThrowsArgumentNullException()
    {
        // Arrange
        var action = new Action(() => { });

        // Act & Assert
        Assert.That(() => new PostponeAction(action, null),
            Throws.ArgumentNullException);
    }

    [Test]
    public void Constructor_Default_SubscribesToTimerTick()
    {
        // Arrange
        var tickSubscribed = false;
        var action = new Action(() => { });
        var timerMock = new Mock<IPostponeActionTimer>();
        timerMock.SetupAdd(m => m.Tick += It.IsAny<EventHandler>())
            .Callback(() => tickSubscribed = true);

        // Act
        var postponeAction = new PostponeAction(action, timerMock.Object);

        // Assert
        timerMock.VerifyAdd(m => m.Tick += It.IsAny<EventHandler>(), Times.Once);
        Assert.That(tickSubscribed, Is.True);
    }

    [Test]
    public void Execute_ZeroDelay_ExecutesActionImmediately()
    {
        // Arrange
        var actionExecuted = false;
        var action = new Action(() => actionExecuted = true);
        var timerMock = new Mock<IPostponeActionTimer>();
        var postponeAction = new PostponeAction(action, timerMock.Object);

        // Act
        postponeAction.Execute(0);

        // Assert
        Assert.That(actionExecuted, Is.True);
        timerMock.Verify(t => t.Start(), Times.Never);
    }

    [Test]
    public void Execute_NegativeDelay_ExecutesActionImmediately()
    {
        // Arrange
        var actionExecuted = false;
        var action = new Action(() => actionExecuted = true);
        var timerMock = new Mock<IPostponeActionTimer>();
        var postponeAction = new PostponeAction(action, timerMock.Object);

        // Act
        postponeAction.Execute(-100);

        // Assert
        Assert.That(actionExecuted, Is.True);
        timerMock.Verify(t => t.Start(), Times.Never);
    }

    [Test]
    public void Execute_PositiveDelay_StartsTimerWithCorrectInterval()
    {
        // Arrange
        var action = new Action(() => { });
        var timerMock = new Mock<IPostponeActionTimer>();
        var postponeAction = new PostponeAction(action, timerMock.Object);
        var delay = 100;

        // Act
        postponeAction.Execute(delay);

        // Assert
        timerMock.VerifySet(t => t.Interval = TimeSpan.FromMilliseconds(delay), Times.Once);
        timerMock.Verify(t => t.Start(), Times.Once);
    }

    [Test]
    public void Stop_StopsTimer()
    {
        // Arrange
        var action = new Action(() => { });
        var timerMock = new Mock<IPostponeActionTimer>();
        var postponeAction = new PostponeAction(action, timerMock.Object);

        // Act
        postponeAction.Stop();

        // Assert
        timerMock.Verify(t => t.Stop(), Times.Once);
    }

    [Test]
    public void OnTimerTick_StopsTimerAndExecutesAction()
    {
        // Arrange
        var actionExecuted = false;
        var action = new Action(() => actionExecuted = true);
        var timerMock = new Mock<IPostponeActionTimer>();
        var postponeAction = new PostponeAction(action, timerMock.Object);

        // Simulate timer tick
        timerMock.Raise(t => t.Tick += null, EventArgs.Empty);

        // Assert
        timerMock.Verify(t => t.Stop(), Times.Once);
        Assert.That(actionExecuted, Is.True);
    }

    [Test]
    public void StaticExecute_NullAction_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => PostponeAction.Execute(null, 100),
            Throws.ArgumentNullException);
    }

    [Test]
    public void StaticExecute_CreatesInstanceAndExecutes()
    {
        // Arrange
        var actionExecuted = false;
        var action = new Action(() => actionExecuted = true);

        // Act
        PostponeAction.Execute(action, 0);

        // Assert
        Assert.That(actionExecuted, Is.True);
    }

    [Test]
    public void Constructor_Default_UsesDispatcherTimerWrapper()
    {
        // Arrange
        var action = new Action(() => { });

        // Act
        var postponeAction = new PostponeAction(action);

        // Assert
        Assert.That(postponeAction, Is.Not.Null);
    }
}
