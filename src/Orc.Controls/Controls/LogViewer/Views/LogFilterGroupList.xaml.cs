// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFilterGroupList.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel;

    public partial class LogFilterGroupList
    {
        #region Fields
        private LogFilterGroupListViewModel _lastKnownViewModel;
        #endregion

        #region Constructors
        public LogFilterGroupList()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            if (_lastKnownViewModel is not null)
            {
                _lastKnownViewModel.Updated -= OnViewModelUpdated;
                _lastKnownViewModel = null;
            }

            _lastKnownViewModel = ViewModel as LogFilterGroupListViewModel;
            if (_lastKnownViewModel is not null)
            {
                _lastKnownViewModel.Updated += OnViewModelUpdated;
            }
        }

        private void OnViewModelUpdated(object sender, EventArgs e)
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        public event EventHandler<EventArgs> Updated;
    }
}
