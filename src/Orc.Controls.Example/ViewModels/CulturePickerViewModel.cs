namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Threading;

    public class CulturePickerViewModel : ViewModelBase
    {
        #region Fields
        private readonly IMessageService _messageService;

        private bool _isInitializing;
        #endregion

        #region Constructors
        public CulturePickerViewModel(IMessageService messageService)
        {
            ArgumentNullException.ThrowIfNull(messageService);

            _messageService = messageService;
        }
        #endregion

        #region Properties
        public CultureInfo Culture { get; set; }
        #endregion

        #region Methods
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

            return Task.CompletedTask;
        }

#pragma warning disable AvoidAsyncVoid
        private async void OnCultureChanged()
#pragma warning restore AvoidAsyncVoid
        {
            if (_isInitializing)
            {
                return;
            }

            await _messageService.ShowAsync($"Selected culture {Culture.EnglishName}");
        }
        #endregion
    }
}
