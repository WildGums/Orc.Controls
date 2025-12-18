namespace Orc.Controls.Settings.Tests;

using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
public class FileSystemLocationProviderTests
{
    [SetUp]
    public void SetUp()
    {
        _locationProvider = new(_basePath);
    }

    private FileSystemLocationProvider _locationProvider;
    private readonly string _basePath = @"C:\Settings";

    [Test]
    public async Task GetLocationAsync_WithSimpleKey_ReturnsCorrectPath()
    {
        // Arrange
        const string settingsKey = "TestKey";

        // Act
        var location = await _locationProvider.GetLocationAsync(settingsKey);

        // Assert
        Assert.That(location, Is.EqualTo(@"C:\Settings\TestKey.json"));
    }

    [Test]
    public async Task GetLocationAsync_WithHierarchicalKey_ReturnsCorrectPath()
    {
        // Arrange
        const string settingsKey = "Window/Tab/Control";

        // Act
        var location = await _locationProvider.GetLocationAsync(settingsKey);

        // Assert
        Assert.That(location, Is.EqualTo(@"C:\Settings\Window\Tab\Control.json"));
    }

    [Test]
    public async Task GetLocationAsync_WithInvalidCharacters_SanitizesPath()
    {
        // Arrange
        const string settingsKey = "Test:Key*With?Invalid<Chars>";

        // Act
        var location = await _locationProvider.GetLocationAsync(settingsKey);

        // Assert
        Assert.That(location, Contains.Substring("Test_Key_With_Invalid_Chars_"));
        Assert.That(location, Does.Not.Contain("*"));
        Assert.That(location, Does.Not.Contain("?"));
        Assert.That(location, Does.Not.Contain("<"));
        Assert.That(location, Does.Not.Contain(">"));
    }

    [Test]
    public async Task GetLocationAsync_WithEmptyKey_ReturnsUnknownPath()
    {
        // Act
        var location = await _locationProvider.GetLocationAsync(string.Empty);

        // Assert
        Assert.That(location, Is.EqualTo(@"C:\Settings\Unknown.json"));
    }

    [Test]
    public async Task GetLocationAsync_WithNullKey_ReturnsUnknownPath()
    {
        // Act
        var location = await _locationProvider.GetLocationAsync(null);

        // Assert
        Assert.That(location, Is.EqualTo(@"C:\Settings\Unknown.json"));
    }
}
