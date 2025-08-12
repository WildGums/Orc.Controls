namespace Orc.Controls.Settings;

using System;
using System.Windows;

/// <summary>
/// Interface for control settings adapters
/// </summary>
public interface IControlSettingsAdapter<TControl, TSettings> /*: IAttachable<TControl>*/
    where TControl : FrameworkElement
    where TSettings : class
{
    /// <summary>
    /// Event fired when control settings change
    /// </summary>
    event EventHandler? SettingsChanged;
    
    /// <summary>
    /// Gets the current settings from the control
    /// </summary>
    TSettings? GetCurrentSettings();
    
    /// <summary>
    /// Applies settings to the control
    /// </summary>
    void ApplySettings(TSettings settings);
    
    /// <summary>
    /// Determines if two settings objects are equal
    /// </summary>
    bool AreSettingsEqual(TSettings? current, TSettings? saved);

    void Attach(TControl target);
    void Detach();
}
