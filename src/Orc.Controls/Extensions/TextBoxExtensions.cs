// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows.Controls;

    public static class TextBoxExtensions
    {
        internal static void SubscribeToOnRightBoundReachedEvent(this TextBox textBox, EventHandler handler)
        {
            switch (textBox)
            {
                case NumericTextBox numericTextBox:
                    numericTextBox.RightBoundReached += handler;
                    break;

                case ListTextBox listTextBox:
                    listTextBox.RightBoundReached += handler;
                    break;
            }
        }

        internal static void SubscribeToOnLeftBoundReachedEvent(this TextBox textBox, EventHandler handler)
        {
            switch (textBox)
            {
                case NumericTextBox numericTextBox:
                    numericTextBox.LeftBoundReached += handler;
                    break;

                case ListTextBox listTextBox:
                    listTextBox.LeftBoundReached += handler;
                    break;
            }
        }

        internal static void UnsubscribeFromOnRightBoundReachedEvent(this TextBox textBox, EventHandler handler)
        {
            switch (textBox)
            {
                case NumericTextBox numericTextBox:
                    numericTextBox.RightBoundReached -= handler;
                    break;

                case ListTextBox listTextBox:
                    listTextBox.RightBoundReached -= handler;
                    break;
            }
        }

        internal static void UnsubscribeFromOnLeftBoundReachedEvent(this TextBox textBox, EventHandler handler)
        {
            switch (textBox)
            {
                case NumericTextBox numericTextBox:
                    numericTextBox.LeftBoundReached -= handler;
                    break;

                case ListTextBox listTextBox:
                    listTextBox.LeftBoundReached -= handler;
                    break;
            }
        }

        internal static void UpdateValue(this TextBox textBox, object value)
        {
            switch (textBox)
            {
                case NumericTextBox _:
                    ((NumericTextBox)textBox).SetCurrentValue(NumericTextBox.ValueProperty, value == null ? (double?)null : Convert.ToDouble(value));
                    break;

                case ListTextBox _:
                    ((ListTextBox)textBox).SetCurrentValue(ListTextBox.ValueProperty, value == null ? null : Convert.ToString(value));
                    break;

                default:
                    textBox.SetCurrentValue(TextBox.TextProperty, value == null ? null : Convert.ToString(value));
                    break;
            }
        }
    }
}
