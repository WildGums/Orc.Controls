﻿namespace Orc.Controls;

using System;
using System.Windows;
using System.Windows.Controls;

public class ComboboxEditableControlBehavior : EditableControlBehaviorBase<ComboBox>
{
    protected override void OnAssociatedObjectLoaded()
    {
        base.OnAssociatedObjectLoaded();

        var combobox = AssociatedObject;
        combobox.GotFocus += ComboboxOnGotFocus;
        combobox.LostFocus += OnComboboxLostFocus;
        combobox.SelectionChanged += OnComboboxSelectionChanged;
        combobox.DropDownClosed += OnComboboxDropDownClosed;
    }

    protected override void OnAssociatedObjectUnloaded()
    {
        base.OnAssociatedObjectUnloaded();

        var combobox = AssociatedObject;
        combobox.GotFocus -= ComboboxOnGotFocus;
        combobox.LostFocus -= OnComboboxLostFocus;
        combobox.SelectionChanged -= OnComboboxSelectionChanged;
        combobox.DropDownClosed -= OnComboboxDropDownClosed;
    }

    private void OnComboboxDropDownClosed(object? _, EventArgs e)
    {
        SetCurrentValue(IsInEditModeProperty, false);

        RaiseEditEnded();
    }

    private void ComboboxOnGotFocus(object? _, RoutedEventArgs e)
    {
        SetCurrentValue(IsInEditModeProperty, true);

        RaiseEditStarted();
    }

    private void OnComboboxLostFocus(object? _, RoutedEventArgs e)
    {
        var combobox = AssociatedObject;
        if (combobox.IsDropDownOpen)
        {
            return;
        }

        if (combobox.IsKeyboardFocused)
        {
            return;
        }

        if (combobox.IsKeyboardFocusWithin)
        {
            return;
        }

        SetCurrentValue(IsInEditModeProperty, false);

        RaiseEditEnded();
    }

    private void OnComboboxSelectionChanged(object? _, SelectionChangedEventArgs e)
    {
        var combobox = AssociatedObject;
        if (combobox.IsDropDownOpen)
        {
            return;
        }

        if (combobox.IsKeyboardFocused)
        {
            return;
        }

        if (combobox.IsKeyboardFocusWithin)
        {
            return;
        }

        SetCurrentValue(IsInEditModeProperty, false);

        RaiseEditEnded();
    }
}
