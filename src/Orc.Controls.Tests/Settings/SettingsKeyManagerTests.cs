namespace Orc.Controls.Settings.Tests;

using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
public class SettingsKeyManagerTests
{
    [SetUp]
    public void SetUp()
    {
        _settingsKeyManager = new();
    }

    private SettingsKeyManager _settingsKeyManager;

    [Test]
    public void GetKeys_InitialState_ReturnsEmptyArray()
    {
        // Act
        var keys = _settingsKeyManager.GetKeys();

        // Assert
        Assert.That(keys, Is.Not.Null);
        Assert.That(keys, Is.Empty);
    }

    [Test]
    public void GetDirtyKeys_InitialState_ReturnsEmptyArray()
    {
        // Act
        var dirtyKeys = _settingsKeyManager.GetDirtyKeys();

        // Assert
        Assert.That(dirtyKeys, Is.Not.Null);
        Assert.That(dirtyKeys, Is.Empty);
    }

    [Test]
    public void SetDirty_WithValidKey_UpdatesDirtyState()
    {
        // Arrange
        const string settingsKey = "TestKey";
        var eventRaised = false;

        _settingsKeyManager.DirtyStateChanged += async (sender, args) =>
        {
            eventRaised = true;
            Assert.That(args.SettingsKey, Is.EqualTo(settingsKey));
            Assert.That(args.IsDirty, Is.True);
        };

        // Act
        _settingsKeyManager.SetDirty(settingsKey, true);

        // Assert
        Assert.That(_settingsKeyManager.IsKeyDirty(settingsKey), Is.True);
        Assert.That(_settingsKeyManager.GetDirtyKeys(), Contains.Item(settingsKey));
        Assert.That(eventRaised, Is.True);
    }


    [Test]
    public void IsKeyDirty_WithNullOrEmptyKey_ReturnsFalse()
    {
        // Act & Assert
        Assert.That(_settingsKeyManager.IsKeyDirty(null), Is.False);
        Assert.That(_settingsKeyManager.IsKeyDirty(string.Empty), Is.False);
        Assert.That(_settingsKeyManager.IsKeyDirty(" "), Is.False);
    }

    [Test]
    public async Task LoadAsync_WithValidKey_RaisesEventAndUpdatesKeys()
    {
        // Arrange
        const string settingsKey = "TestKey";
        var loadEventRaised = false;
        var keyLoadedEventRaised = false;

        _settingsKeyManager.LoadRequested += (sender, args) =>
        {
            loadEventRaised = true;
            args.Success = true;
            return Task.CompletedTask;
        };

        _settingsKeyManager.KeyLoaded += (sender, args) =>
        {
            keyLoadedEventRaised = true;
            Assert.That(args.SettingsKey, Is.EqualTo(settingsKey));
        };

        // Act
        await _settingsKeyManager.LoadAsync(settingsKey);

        // Assert
        Assert.That(loadEventRaised, Is.True);
        Assert.That(keyLoadedEventRaised, Is.True);
        Assert.That(_settingsKeyManager.GetKeys(), Contains.Item(settingsKey));
    }

    [Test]
    public async Task LoadAsync_WithNullOrEmptyKey_DoesNotRaiseEvent()
    {
        // Arrange
        var eventRaised = false;
        _settingsKeyManager.LoadRequested += (sender, args) =>
        {
            eventRaised = true;
            return Task.CompletedTask;
        };

        // Act
        await _settingsKeyManager.LoadAsync(null);
        await _settingsKeyManager.LoadAsync(string.Empty);

        // Assert
        Assert.That(eventRaised, Is.False);
    }

    [Test]
    public async Task SaveAsync_WithValidKey_RaisesEventAndClearsDirtyState()
    {
        // Arrange
        const string settingsKey = "TestKey";
        _settingsKeyManager.SetDirty(settingsKey, true);
        var saveEventRaised = false;

        _settingsKeyManager.SaveRequested += (sender, args) =>
        {
            saveEventRaised = true;
            args.Success = true;
            return Task.CompletedTask;
        };

        // Act
        await _settingsKeyManager.SaveAsync(settingsKey);

        // Assert
        Assert.That(saveEventRaised, Is.True);
        Assert.That(_settingsKeyManager.IsKeyDirty(settingsKey), Is.False);
    }
}

// Test helper classes
