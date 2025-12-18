namespace Orc.Controls.Settings.Tests;

using NUnit.Framework;

[TestFixture]
public class SettingsHelperTests
{
    [Test]
    public void GetCombinedSettingsKey_WithNullElement_ReturnsEmptyString()
    {
        // Act
        var result = SettingsHelper.GetCombinedSettingsKey(null);

        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    // Note: More comprehensive tests for SettingsHelper would require setting up 
    // WPF visual tree elements, which is complex in unit tests and might be better 
    // suited for integration tests
}