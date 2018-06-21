// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkLabelViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.ViewModels
{
    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    public class LinkLabelViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;

        public LinkLabelViewModel(IMessageService messageService)
        {
            Argument.IsNotNull(() => messageService);

            _messageService = messageService;
            DefaultAction = new Command(OnDefaultActionExecute);
        }

        #region Commands
        public Command DefaultAction { get; private set; }

        private void OnDefaultActionExecute()
        {
            _messageService.ShowInformationAsync("Default action has been executed");
        }
        #endregion
    }
}