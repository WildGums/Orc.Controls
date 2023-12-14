namespace Orc.Controls;

using System;
using System.Windows.Controls;

public static class TextBoxExtensions
{
    internal static void SubscribeToOnRightBoundReachedEvent(this TextBox textBox, EventHandler<EventArgs> handler)
    {
        ArgumentNullException.ThrowIfNull(textBox);
        ArgumentNullException.ThrowIfNull(handler);

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

    internal static void SubscribeToOnLeftBoundReachedEvent(this TextBox textBox, EventHandler<EventArgs> handler)
    {
        ArgumentNullException.ThrowIfNull(textBox);
        ArgumentNullException.ThrowIfNull(handler);

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

    internal static void UnsubscribeFromOnRightBoundReachedEvent(this TextBox textBox, EventHandler<EventArgs> handler)
    {
        ArgumentNullException.ThrowIfNull(textBox);
        ArgumentNullException.ThrowIfNull(handler);

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

    internal static void UnsubscribeFromOnLeftBoundReachedEvent(this TextBox textBox, EventHandler<EventArgs> handler)
    {
        ArgumentNullException.ThrowIfNull(textBox);
        ArgumentNullException.ThrowIfNull(handler);

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

    internal static void UpdateValue(this TextBox textBox, object? value)
    {
        ArgumentNullException.ThrowIfNull(textBox);

        switch (textBox)
        {
            case NumericTextBox box:
                box.SetCurrentValue(NumericTextBox.ValueProperty, value is null ? null : Convert.ToDouble(value));
                break;

            case ListTextBox box:
                box.SetCurrentValue(ListTextBox.ValueProperty, value is null ? null : Convert.ToString(value));
                break;

            default:
                textBox.SetCurrentValue(TextBox.TextProperty, value is null ? null : Convert.ToString(value));
                break;
        }
    }
}
