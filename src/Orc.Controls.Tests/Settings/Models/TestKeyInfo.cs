namespace Orc.Controls.Settings.Tests;

public class TestKeyInfo
{
    [SettingsKeyProperty("Window")] public string WindowName { get; set; } = string.Empty;

    [SettingsKeyProperty("Tab")] public int TabIndex { get; set; }

    [SettingsKeyProperty("View")] public TestViewType ViewType { get; set; } = TestViewType.List;
}