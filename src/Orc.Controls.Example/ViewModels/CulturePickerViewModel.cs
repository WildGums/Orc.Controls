namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Services;

    public class CulturePickerViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;

        private bool _isInitializing;

        public CulturePickerViewModel(IServiceProvider serviceProvider, IMessageService messageService)
            : base(serviceProvider)
        {
            _messageService = messageService;
        }

        public CultureInfo Culture { get; set; }

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
    }
}
