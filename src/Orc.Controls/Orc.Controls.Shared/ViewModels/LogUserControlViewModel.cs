// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogUserControlViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Logging;

    public class LogUserControlViewModel : ViewModelBase
    {
        #region Constructors
        public LogUserControlViewModel()
        {
        }
        #endregion

        #region Properties
        public Type LogListenerType { get; set; }
        #endregion

        protected override async Task Initialize()
        {
            await base.Initialize();

            // TODO: subscribe to events here
        }

        protected override async Task Close()
        {
            // TODO: unsubscribe from events here

            await base.Close();
        }
    }
}