// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropDownButtonViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    public class DropDownButtonViewModel : ViewModelBase
    {
        private readonly IDispatcherService _dispatcherService;

        public DropDownButtonViewModel(IDispatcherService dispatcherService)
        {
            Argument.IsNotNull(() => dispatcherService);

            _dispatcherService = dispatcherService;
            Items = new List<string> {"asasddddddddddddddddddddddddddd", "asasd"};
            DropDown = new TaskCommand(OnDropDownExecute, OnDropDownCanExecute);
            
        }

        

        public IList<string> Items { get; set; }

        public bool IsDropDownOpen { get; set; }

        public string Header { get; set; }

        #region Commands
        public TaskCommand DropDown { get; private set; }

        private bool OnDropDownCanExecute()
        {
            return true;
        }

        private async Task OnDropDownExecute()
        {
/*
            try
            {
IsDropDownOpen = !IsDropDownOpen;
            }
            catch (Exception)
            {
                
                throw;
            }
*/
                
           
        }
        #endregion
        
    }
}