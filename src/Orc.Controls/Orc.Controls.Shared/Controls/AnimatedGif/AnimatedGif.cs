// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimatedGif.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;
    using Catel.Logging;

    /// <summary>
    /// User control supporting animated gif.
    /// </summary>
    public class AnimatedGif : System.Windows.Controls.Image
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private Bitmap _bitmap;
        #endregion

        #region Win32 imports
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        private static extern IntPtr DeleteObject(IntPtr hDc);
        #endregion

        #region Delegates
        /// <summary>
        /// OnFrameChanged delegate.
        /// </summary>
        private delegate void OnFrameChangedDelegate();
        #endregion

        #region Constructors
        public AnimatedGif()
        {
            Focusable = false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether this instance is animating.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is animating; otherwise, <c>false</c>.
        /// </value>
        private bool IsAnimating { get; set; }

        /// <summary>
        /// Gets or sets the current frame.
        /// </summary>
        /// <value>The current frame.</value>
        private int CurrentFrame { get; set; }

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
        public static readonly DependencyProperty GifSourceProperty = DependencyProperty.Register("GifSource", typeof(string),
            typeof(AnimatedGif), new UIPropertyMetadata(string.Empty, OnGifSourceChanged));
        #endregion

        #region Methods
        /// <summary>
        /// Invoked when the GifSource dependency property has changed.
        /// </summary>
        /// <param name="sender">The object that contains the dependency property.</param>
        /// <param name="e">The event data.</param>
        [DebuggerStepperBoundary]
        private static void OnGifSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var typedSender = sender as AnimatedGif;
            if (typedSender != null)
            {
                typedSender.SetImageGifSource();
            }
        }

        /// <summary>
        /// Sets the image gif source.
        /// </summary>
        private void SetImageGifSource()
        {
            if (_bitmap != null)
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
                Assembly assemblyToSearch = Assembly.GetAssembly(GetType());
                _bitmap = GetBitmapResourceFromAssembly(assemblyToSearch);
                if (_bitmap == null)
                {
                    // Search calling assembly
                    assemblyToSearch = Assembly.GetCallingAssembly();
                    _bitmap = GetBitmapResourceFromAssembly(assemblyToSearch);
                    if (_bitmap == null)
                    {
                        // Get entry assembly
                        assemblyToSearch = Catel.Reflection.AssemblyHelper.GetEntryAssembly();
                        _bitmap = GetBitmapResourceFromAssembly(assemblyToSearch);
                        if (_bitmap == null)
                        {
                            throw Log.ErrorAndCreateException<FileNotFoundException>("Gif source '{0}' was not found", GifSource);
                        }
                    }
                }
            }

            if (_bitmap != null)
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
        private Bitmap GetBitmapResourceFromAssembly(Assembly assemblyToSearch)
        {
            // Loop through all resources
            if (assemblyToSearch.FullName != null)
            {
                try
                {
                    // Get stream resource info
                    var streamResourceInfo = Application.GetResourceStream(new Uri(GifSource, UriKind.RelativeOrAbsolute));
                    if (streamResourceInfo != null)
                    {
                        return (Bitmap)Image.FromStream(streamResourceInfo.Stream);
                    }
                }
                catch (IOException)
                {
                    // IOException when not existent URI path.
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Called when a frame has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        [DebuggerStepperBoundary]
        private void OnFrameChanged(object sender, EventArgs e)
        {
            // Dispatch the frame changed
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new OnFrameChangedDelegate(OnFrameChangedInMainThread));
        }

        /// <summary>
        /// Called when a frame changed in the main thread.
        /// </summary>
        [DebuggerStepperBoundary]
        private void OnFrameChangedInMainThread()
        {
            // _bitmap should never be null
            if (_bitmap != null)
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

                SetCurrentValue(SourceProperty, null);
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
        #endregion
    }
}