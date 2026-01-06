namespace Orc.Controls.Example.ViewModels
{
    using System;
    using Catel.MVVM;
    using Catel.Services;

    public class LinkLabelViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;

        public LinkLabelViewModel(IServiceProvider serviceProvider, IMessageService messageService)
            : base(serviceProvider)
        {
            _messageService = messageService;

            DefaultAction = new Command(serviceProvider, OnDefaultActionExecute);
        }

        public Command DefaultAction { get; }

        private void OnDefaultActionExecute()
        {
            _messageService.ShowInformationAsync("Default action has been executed");
        }
    }
}
