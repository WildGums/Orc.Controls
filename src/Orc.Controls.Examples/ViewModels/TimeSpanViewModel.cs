// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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