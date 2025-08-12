namespace Orc.Controls.Settings.Tests;

using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

[TestFixture]
public class SettingsStorageTests
{
    [SetUp]
    public void SetUp()
    {
        _mockDataStorage = new();
        _mockSerializer = new();
        _settingsStorage = new(_mockDataStorage.Object, _mockSerializer.Object);
    }

    private Mock<ISettingsDataStorage> _mockDataStorage;
    private Mock<ISettingsSerializer<TestSettings>> _mockSerializer;
    private SettingsStorage<TestSettings> _settingsStorage;

    [Test]
    public async Task LoadAsync_WithExistingData_ReturnsDeserializedObject()
    {
        // Arrange
        const string settingsKey = "TestKey";
        const string jsonData = "{ \"value\": \"test\" }";
        var expectedSettings = new TestSettings
        {
            Value = "test"
        };

        _mockDataStorage.Setup(x => x.LoadStringAsync(settingsKey))
            .ReturnsAsync(jsonData);
        _mockSerializer.Setup(x => x.DeserializeAsync(jsonData))
            .ReturnsAsync(expectedSettings);

        // Act
        var result = await _settingsStorage.LoadAsync(settingsKey);

        // Assert
        Assert.That(result, Is.EqualTo(expectedSettings));
        _mockDataStorage.Verify(x => x.LoadStringAsync(settingsKey), Times.Once);
        _mockSerializer.Verify(x => x.DeserializeAsync(jsonData), Times.Once);
    }

    [Test]
    public async Task LoadAsync_WithNoData_ReturnsNull()
    {
        // Arrange
        const string settingsKey = "TestKey";
        _mockDataStorage.Setup(x => x.LoadStringAsync(settingsKey))
            .ReturnsAsync((string)null);

        // Act
        var result = await _settingsStorage.LoadAsync(settingsKey);

        // Assert
        Assert.That(result, Is.Null);
        _mockSerializer.Verify(x => x.DeserializeAsync(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task SaveAsync_WithValidSettings_SerializesAndSaves()
    {
        // Arrange
        const string settingsKey = "TestKey";
        const string jsonData = "{ \"value\": \"test\" }";
        var settings = new TestSettings
        {
            Value = "test"
        };

        _mockSerializer.Setup(x => x.SerializeAsync(settings))
            .ReturnsAsync(jsonData);

        // Act
        await _settingsStorage.SaveAsync(settingsKey, settings);

        // Assert
        _mockSerializer.Verify(x => x.SerializeAsync(settings), Times.Once);
        _mockDataStorage.Verify(x => x.SaveStringAsync(settingsKey, jsonData), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_WithValidKey_CallsDataStorage()
    {
        // Arrange
        const string settingsKey = "TestKey";

        // Act
        await _settingsStorage.DeleteAsync(settingsKey);

        // Assert
        _mockDataStorage.Verify(x => x.DeleteAsync(settingsKey), Times.Once);
    }

    [Test]
    public async Task ExistsAsync_WithValidKey_ReturnsDataStorageResult()
    {
        // Arrange
        const string settingsKey = "TestKey";
        _mockDataStorage.Setup(x => x.ExistsAsync(settingsKey))
            .ReturnsAsync(true);

        // Act
        var result = await _settingsStorage.ExistsAsync(settingsKey);

        // Assert
        Assert.That(result, Is.True);
        _mockDataStorage.Verify(x => x.ExistsAsync(settingsKey), Times.Once);
    }
}