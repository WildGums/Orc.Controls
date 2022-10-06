// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaElementThreadInfo.cs" company="WildGums">
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
    /// Media element thread info.
    /// </summary>
    public class MediaElementThreadInfo : Disposable
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaElementThreadInfo"/> class.
        /// </summary>
        /// <param name="hostVisual">The host visual.</param>
        /// <param name="createVisual">The function responsible fro creating the visual that will run on the separate UI thread.</param>
        /// <param name="thread">The thread.</param>
        public MediaElementThreadInfo(HostVisual hostVisual, Func<Visual> createVisual, Thread thread)
        {
            Argument.IsNotNull(nameof(hostVisual), hostVisual);
            Argument.IsNotNull(nameof(createVisual), createVisual);
            Argument.IsNotNull(nameof(thread), thread);

            Id = UniqueIdentifierHelper.GetUniqueIdentifier(typeof(MediaElementThreadInfo));
            HostVisual = hostVisual;
            CreateVisual = createVisual;
            Thread = thread;

            thread.Name = $"Media Element Thread: {Id}";
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a unique identifier for this object.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the media element.
        /// </summary>
        /// <value>The media element.</value>
        public HostVisual HostVisual { get; private set; }

        /// <summary>
        /// The function responsible fro creating the visual that will run on the separate UI thread.
        /// </summary>
        public Func<Visual> CreateVisual { get; private set; }

        /// <summary>
        /// Gets the thread that must be killed by the user.
        /// </summary>
        /// <value>The thread.</value>
        public Thread Thread { get; private set; }

        /// <summary>
        /// Gets the dispatcher 
        /// </summary>
        public Dispatcher Dispatcher { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Updates the dispatcher to the specified value.
        /// </summary>
        public void UpdateDispatcher(Dispatcher dispatcher)
        {
            ArgumentNullException.ThrowIfNull(dispatcher);

            if (Dispatcher is not null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Dispatcher is already set and cannot be set again");
            }

            Dispatcher = dispatcher;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();

            var dispatcher = Dispatcher;
            var thread = Thread;

            // Step 1: shut down the dispatcher
            if (dispatcher is not null)
            {
                if (!dispatcher.HasShutdownStarted && !dispatcher.HasShutdownFinished)
                {
                    EventHandler shutdownHandler = null;
                    shutdownHandler = new EventHandler((sender ,e) =>
                    {
                        Log.Debug($"[{Id}] Dispatcher has been shutdown, thread should exit any time now");

                        dispatcher.ShutdownFinished -= shutdownHandler;
                    });

                    dispatcher.ShutdownFinished += shutdownHandler;

                    Log.Debug($"[{Id}] Shutting down the dispatcher");

                    dispatcher.InvokeShutdown();
                }
            }
            else if (thread is not null)
            {
                Log.Warning($"[{Id}] No dispatcher object was available, aborting the thread");

#if NETCOREAPP3_1
                thread.Abort();
#endif
            }
        }
        #endregion
    }
}

#endif
