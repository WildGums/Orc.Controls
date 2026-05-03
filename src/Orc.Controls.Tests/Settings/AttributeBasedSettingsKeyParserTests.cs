namespace Orc.Controls.Settings.Tests;

using NUnit.Framework;

[TestFixture]
public class AttributeBasedSettingsKeyParserTests
{
    [SetUp]
    public void SetUp()
    {
        _parser = new();
    }

    private AttributeBasedSettingsKeyParser<TestKeyInfo> _parser;

    [Test]
    public void ToString_WithValidObject_ReturnsFormattedString()
    {
        // Arrange
        var keyInfo = new TestKeyInfo
        {
            WindowName = "MainWindow",
            TabIndex = 1,
            ViewType = TestViewType.Grid
        };

        // Act
        var result = _parser.ToString(keyInfo);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Contains.Substring("[Window = MainWindow]"));
        Assert.That(result, Contains.Substring("[Tab = 1]"));
        Assert.That(result, Contains.Substring("[View = Grid]"));
    }

    [Test]
    public void Parse_WithValidString_ReturnsObject()
    {
        // Arrange
        const string settingsKey = "[Window = MainWindow][Tab = 1][View = Grid]";

        // Act
        var result = _parser.Parse(settingsKey);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.WindowName, Is.EqualTo("MainWindow"));
        Assert.That(result.TabIndex, Is.EqualTo(1));
        Assert.That(result.ViewType, Is.EqualTo(TestViewType.Grid));
    }

    [Test]
    public void Parse_WithPartialString_ReturnsObjectWithDefaults()
    {
        // Arrange
        const string settingsKey = "[Window = TestWindow]";

        // Act
        var result = _parser.Parse(settingsKey);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.WindowName, Is.EqualTo("TestWindow"));
        Assert.That(result.TabIndex, Is.EqualTo(0)); // Default value
        Assert.That(result.ViewType, Is.EqualTo(TestViewType.List)); // Default enum value
    }

    [Test]
    public void ToString_WithNullObject_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.That(() => _parser.ToString(null), Throws.ArgumentNullException);
    }
}
