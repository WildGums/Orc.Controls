namespace Orc.Controls.Example.ViewModels
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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan Span { get; set; }
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
            StartDate = ranges[0].Start;
            EndDate = ranges[0].End;
            Span = ranges[0].Duration;
            SelectedRange = ranges[0];
        }
        #endregion
    }
}
