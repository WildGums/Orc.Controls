namespace Orc.Controls.Settings;

using System;
using System.Windows;

public static class SettingsManagement
{
    /// <summary>
    /// SettingsKey attached property
    /// </summary>
    public static readonly DependencyProperty SettingsKeyProperty =
        DependencyProperty.RegisterAttached(
            "SettingsKey",
            typeof(object),
            typeof(SettingsManagement),
            new(null, OnSettingsKeyChanged));

    /// <summary>
    /// Event fired when SettingsKey property changes on any element
    /// </summary>
    public static event EventHandler<SettingsKeyChangedEventArgs>? SettingsKeyChanged;

    /// <summary>
    /// Property changed callback for SettingsKey
    /// </summary>
    /// <param name="d">The dependency object</param>
    /// <param name="e">Event arguments</param>
    private static void OnSettingsKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var oldValue = e.OldValue;
        var newValue = e.NewValue;

        SettingsKeyChanged?.Invoke(d, new(d, oldValue, newValue));
    }

    /// <summary>
    /// Gets the SettingsKey value for the specified element
    /// </summary>
    /// <param name="element">The element to get the value from</param>
    /// <returns>The SettingsKey value</returns>
    public static object? GetSettingsKey(DependencyObject element)
    {
        if (element is null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        return (object?)element.GetValue(SettingsKeyProperty);
    }

    /// <summary>
    /// Sets the SettingsKey value for the specified element
    /// </summary>
    /// <param name="element">The element to set the value on</param>
    /// <param name="value">The SettingsKey value to set</param>
    public static void SetSettingsKey(DependencyObject element, object? value)
    {
        if (element is null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        element.SetValue(SettingsKeyProperty, value);
    }

    // Alternative approach using ItemsControl.ItemsControlFromItemContainer
    //private static DependencyObject? FindTabItemParentAlternative(FrameworkElement element)
    //{
    //    // Walk up the logical tree to find a TabItem
    //    var current = element as DependencyObject;
    //    while (current is not null)
    //    {
    //        if (current is TabItem tabItem)
    //        {
    //            // Use ItemsControl.ItemsControlFromItemContainer to get the TabControl
    //            var tabControl = ItemsControl.ItemsControlFromItemContainer(tabItem);
    //            return tabControl;
    //        }

    //        current = current.GetLogicalParent();
    //    }

    //    return null;
    //}
}
