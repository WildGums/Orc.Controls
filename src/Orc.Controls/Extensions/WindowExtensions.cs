namespace Orc.Controls;

using System;
using System.Windows;

public static class WindowExtensions
{
    private const string SizeSeparator = "|";

    public static void CenterWindowToParent(this Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        var owner = window.Owner;
        if (owner is not null)
        {
            window.CenterWindowToSize(new Rect(owner.Left, owner.Top, owner.ActualWidth, owner.ActualHeight));
            return;
        }

        var parentWindow = Catel.Windows.DependencyObjectExtensions.FindLogicalOrVisualAncestorByType<Window>(window);
        if (parentWindow is not null)
        {
            window.CenterWindowToSize(new Rect(parentWindow.Left, parentWindow.Top, parentWindow.ActualWidth, parentWindow.ActualHeight));
        }
    }

    public static void CenterWindowToSize(this Window window, Rect parentRect)
    {
        ArgumentNullException.ThrowIfNull(window);

        var windowWidth = window.Width;
        var windowHeight = window.Height;

        window.SetCurrentValue(Window.LeftProperty, parentRect.Left + (parentRect.Width / 2) - (windowWidth / 2));
        window.SetCurrentValue(Window.TopProperty, parentRect.Top + (parentRect.Height / 2) - (windowHeight / 2));
    }

    internal static Size GetSize(this Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        return new Size(window.ActualWidth, window.ActualHeight);
    }
}
