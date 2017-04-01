// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateRangePickerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Threading.Tasks;
    using Catel.Collections;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Threading;
    using Models;
    using System.Collections.ObjectModel;

    public class DateRangePickerViewModel : ViewModelBase
    {
        #region Constructors
        public DateRangePickerViewModel()
        {

        }
        #endregion

        #region Properties
        public FastObservableCollection<DateRange> Ranges { get; set; }
        public DateRange SelectedRange { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public TimeSpan Duration { get; set; }
        public string DurationString { get; set; }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            var ranges = new FastObservableCollection<DateRange>();
            using (ranges.SuspendChangeNotifications())
            {
                ranges.Add(PredefinedDateRanges.Today);
                ranges.Add(PredefinedDateRanges.Yesterday);
                ranges.Add(PredefinedDateRanges.ThisWeek);
                ranges.Add(PredefinedDateRanges.LastWeek);
                ranges.Add(PredefinedDateRanges.ThisMonth);
                ranges.Add(PredefinedDateRanges.LastMonth);
            }

            Ranges = ranges;
            DateStart = ranges[0].Start;
            DateEnd = ranges[0].End;
            Duration = ranges[0].Duration;
            SelectedRange = ranges[0];
        }

        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (Duration != null && !string.IsNullOrEmpty(e.PropertyName) && e.HasPropertyChanged(e.PropertyName))
            {
                DurationString = Duration.ToString();
            }
            else
            {
                DurationString = string.Empty;
            }
        }
        #endregion
    }
}
