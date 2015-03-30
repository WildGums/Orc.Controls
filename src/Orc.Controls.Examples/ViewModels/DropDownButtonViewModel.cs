// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropDownButtonViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    public class DropDownButtonViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;

        public DropDownButtonViewModel(IMessageService messageService)
        {
            Argument.IsNotNull(() => messageService);

            _messageService = messageService;
            DefaultAction = new Command(OnDefaultActionExecute);
        }

        #region Commands
        public Command DefaultAction { get; private set; }

        private void OnDefaultActionExecute()
        {
            _messageService.ShowInformation("Default action has been executed");
        }
        #endregion
    }
}