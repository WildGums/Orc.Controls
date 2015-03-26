// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using System;
    using Catel.MVVM;

    public class TimeSpanViewModel : ViewModelBase
    {
        public TimeSpanViewModel()
        {
            TimeSpanValue = new TimeSpan(10, 11, 12, 13);
        }

        public TimeSpan TimeSpanValue { get; set; }
    }
}