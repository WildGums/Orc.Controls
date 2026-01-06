namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel.Collections;
    using Catel.MVVM;
    using Catel.Services;

    public class DateRangePickerViewModel : ViewModelBase
    {
        private readonly IDispatcherService _dispatcherService;

        public DateRangePickerViewModel(IServiceProvider serviceProvider, IDispatcherService dispatcherService)
            : base(serviceProvider)
        {
            _dispatcherService = dispatcherService;
        }

        public FastObservableCollection<DateRange> Ranges { get; set; }
        public DateRange SelectedRange { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan Span { get; set; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            var ranges = new FastObservableCollection<DateRange>(_dispatcherService);

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
    }
}
