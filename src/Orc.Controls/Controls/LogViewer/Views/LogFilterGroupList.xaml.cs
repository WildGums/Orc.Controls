// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilterGroupListControl.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Controls
{
    using System;
    using Catel;

    public partial class LogFilterGroupList
    {
        private LogFilterGroupListViewModel _lastKnownViewModel;

        public LogFilterGroupList()
        {
            InitializeComponent();
        }

        public event EventHandler<EventArgs> Updated;

        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            if (_lastKnownViewModel != null)
            {
                _lastKnownViewModel.Updated -= OnViewModelUpdated;
                _lastKnownViewModel = null;
            }

            _lastKnownViewModel = ViewModel as LogFilterGroupListViewModel;
            if (_lastKnownViewModel != null)
            {
                _lastKnownViewModel.Updated += OnViewModelUpdated;
            }
        }

        private void OnViewModelUpdated(object sender, EventArgs e)
        {
            Updated.SafeInvoke(this);
        }
    }
}
