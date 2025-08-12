namespace Orc.Controls.Settings.Tests;

using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
public class JsonSettingsSerializerTests
{
    [SetUp]
    public void SetUp()
    {
        _serializer = new();
    }

    private JsonSettingsSerializer<TestSettings> _serializer;

    [Test]
    public async Task SerializeAsync_WithValidObject_ReturnsJsonString()
    {
        // Arrange
        var settings = new TestSettings
        {
            Value = "Test Value",
            Number = 42,
            IsEnabled = true
        };

        // Act
        var json = await _serializer.SerializeAsync(settings);

        // Assert
        Assert.That(json, Is.Not.Null);
        Assert.That(json, Is.Not.Empty);
        Assert.That(json, Contains.Substring("Test Value"));
        Assert.That(json, Contains.Substring("42"));
        Assert.That(json, Contains.Substring("true"));
    }

    [Test]
    public async Task DeserializeAsync_WithValidJson_ReturnsObject()
    {
        // Arrange
        var originalSettings = new TestSettings
        {
            Value = "Test Value",
            Number = 42,
            IsEnabled = true
        };
        var json = await _serializer.SerializeAsync(originalSettings);

        // Act
        var deserializedSettings = await _serializer.DeserializeAsync(json);

        // Assert
        Assert.That(deserializedSettings, Is.Not.Null);
        Assert.That(deserializedSettings.Value, Is.EqualTo("Test Value"));
        Assert.That(deserializedSettings.Number, Is.EqualTo(42));
        Assert.That(deserializedSettings.IsEnabled, Is.True);
    }

    [Test]
    public async Task DeserializeAsync_WithInvalidJson_ReturnsNull()
    {
        // Arrange
        const string invalidJson = "{ invalid json }";

        // Act
        var result = await _serializer.DeserializeAsync(invalidJson);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task DeserializeAsync_WithEmptyString_ReturnsNull()
    {
        // Act
        var result = await _serializer.DeserializeAsync(string.Empty);

        // Assert
        Assert.That(result, Is.Null);
    }
}