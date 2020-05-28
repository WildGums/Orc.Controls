// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComboboxEditableControlBehavior.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class ComboboxEditableControlBehavior : EditableControlBehaviorBase<ComboBox>
    {
        #region Methods
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

        private void OnComboboxDropDownClosed(object sender, EventArgs e)
        {
            SetCurrentValue(IsInEditModeProperty, false);

            RaiseEditEnded();
        }

        private void ComboboxOnGotFocus(object sender, RoutedEventArgs e)
        {
            SetCurrentValue(IsInEditModeProperty, true);

            RaiseEditStarted();
        }

        private void OnComboboxLostFocus(object sender, RoutedEventArgs e)
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

        private void OnComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
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
        #endregion
    }
}
