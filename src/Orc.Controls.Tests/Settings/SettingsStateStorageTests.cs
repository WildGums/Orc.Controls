namespace Orc.Controls.Settings.Tests;

using NUnit.Framework;

[TestFixture]
public class SettingsStateStorageTests
{
    [SetUp]
    public void SetUp()
    {
        _stateStorage = new();
    }

    private SettingsStateStorage _stateStorage;

    [Test]
    public void StoreCurrentSettings_WithValidData_StoresAndRaisesEvent()
    {
        // Arrange
        const string settingsKey = "TestKey";
        var testSettings = new TestSettings
        {
            Value = "Test"
        };
        var eventRaised = false;

        _stateStorage.SettingsStored += (sender, args) =>
        {
            eventRaised = true;
            Assert.That(args.SettingsKey, Is.EqualTo(settingsKey));
        };

        // Act
        _stateStorage.StoreCurrentSettings(settingsKey, testSettings);

        // Assert
        Assert.That(eventRaised, Is.True);
        var retrieved = _stateStorage.GetStoredSettings<TestSettings>(settingsKey);
        Assert.That(retrieved, Is.Not.Null);
        Assert.That(retrieved.Value, Is.EqualTo("Test"));
    }

    [Test]
    public void GetStoredSettings_WithNonExistentKey_ReturnsNull()
    {
        // Act
        var result = _stateStorage.GetStoredSettings<TestSettings>("NonExistentKey");

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetStoredSettings_WithWrongType_ReturnsNull()
    {
        // Arrange
        const string settingsKey = "TestKey";
        _stateStorage.StoreCurrentSettings(settingsKey, new TestSettings
        {
            Value = "Test"
        });

        // Act
        var result = _stateStorage.GetStoredSettings<AnotherTestSettings>(settingsKey);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void RemoveStoredSettings_WithExistingKey_RemovesAndRaisesEvent()
    {
        // Arrange
        const string settingsKey = "TestKey";
        var testSettings = new TestSettings
        {
            Value = "Test"
        };
        _stateStorage.StoreCurrentSettings(settingsKey, testSettings);
        var eventRaised = false;

        _stateStorage.SettingsRemoved += (sender, args) =>
        {
            eventRaised = true;
            Assert.That(args.SettingsKey, Is.EqualTo(settingsKey));
        };

        // Act
        _stateStorage.RemoveStoredSettings(settingsKey);

        // Assert
        Assert.That(eventRaised, Is.True);
        var retrieved = _stateStorage.GetStoredSettings<TestSettings>(settingsKey);
        Assert.That(retrieved, Is.Null);
    }

    [Test]
    public void Rename_WithExistingKey_MovesSettings()
    {
        // Arrange
        const string oldKey = "OldKey";
        const string newKey = "NewKey";
        var testSettings = new TestSettings
        {
            Value = "Test"
        };
        _stateStorage.StoreCurrentSettings(oldKey, testSettings);

        // Act
        _stateStorage.Rename(oldKey, newKey);

        // Assert
        var oldRetrieved = _stateStorage.GetStoredSettings<TestSettings>(oldKey);
        var newRetrieved = _stateStorage.GetStoredSettings<TestSettings>(newKey);

        Assert.That(oldRetrieved, Is.Null);
        Assert.That(newRetrieved, Is.Not.Null);
        Assert.That(newRetrieved.Value, Is.EqualTo("Test"));
    }
}