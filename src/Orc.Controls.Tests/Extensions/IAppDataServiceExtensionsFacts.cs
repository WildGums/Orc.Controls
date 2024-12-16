namespace Orc.Controls.Tests;

using System;
using System.IO;
using System.Threading;
using System.Windows;
using Catel.IO;
using Catel.Services;
using Moq;
using NUnit.Framework;
using Path = System.IO.Path;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class IAppDataServiceExtensionsFacts
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class TheSaveWindowSizeMethod
    {
        [Test]
        public void ThrowsArgumentNullExceptionForNullWindow()
        {
            var appDataService = new Mock<IAppDataService>().Object;

            Assert.Throws<ArgumentNullException>(() => appDataService.SaveWindowSize(null));
        }

        [Test]
        public void SavesWindowSizeCorrectly()
        {
            // Arrange
            var mockAppDataService = new Mock<IAppDataService>();
            var appData = Path.Combine(Path.GetTempPath(), "TestAppData");
            mockAppDataService.Setup(x => x.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming))
                .Returns(appData);

            var window = new Window();
            // Initialize window properties before setting values
            window.SetCurrentValue(FrameworkElement.WidthProperty, 800d);
            window.SetCurrentValue(FrameworkElement.HeightProperty, 600d);
            window.SetCurrentValue(Window.LeftProperty, 100d);
            window.SetCurrentValue(Window.TopProperty, 200d);
            window.SetCurrentValue(Window.WindowStateProperty, WindowState.Normal);

            // Act
            mockAppDataService.Object.SaveWindowSize(window);

            // Assert
            var expectedPath = Path.Combine(appData, "windows", $"{window.GetType().FullName}.dat");
            Assert.That(File.Exists(expectedPath));

            var contents = File.ReadAllText(expectedPath);
            var parts = contents.Split('|');
            Assert.That(parts.Length, Is.EqualTo(5));
            Assert.That(double.Parse(parts[0]), Is.EqualTo(800));
            Assert.That(double.Parse(parts[1]), Is.EqualTo(600));
            Assert.That(parts[2], Is.EqualTo("Normal"));
            Assert.That(double.Parse(parts[3]), Is.EqualTo(100));
            Assert.That(double.Parse(parts[4]), Is.EqualTo(200));

            // Cleanup
            Directory.Delete(appData, true);
        }

        [Test]
        public void SavesWindowSizeWithTagCorrectly()
        {
            // Arrange
            var mockAppDataService = new Mock<IAppDataService>();
            var appData = Path.Combine(Path.GetTempPath(), "TestAppData");
            mockAppDataService.Setup(x => x.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming))
                .Returns(appData);

            var window = new Window();
            window.SetCurrentValue(FrameworkElement.WidthProperty, 800d);
            window.SetCurrentValue(FrameworkElement.HeightProperty, 600d);
            var tag = "TestTag";

            // Act
            mockAppDataService.Object.SaveWindowSize(window, tag);

            // Assert
            var expectedPath = Path.Combine(appData, "windows", $"{window.GetType().FullName}_testtag.dat");
            Assert.That(File.Exists(expectedPath));

            // Cleanup
            Directory.Delete(appData, true);
        }
    }

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class TheLoadWindowSizeMethod
    {
        private string _tempPath;

        [SetUp]
        public void SetUp()
        {
            _tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(_tempPath);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempPath))
            {
                Directory.Delete(_tempPath, true);
            }
        }

        [Test]
        public void ThrowsArgumentNullExceptionForNullWindow()
        {
            var appDataService = new Mock<IAppDataService>().Object;

            Assert.Throws<ArgumentNullException>(() => appDataService.LoadWindowSize(null, true));
        }

        [Test]
        public void LoadsWindowSizeCorrectly()
        {
            // Arrange
            var mockAppDataService = new Mock<IAppDataService>();
            mockAppDataService.Setup(x => x.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming))
                .Returns(_tempPath);

            var window = new Window();
            // Initialize window with size that would fit on most screens
            window.SetCurrentValue(FrameworkElement.WidthProperty, 800d);
            window.SetCurrentValue(FrameworkElement.HeightProperty, 600d);
            window.SetCurrentValue(Window.WindowStateProperty, WindowState.Normal);

            var directory = Path.Combine(_tempPath, "windows");
            Directory.CreateDirectory(directory);
            var filePath = Path.Combine(directory, $"{window.GetType().FullName}.dat");

            // Use conservative values that should work on most screens
            var left = SystemParameters.VirtualScreenLeft + 100;
            var top = SystemParameters.VirtualScreenTop + 100;

            File.WriteAllText(filePath, $"800|600|Normal|{left}|{top}");

            // Act
            mockAppDataService.Object.LoadWindowSize(window, restoreWindowState: true, restoreWindowPosition: true);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(window.Width, Is.EqualTo(800).Within(0.1));
                Assert.That(window.Height, Is.EqualTo(600).Within(0.1));
                Assert.That(window.WindowState, Is.EqualTo(WindowState.Normal));
                // Only verify that position values are valid numbers, not exact values
                Assert.That(window.Left, Is.Not.NaN);
                Assert.That(window.Top, Is.Not.NaN);
                // Verify position is within virtual screen bounds
                Assert.That(window.Left + window.Width, Is.LessThanOrEqualTo(SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth));
                Assert.That(window.Top + window.Height, Is.LessThanOrEqualTo(SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight));
            });
        }

        [Test]
        public void DoesNotRestoreWindowStateWhenFlagIsFalse()
        {
            // Arrange
            var mockAppDataService = new Mock<IAppDataService>();
            mockAppDataService.Setup(x => x.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming))
                .Returns(_tempPath);

            var window = new Window();
            window.SetCurrentValue(Window.WindowStateProperty, WindowState.Normal);

            var directory = Path.Combine(_tempPath, "windows");
            Directory.CreateDirectory(directory);
            var filePath = Path.Combine(directory, $"{window.GetType().FullName}.dat");
            File.WriteAllText(filePath, "800|600|Maximized|100|200");

            // Act
            mockAppDataService.Object.LoadWindowSize(window, false);

            // Assert
            Assert.That(window.WindowState, Is.EqualTo(WindowState.Normal));
        }

        [Test]
        public void DoesNotRestoreWindowPositionWhenFlagIsFalse()
        {
            // Arrange
            var mockAppDataService = new Mock<IAppDataService>();
            mockAppDataService.Setup(x => x.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming))
                .Returns(_tempPath);

            var window = new Window();
            window.SetCurrentValue(Window.LeftProperty, 0d);
            window.SetCurrentValue(Window.TopProperty, 0d);

            var directory = Path.Combine(_tempPath, "windows");
            Directory.CreateDirectory(directory);
            var filePath = Path.Combine(directory, $"{window.GetType().FullName}.dat");
            File.WriteAllText(filePath, "800|600|Normal|100|200");

            // Act
            mockAppDataService.Object.LoadWindowSize(window, restoreWindowState: true, restoreWindowPosition: false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(window.Left, Is.EqualTo(0).Within(0.1));
                Assert.That(window.Top, Is.EqualTo(0).Within(0.1));
            });
        }

        [Test]
        public void HandlesNonExistentFile()
        {
            // Arrange
            var mockAppDataService = new Mock<IAppDataService>();
            mockAppDataService.Setup(x => x.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming))
                .Returns(_tempPath);

            var window = new Window();
            window.SetCurrentValue(FrameworkElement.WidthProperty, 100d);
            window.SetCurrentValue(FrameworkElement.HeightProperty, 100d);

            // Act & Assert
            Assert.DoesNotThrow(() => mockAppDataService.Object.LoadWindowSize(window));
            Assert.Multiple(() =>
            {
                Assert.That(window.Width, Is.EqualTo(100).Within(0.1));
                Assert.That(window.Height, Is.EqualTo(100).Within(0.1));
            });
        }

        [Test]
        public void HandlesMalformedFile()
        {
            // Arrange
            var mockAppDataService = new Mock<IAppDataService>();
            mockAppDataService.Setup(x => x.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming))
                .Returns(_tempPath);

            var window = new Window();
            window.SetCurrentValue(FrameworkElement.WidthProperty, 100d);
            window.SetCurrentValue(FrameworkElement.HeightProperty, 100d);

            var directory = Path.Combine(_tempPath, "windows");
            Directory.CreateDirectory(directory);
            var filePath = Path.Combine(directory, $"{window.GetType().FullName}.dat");
            File.WriteAllText(filePath, "invalid|data");

            // Act & Assert
            Assert.DoesNotThrow(() => mockAppDataService.Object.LoadWindowSize(window));
            Assert.Multiple(() =>
            {
                Assert.That(window.Width, Is.EqualTo(100).Within(0.1));
                Assert.That(window.Height, Is.EqualTo(100).Within(0.1));
            });
        }
    }
}
