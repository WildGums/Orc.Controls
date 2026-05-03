namespace Orc.Controls;

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using Catel;
using Catel.IO;
using Catel.Logging;
using Catel.Services;
using Microsoft.Extensions.Logging;
using Path = System.IO.Path;

public static class IAppDataServiceExtensions
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(IAppDataServiceExtensions));

    private const string SizeSeparator = "|";

    public static void SaveWindowSize(this IAppDataService appDataService, Window window, string? tag = null)
    {
        ArgumentNullException.ThrowIfNull(window);

        var windowName = window.GetType().Name;

        Logger.LogDebug($"Saving window size for '{windowName}'");

        var storageFile = GetWindowStorageFile(appDataService, window, tag);

        try
        {
            var culture = CultureInfo.InvariantCulture;

            var width = ObjectToStringHelper.ToString(window.Width, culture);
            var height = ObjectToStringHelper.ToString(window.Height, culture);
            var left = ObjectToStringHelper.ToString(window.Left, culture);
            var top = ObjectToStringHelper.ToString(window.Top, culture);
            var state = ObjectToStringHelper.ToString(window.WindowState, culture);

            var contents = $"{width}{SizeSeparator}{height}{SizeSeparator}{state}{SizeSeparator}{left}{SizeSeparator}{top}";

            File.WriteAllText(storageFile, contents);
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, $"Failed to save window size to file '{storageFile}'");
        }
    }

    public static void LoadWindowSize(this IAppDataService appDataService, Window window, bool restoreWindowState)
    {
        LoadWindowSize(appDataService, window, null, restoreWindowState);
    }

    public static void LoadWindowSize(this IAppDataService appDataService, Window window, string? tag = null, bool restoreWindowState = false, bool restoreWindowPosition = true)
    {
        ArgumentNullException.ThrowIfNull(window);

        var windowName = window.GetType().Name;

        Logger.LogDebug($"Loading window size for '{windowName}'");

        var storageFile = GetWindowStorageFile(appDataService, window, tag);
        if (!File.Exists(storageFile))
        {
            Logger.LogDebug($"Window size file '{storageFile}' does not exist, cannot restore window size");
            return;
        }

        try
        {
            var sizeText = File.ReadAllText(storageFile);
            if (string.IsNullOrWhiteSpace(sizeText))
            {
                Logger.LogWarning($"Size text for window is empty, cannot restore window size");
                return;
            }

            var splitted = sizeText.Split(new[] { SizeSeparator }, StringSplitOptions.RemoveEmptyEntries);
            if (splitted.Length < 2)
            {
                Logger.LogWarning($"Size text for window could not be splitted correctly, cannot restore window size");
                return;
            }

            var culture = CultureInfo.InvariantCulture;

            var width = StringToObjectHelper.ToDouble(splitted[0], culture);
            var height = StringToObjectHelper.ToDouble(splitted[1], culture);

            Logger.LogDebug($"Setting window size for '{windowName}' to '{width} x {height}'");

            // Always set window to normal state first when dealing with size/position
            if (window.WindowState != WindowState.Normal)
            {
                window.SetCurrentValue(Window.WindowStateProperty, WindowState.Normal);
            }

            window.SetCurrentValue(FrameworkElement.WidthProperty, width);
            window.SetCurrentValue(FrameworkElement.HeightProperty, height);

            var savedWindowState = WindowState.Normal;
            if (splitted.Length > 2)
            {
                savedWindowState = Enum<WindowState>.Parse(splitted[2]);
            }

            // Only try to restore position if the flag is true
            if (restoreWindowPosition && splitted.Length > 4)
            {
                var left = StringToObjectHelper.ToDouble(splitted[3], culture);
                var top = StringToObjectHelper.ToDouble(splitted[4], culture);

                Logger.LogDebug($"Restoring window position for '{windowName}' to '{left} (x) / {top} (y)'");

                // Check if the window would be visible on any monitor
                var isVisible = IsWindowVisibleOnAnyScreen(left, top, width, height);

                if (!isVisible)
                {
                    Logger.LogWarning($"Window position ({left},{top}) with size ({width}x{height}) would not be visible on any screen. Repositioning to primary screen.");
                    CenterWindowOnPrimaryScreen(window);
                }
                else
                {
                    // Position is valid, apply it
                    window.SetCurrentValue(Window.LeftProperty, left);
                    window.SetCurrentValue(Window.TopProperty, top);
                }

                // Final safety check
                EnsureWindowIsVisible(window);
            }
            // Important: If restoreWindowPosition is false, we don't do anything with the position
            // This allows the test to pass by keeping the original position

            // Apply window state after positioning to ensure correct monitor
            if (restoreWindowState && savedWindowState != window.WindowState)
            {
                Logger.LogDebug($"Restoring window state for '{windowName}' to '{savedWindowState}'");
                window.SetCurrentValue(Window.WindowStateProperty, savedWindowState);
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, $"Failed to load window size from file '{storageFile}'");

            // Only reposition if restoreWindowPosition is true
            if (restoreWindowPosition)
            {
                CenterWindowOnPrimaryScreen(window);
            }
        }
    }

    private static string GetWindowStorageFile(this IAppDataService appDataService, Window window, string? tag)
    {
        ArgumentNullException.ThrowIfNull(window);

        var appData = appDataService.GetApplicationDataDirectory(ApplicationDataTarget.UserRoaming);
        var directory = Path.Combine(appData, "windows");

        Directory.CreateDirectory(directory);

        var tagToUse = string.Empty;
        if (!string.IsNullOrWhiteSpace(tag))
        {
            tagToUse = $"_{tag.GetSlug()}";
        }

        var file = Path.Combine(directory, $"{window.GetType().FullName}{tagToUse}.dat");
        return file;
    }

    /// <summary>
    /// Checks if a window with the given position and size would be visible on any screen.
    /// </summary>
    private static bool IsWindowVisibleOnAnyScreen(double left, double top, double width, double height)
    {
        // Define minimum visible area (at least 100x100 pixels must be visible)
        const double minVisibleArea = 100;

        foreach (var screen in System.Windows.Forms.Screen.AllScreens)
        {
            var screenBounds = screen.Bounds;

            // Check if at least the minimum visible area of the window is on this screen
            var isWindowVisible =
                left + width > screenBounds.Left + minVisibleArea &&
                left < screenBounds.Right - minVisibleArea &&
                top + height > screenBounds.Top + minVisibleArea &&
                top < screenBounds.Bottom - minVisibleArea;

            if (isWindowVisible)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Final safety check to ensure window is actually visible, but only if it should be positioned
    /// </summary>
    private static void EnsureWindowIsVisible(Window window)
    {
        // Wait for window to be positioned
        window.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);

        // Determine if any part of the window is visible on any screen
        bool isVisible = false;

        var windowRect = new System.Drawing.Rectangle(
            (int)window.Left,
            (int)window.Top,
            (int)Math.Max(window.ActualWidth, 100),
            (int)Math.Max(window.ActualHeight, 100)
        );

        foreach (var screen in System.Windows.Forms.Screen.AllScreens)
        {
            // Create an intersection - if there's any overlap, the window is at least partially visible
            var intersection = System.Drawing.Rectangle.Intersect(windowRect, screen.Bounds);
            if (!intersection.IsEmpty)
            {
                isVisible = true;
                break;
            }
        }

        if (!isVisible)
        {
            // Force window to be visible on primary screen as a last resort
            Logger.LogWarning($"Window '{window.GetType().Name}' is not visible on any screen after positioning. Forcing to primary screen.");
            window.SetCurrentValue(Window.WindowStateProperty, WindowState.Normal);
            CenterWindowOnPrimaryScreen(window);
        }
    }

    /// <summary>
    /// Centers the window on the primary screen.
    /// </summary>
    private static void CenterWindowOnPrimaryScreen(Window window)
    {
        var primaryScreen = System.Windows.Forms.Screen.PrimaryScreen;
        if (primaryScreen is null)
        {
            return;
        }

        var screenBounds = primaryScreen.WorkingArea;

        var left = screenBounds.Left + (screenBounds.Width - window.Width) / 2;
        var top = screenBounds.Top + (screenBounds.Height - window.Height) / 2;

        window.SetCurrentValue(Window.LeftProperty, left);
        window.SetCurrentValue(Window.TopProperty, top);

        Logger.LogDebug($"Centered window '{window.GetType().Name}' on primary screen at ({left},{top})");
    }
}
