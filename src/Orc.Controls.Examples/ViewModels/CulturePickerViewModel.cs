// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CulturePickerViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2016 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using System.Globalization;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Threading;

    public class CulturePickerViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;

        public CulturePickerViewModel(IMessageService messageService)
        {
            Argument.IsNotNull(() => messageService);

            _messageService = messageService;
        }

        public CultureInfo Culture { get; set; }

        private bool _isInitializing;
        protected override Task InitializeAsync()
        {
            _isInitializing = true;
            try
            {
                Culture = CultureInfo.CurrentUICulture;
            }
            finally
            {
                _isInitializing = false;
            }            

            return TaskHelper.Completed;
        }

        private async void OnCultureChanged()
        {
            if (_isInitializing)
            {
                return;
            }

            await _messageService.ShowAsync(string.Format("Selected culture {0}", Culture.EnglishName));
        }
    }
}