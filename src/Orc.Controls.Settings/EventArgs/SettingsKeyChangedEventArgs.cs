namespace Orc.Controls.Settings;

using System;
using System.Windows;

/// <summary>
/// Event arguments for SettingsKey changes
/// </summary>
public class SettingsKeyChangedEventArgs : EventArgs
{
    public DependencyObject Element { get; }
    public object? OldValue { get; }
    public object? NewValue { get; }

    public SettingsKeyChangedEventArgs(DependencyObject element, object? oldValue, object? newValue)
    {
        Element = element;
        OldValue = oldValue;
        NewValue = newValue;
    }
}
