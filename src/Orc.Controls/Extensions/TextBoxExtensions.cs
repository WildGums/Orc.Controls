// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public static class TextBoxExtensions
    {
        internal static void SubscribeToOnRightBoundReachedEvent(this TextBox textBox, EventHandler handler)
        {
            var numericTextBox = textBox as NumericTextBox;
            if (numericTextBox != null)
            {
                numericTextBox.RightBoundReached += handler;
            }

            var listTextBox = textBox as ListTextBox;
            if (listTextBox != null)
            {
                listTextBox.RightBoundReached += handler;
            }
        }

        internal static void SubscribeToOnLeftBoundReachedEvent(this TextBox textBox, EventHandler handler)
        {
            var numericTextBox = textBox as NumericTextBox;
            if (numericTextBox != null)
            {
                numericTextBox.LeftBoundReached += handler;
            }

            var listTextBox = textBox as ListTextBox;
            if (listTextBox != null)
            {
                listTextBox.LeftBoundReached += handler;
            }
        }

        internal static void UnsubscribeFromOnRightBoundReachedEvent(this TextBox textBox, EventHandler handler)
        {
            var numericTextBox = textBox as NumericTextBox;
            if (numericTextBox != null)
            {
                numericTextBox.RightBoundReached -= handler;
            }

            var listTextBox = textBox as ListTextBox;
            if (listTextBox != null)
            {
                listTextBox.RightBoundReached -= handler;
            }
        }

        internal static void UnsubscribeFromOnLeftBoundReachedEvent(this TextBox textBox, EventHandler handler)
        {
            var numericTextBox = textBox as NumericTextBox;
            if (numericTextBox != null)
            {
                numericTextBox.LeftBoundReached -= handler;
            }

            var listTextBox = textBox as ListTextBox;
            if (listTextBox != null)
            {
                listTextBox.LeftBoundReached -= handler;
            }
        }

        internal static void UpdateValue(this TextBox textBox, object value)
        {
            if (textBox is NumericTextBox)
            {
                ((NumericTextBox)textBox).SetCurrentValue(NumericTextBox.ValueProperty, value == null ? (double?)null : Convert.ToDouble(value));
            }
            else if (textBox is ListTextBox)
            {
                ((ListTextBox)textBox).SetCurrentValue(ListTextBox.ValueProperty, value == null ? null : Convert.ToString(value));
            }
            else
            {
                textBox.SetCurrentValue(TextBox.TextProperty, value == null ? null : Convert.ToString(value));
            }
        }
    }
}
