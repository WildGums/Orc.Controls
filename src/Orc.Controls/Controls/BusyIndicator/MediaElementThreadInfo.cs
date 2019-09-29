// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaElementThreadInfo.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


#if NET || NETCORE

namespace Orc.Controls
{
    using System.Threading;
    using System.Windows.Media;
    using Catel;

    /// <summary>
    /// Media element thread info.
    /// </summary>
    public class MediaElementThreadInfo : Disposable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaElementThreadInfo"/> class.
        /// </summary>
        /// <param name="hostVisual">The host visual.</param>
        /// <param name="thread">The thread.</param>
        public MediaElementThreadInfo(HostVisual hostVisual, Thread thread)
        {
            Argument.IsNotNull(nameof(hostVisual), hostVisual);
            Argument.IsNotNull(nameof(thread), thread);

            HostVisual = hostVisual;
            Thread = thread;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the thread that must be killed by the user.
        /// </summary>
        /// <value>The thread.</value>
        public Thread Thread { get; private set; }

        /// <summary>
        /// Gets the media element.
        /// </summary>
        /// <value>The media element.</value>
        public HostVisual HostVisual { get; private set; }
        #endregion

        #region Methods
        protected override void DisposeManaged()
        {
            base.DisposeManaged();

            var thread = Thread;
            thread?.Abort();

            Thread = null;
        }
        #endregion
    }
}

#endif
