namespace Orc.Controls.Settings;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

public class ControlRequestedEventArgs : EventArgs
{
    private const string SearchPattern = @"\{[^}]*\}[^/]*";

    private readonly List<ISettingsElement> _elements = [];

    public ControlRequestedEventArgs(string settingsKey) => SettingsKey = settingsKey;

    public string SettingsKey { get; }
    public IReadOnlyCollection<ISettingsElement> Elements => _elements;

    public async Task VisitControlAsync(FrameworkElement? element, ISettingsElement settingsElement)
    {
        if (element is null)
        {
            return;
        }

        var currentElementSettingsKey = settingsElement.SettingsKey;
        if (string.IsNullOrWhiteSpace(currentElementSettingsKey))
        {
            return;
        }

        var settingsKey = SettingsKey;
        if (Equals(settingsElement.SettingsKey, settingsKey))
        {
            _elements.Add(settingsElement);
            return;
        }

        var constantKeyPart = Regex.Replace(settingsKey, SearchPattern, string.Empty);
        var elementConstantKeyPart = Regex.Replace(currentElementSettingsKey, SearchPattern, string.Empty);
        if (!Equals(constantKeyPart, elementConstantKeyPart))
        {
            return;
        }

        var settings = await settingsElement.GetSettingsAsync(settingsKey);
        if (settings is not null)
        {
            _elements.Add(settingsElement);
        }
    }
}
