namespace Orc.Controls.Tests.Helpers;

using System;
using System.Threading;
using NUnit.Framework;

[TestFixture]
[Apartment(ApartmentState.STA)] // Требуется для DispatcherTimer
public class DispatcherTimerWrapperTests
{
    [Test]
    public void Constructor_CreatesDispatcherTimer()
    {
        // Act
        var wrapper = new DispatcherTimerWrapper();

        // Assert
        Assert.That(wrapper, Is.Not.Null);
    }

    [Test]
    public void Interval_SetAndGet_ReturnsCorrectValue()
    {
        // Arrange
        var wrapper = new DispatcherTimerWrapper();
        var interval = TimeSpan.FromMilliseconds(500);

        // Act
        wrapper.Interval = interval;

        // Assert
        Assert.That(wrapper.Interval, Is.EqualTo(interval));
    }

    [Test]
    public void Start_SetsIsEnabledToTrue()
    {
        // Arrange
        var wrapper = new DispatcherTimerWrapper
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };

        // Act
        wrapper.Start();

        // Assert
        Assert.That(wrapper.IsEnabled, Is.True);

        // Cleanup
        wrapper.Stop();
    }

    [Test]
    public void Stop_SetsIsEnabledToFalse()
    {
        // Arrange
        var wrapper = new DispatcherTimerWrapper
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        wrapper.Start();

        // Act
        wrapper.Stop();

        // Assert
        Assert.That(wrapper.IsEnabled, Is.False);
    }
}
