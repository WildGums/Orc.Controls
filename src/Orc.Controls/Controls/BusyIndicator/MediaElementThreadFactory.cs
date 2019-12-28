// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaElementThreadFactory.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


#if NET || NETCORE

namespace Orc.Controls
{
    using System;
    using System.Threading;
    using System.Windows.Media;
    using System.Windows.Threading;
    using Catel;
    using Catel.Logging;

    /// <summary>
    /// Factory that allows the creation of media elements on a worker thread.
    /// </summary>
    public static class MediaElementThreadFactory
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static readonly AutoResetEvent AutoResetEvent = new AutoResetEvent(false);
        #endregion

        #region Methods
        /// <summary>
        /// Creates the media element on worker thread.
        /// <para />
        /// Note that the <see cref="MediaElementThreadInfo"/> implements <see cref="IDisposable"/>.
        /// </summary>
        /// <returns>The media element thread info.</returns>
        public static MediaElementThreadInfo CreateMediaElementsOnWorkerThread(Func<Visual> createVisual)
        {
            Argument.IsNotNull("createVisual", createVisual);

            var thread = new Thread(WorkerThread);

            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;

            var mediaElementThreadInfo = new MediaElementThreadInfo(new HostVisual(), createVisual, thread);

            thread.Start(mediaElementThreadInfo);

            // Wait for the element to be created by the thread
            AutoResetEvent.WaitOne();

            return mediaElementThreadInfo;
        }

        private static void WorkerThread(object arg)
        {
            try
            {
                var mediaElementThreadInfo = (MediaElementThreadInfo)arg;

                var hostVisual = mediaElementThreadInfo.HostVisual;
                var createVisual = mediaElementThreadInfo.CreateVisual;

                var indeterminateSource = new VisualTargetPresentationSource(hostVisual);
                indeterminateSource.RootVisual = createVisual();

                // The visual has been created, signal the creator of this thread
                AutoResetEvent.Set();

                var dispatcher = Dispatcher.CurrentDispatcher;

                mediaElementThreadInfo.UpdateDispatcher(dispatcher);

                Dispatcher.Run();

                Log.Debug($"[{mediaElementThreadInfo.Id}] Dispatcher has shut down, exiting worker thread");
            }
            catch
            {
                // Ignore
            }
        }
        #endregion
    }
}

#endif
