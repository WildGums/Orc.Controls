namespace Orc.Controls.Example.ViewModels
{
    using System;
    using Catel.MVVM;
    using Catel.Services;

    public class DropDownButtonViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;

        public DropDownButtonViewModel(IServiceProvider serviceProvider, IMessageService messageService)
            : base(serviceProvider)
        {
            _messageService = messageService;

            DefaultAction = new Command(serviceProvider, OnDefaultActionExecute);
        }

        public Command DefaultAction { get; private set; }

        private void OnDefaultActionExecute()
        {
            _messageService.ShowInformationAsync("Default action has been executed");
        }
    }
}
