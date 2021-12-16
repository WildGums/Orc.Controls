// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectTextOnLoaded.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Timers;
    using Catel.IoC;
    using Catel.Services;
    using Catel.Windows.Interactivity;
#if NET || NETCORE
    using System.Windows.Controls;

#elif NETFX_CORE
    using Windows.UI.Xaml.Controls;
#endif

    public class SelectTextOnLoaded : BehaviorBase<TextBox>
    {
        #region Constants
        private const double DelayBeforeTextSelected = 10d;
        #endregion

        #region Fields
        private readonly IDispatcherService _dispatcherService;
#pragma warning disable IDISP006 // Implement IDisposable.
        private readonly Timer _textSelectTimer = new Timer(DelayBeforeTextSelected);
#pragma warning restore IDISP006 // Implement IDisposable.
        #endregion

        #region Constructors
        public SelectTextOnLoaded()
        {
#pragma warning disable IDISP004 // Don't ignore created IDisposable.
            _dispatcherService = this.GetServiceLocator().ResolveType<IDispatcherService>();
#pragma warning restore IDISP004 // Don't ignore created IDisposable.
        }
        #endregion

        #region Methods
        protected override void OnAssociatedObjectLoaded()
        {
            base.OnAssociatedObjectLoaded();

            _textSelectTimer.Elapsed += OnSearchTimerElapsed;
            _textSelectTimer.Start();
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            _textSelectTimer.Stop();
            _textSelectTimer.Elapsed -= OnSearchTimerElapsed;

            base.OnAssociatedObjectUnloaded();
        }

        private void OnSearchTimerElapsed(object sender, EventArgs e)
        {
            _textSelectTimer.Stop();

            _dispatcherService.Invoke(() => AssociatedObject?.SelectAll());
        }
        #endregion
    }
}
