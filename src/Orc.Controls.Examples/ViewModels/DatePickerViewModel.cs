// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatePickerViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using System;
    using Catel.MVVM;

    public class DatePickerViewModel : ViewModelBase
    {
        #region Constructors
        public DatePickerViewModel()
        {
            DateValue = DateTime.Today;
        }
        #endregion

        #region Properties
        public DateTime DateValue { get; set; }
        #endregion
    }
}