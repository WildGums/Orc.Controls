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

            var window = new Window
            {
                Width = 800,
                Height = 600,
                Left = 100,
                Top = 200,
                WindowState = WindowState.Normal
            };

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
        }

        [Test]
        public void SavesWindowSizeWithTagCorrectly()
        {
            // Arrange
            var mockAppDataService = new Mock<IAppDataService>();
            var appData = Path.Combine(Path.GetTempPath(), "TestAppData");
            mockAppDataService.Setup(x => x.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming))
                .Returns(appData);

            var window = new Window
            {
                Width = 800,
                Height = 600
            };
            var tag = "TestTag";

            // Act
            mockAppDataService.Object.SaveWindowSize(window, tag);

            // Assert
            var expectedPath = Path.Combine(appData, "windows", $"{window.GetType().FullName}_testtag.dat");
            Assert.That(File.Exists(expectedPath));
        }
    }

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class TheLoadWindowSizeMethod
    {
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
            var appData = Path.Combine(Path.GetTempPath(), "TestAppData");
            mockAppDataService.Setup(x => x.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming))
                .Returns(appData);

            var window = new Window();
            var directory = Path.Combine(appData, "windows");
            Directory.CreateDirectory(directory);
            var filePath = Path.Combine(directory, $"{window.GetType().FullName}.dat");
            File.WriteAllText(filePath, "800|600|Normal|100|200");

            // Act
            mockAppDataService.Object.LoadWindowSize(window, restoreWindowState:true);

            // Assert
            Assert.That(window.Width, Is.EqualTo(800));
            Assert.That(window.Height, Is.EqualTo(600));
            Assert.That(window.WindowState, Is.EqualTo(WindowState.Normal));
            Assert.That(window.Left, Is.EqualTo(100));
            Assert.That(window.Top, Is.EqualTo(200));
        }

        [Test]
        public void DoesNotRestoreWindowStateWhenFlagIsFalse()
        {
            // Arrange
            var mockAppDataService = new Mock<IAppDataService>();
            var appData = Path.Combine(Path.GetTempPath(), "TestAppData");
            mockAppDataService.Setup(x => x.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming))
                .Returns(appData);

            var window = new Window
            {
                WindowState = WindowState.Normal
            };
            var directory = Path.Combine(appData, "windows");
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
            var appData = Path.Combine(Path.GetTempPath(), "TestAppData");
            mockAppDataService.Setup(x => x.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming))
                .Returns(appData);

            var window = new Window
            {
                Left = 0,
                Top = 0
            };
            var directory = Path.Combine(appData, "windows");
            Directory.CreateDirectory(directory);
            var filePath = Path.Combine(directory, $"{window.GetType().FullName}.dat");
            File.WriteAllText(filePath, "800|600|Normal|100|200");

            // Act
            mockAppDataService.Object.LoadWindowSize(window,restoreWindowState: true, restoreWindowPosition: false);

            // Assert
            Assert.That(window.Left, Is.EqualTo(0));
            Assert.That(window.Top, Is.EqualTo(0));
        }

        [Test]
        public void HandlesNonExistentFile()
        {
            // Arrange
            var mockAppDataService = new Mock<IAppDataService>();
            var appData = Path.Combine(Path.GetTempPath(), "TestAppData");
            mockAppDataService.Setup(x => x.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming))
                .Returns(appData);

            var window = new Window
            {
                Width = 100,
                Height = 100
            };

            // Act & Assert
            Assert.DoesNotThrow(() => mockAppDataService.Object.LoadWindowSize(window));
            Assert.That(window.Width, Is.EqualTo(100));
            Assert.That(window.Height, Is.EqualTo(100));
        }

        [Test]
        public void HandlesMalformedFile()
        {
            // Arrange
            var mockAppDataService = new Mock<IAppDataService>();
            var appData = Path.Combine(Path.GetTempPath(), "TestAppData");
            mockAppDataService.Setup(x => x.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming))
                .Returns(appData);

            var window = new Window
            {
                Width = 100,
                Height = 100
            };
            var directory = Path.Combine(appData, "windows");
            Directory.CreateDirectory(directory);
            var filePath = Path.Combine(directory, $"{window.GetType().FullName}.dat");
            File.WriteAllText(filePath, "invalid|data");

            // Act & Assert
            Assert.DoesNotThrow(() => mockAppDataService.Object.LoadWindowSize(window));
            Assert.That(window.Width, Is.EqualTo(100));
            Assert.That(window.Height, Is.EqualTo(100));
        }
    }
}
