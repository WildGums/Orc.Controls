// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePickerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using System;
    using Catel.MVVM;

    public class DateTimePickerViewModel : ViewModelBase
    {
        #region Constructors
        public DateTimePickerViewModel()
        {
            DateTimeValue = DateTime.Now;
        }
        #endregion

        #region Properties
        public DateTime DateTimeValue { get; set; }
        #endregion
    }
}