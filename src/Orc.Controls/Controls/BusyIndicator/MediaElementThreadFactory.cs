namespace Orc.Controls;

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
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private static readonly AutoResetEvent AutoResetEvent = new AutoResetEvent(false);

    /// <summary>
    /// Creates the media element on worker thread.
    /// <para />
    /// Note that the <see cref="MediaElementThreadInfo"/> implements <see cref="IDisposable"/>.
    /// </summary>
    /// <returns>The media element thread info.</returns>
    public static MediaElementThreadInfo CreateMediaElementsOnWorkerThread(Func<Visual> createVisual)
    {
        ArgumentNullException.ThrowIfNull(createVisual);

        var thread = new Thread(WorkerThread);

        thread.SetApartmentState(ApartmentState.STA);
        thread.IsBackground = true;

        var mediaElementThreadInfo = new MediaElementThreadInfo(new HostVisual(), createVisual, thread);

        thread.Start(mediaElementThreadInfo);

        // Wait for the element to be created by the thread
        AutoResetEvent.WaitOne();

        return mediaElementThreadInfo;
    }

    private static void WorkerThread(object? arg)
    {
        if (arg is not MediaElementThreadInfo mediaElementThreadInfo)
        {
            return;
        }

        try
        {
            var hostVisual = mediaElementThreadInfo.HostVisual;
            var createVisual = mediaElementThreadInfo.CreateVisual;

            //TODO: Check if it will work the same without this element
            var indeterminateSource = new VisualTargetPresentationSource(hostVisual)
            {
                RootVisual = createVisual()
            };

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
}
