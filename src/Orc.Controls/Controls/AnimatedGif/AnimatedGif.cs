namespace Orc.Controls;

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Automation;
using Catel.Logging;
using Catel.Reflection;

/// <summary>
/// User control supporting animated gif.
/// </summary>
public class AnimatedGif : System.Windows.Controls.Image
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private Bitmap? _bitmap;

    [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
    private static extern IntPtr DeleteObject(IntPtr hDc);
    /// <summary>
    /// OnFrameChanged delegate.
    /// </summary>
    private delegate void OnFrameChangedDelegate();

    public AnimatedGif()
    {
        Focusable = false;
    }

    /// <summary>
    /// Gets or sets GifSource.
    /// </summary>
    /// <remarks>
    /// Wrapper for the GifSource dependency property.
    /// </remarks>
    public string GifSource
    {
        get { return (string)GetValue(GifSourceProperty); }
        set { SetValue(GifSourceProperty, value); }
    }

    /// <summary>
    /// DependencyProperty definition as the backing store for GifSource.
    /// </summary>
    public static readonly DependencyProperty GifSourceProperty = DependencyProperty.Register(nameof(GifSource), typeof(string),
        typeof(AnimatedGif), new UIPropertyMetadata(string.Empty, OnGifSourceChanged));

    /// <summary>
    /// Invoked when the GifSource dependency property has changed.
    /// </summary>
    /// <param name="sender">The object that contains the dependency property.</param>
    /// <param name="e">The event data.</param>
    [DebuggerStepperBoundary]
    private static void OnGifSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var typedSender = sender as AnimatedGif;
        typedSender?.SetImageGifSource();
    }

    /// <summary>
    /// Sets the image gif source.
    /// </summary>
    private void SetImageGifSource()
    {
        if (_bitmap is not null)
        {
            ImageAnimator.StopAnimate(_bitmap, OnFrameChanged);

            _bitmap = null;
        }

        if (string.IsNullOrEmpty(GifSource))
        {
            SetCurrentValue(SourceProperty, null);

            InvalidateVisual();

            return;
        }

        // Check if this is a file
        if (File.Exists(GifSource))
        {
            _bitmap = (Bitmap)Image.FromFile(GifSource);
        }
        else
        {
            // Support looking for embedded resources
            var type = GetType();
            var assemblyToSearch = type.GetAssemblyEx();
            _bitmap = GetBitmapResourceFromAssembly(assemblyToSearch);
            if (_bitmap is null)
            {
                // Search calling assembly
                assemblyToSearch = Assembly.GetCallingAssembly();
                _bitmap = GetBitmapResourceFromAssembly(assemblyToSearch);
                if (_bitmap is null)
                {
                    // Get entry assembly
                    assemblyToSearch = AssemblyHelper.GetEntryAssembly();
                    if (assemblyToSearch is null)
                    {
                        throw Log.ErrorAndCreateException<FileNotFoundException>("Gif source '{0}' was not found", GifSource);
                    }

                    _bitmap = GetBitmapResourceFromAssembly(assemblyToSearch);
                    if (_bitmap is null)
                    {
                        throw Log.ErrorAndCreateException<FileNotFoundException>("Gif source '{0}' was not found", GifSource);
                    }
                }
            }
        }

        if (_bitmap is not null)
        {
            // Start animating
            ImageAnimator.Animate(_bitmap, OnFrameChanged);
        }
    }

    /// <summary>
    /// Gets the bitmap resource from a specific assembly.
    /// </summary>
    /// <param name="assemblyToSearch">The assembly to search.</param>
    /// <returns><see cref="Bitmap"/> or null if resource is not found.</returns>
    [DebuggerStepperBoundary]
    private Bitmap? GetBitmapResourceFromAssembly(Assembly assemblyToSearch)
    {
        // Loop through all resources
        if (assemblyToSearch.FullName is null)
        {
            return null;
        }

        try
        {
            // Get stream resource info
            var streamResourceInfo = Application.GetResourceStream(new Uri(GifSource, UriKind.RelativeOrAbsolute));
            if (streamResourceInfo is not null)
            {
                return (Bitmap)Image.FromStream(streamResourceInfo.Stream);
            }
        }
        catch (IOException)
        {
            // IOException when not existent URI path.
            return null;
        }

        return null;
    }

    /// <summary>
    /// Called when a frame has changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    [DebuggerStepperBoundary]
    private void OnFrameChanged(object? sender, EventArgs e)
    {
        // Dispatch the frame changed, but with lower prio so rendering won't screw up
        Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new OnFrameChangedDelegate(OnFrameChangedInMainThread));
    }

    /// <summary>
    /// Called when a frame changed in the main thread.
    /// </summary>
    [DebuggerStepperBoundary]
    private void OnFrameChangedInMainThread()
    {
        if (!IsVisible)
        {
            return;
        }

        var wasNull = ReferenceEquals(null, GetValue(SourceProperty));
        var isNull = false;

        // _bitmap should never be null
        if (_bitmap is not null)
        {
            // Update the frames
            ImageAnimator.UpdateFrames(_bitmap);

            // Get bitmap source
            var bitmapSource = GetBitmapSource(_bitmap);
            SetCurrentValue(SourceProperty, bitmapSource);
        }
        else
        {
            Log.Debug("Bitmap is NULL while animating");

            isNull = true;

            SetCurrentValue(SourceProperty, null);
        }

        if (wasNull && isNull)
        {
            return;
        }

        // Invalidate visual
        InvalidateVisual();
    }

    /// <summary>
    /// Gets the bitmap source.
    /// </summary>
    /// <param name="gdiBitmap">The GDI bitmap.</param>
    /// <returns></returns>
    [DebuggerStepperBoundary]
    private static BitmapSource GetBitmapSource(Bitmap gdiBitmap)
    {
        // Get the bitmap
        var hBitmap = gdiBitmap.GetHbitmap();

        // Create the bitmap
        var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        // Delete bitmap pointer
        DeleteObject(hBitmap);

        // Return bitmap source
        return bitmapSource;
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new AnimatedGifAutomationPeer(this);
    }
}
