
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimePicker.cs" company="">
// Clock-like TimePicker control https://github.com/roy-t/TimePicker
// </copyright>
//---------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel.Collections;
    using Catel.Data;
    using Catel.MVVM;
    using Enums;
    using Models;

    public class TimePickerViewModel : ViewModelBase
    {
        public TimePickerViewModel()
        {
            Time = new AnalogueTime(12, 00, Meridiem.AM);
            SetAmPm = new Command(OnSetAmPm);
        }

        public TimeSpan? TimeValue { get; set; }
        public string TimeValueString { get; set; }
        public FastObservableCollection<CultureFormat> AvailableFormats { get; private set; }
        public CultureFormat SelectedFormat { get; set; }

        public Command SetNull { get; }


        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            using (AvailableFormats.SuspendChangeNotifications())
            {
                foreach (var cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
                {
                    var format = new CultureFormat
                    {
                        CultureCode = $"[{cultureInfo.IetfLanguageTag}]",
                        FormatValue = cultureInfo.DateTimeFormat.LongTimePattern
                    };
                    AvailableFormats.Add(format);

                    format = new CultureFormat
                    {
                        CultureCode = $"[{cultureInfo.IetfLanguageTag}]",
                        FormatValue = cultureInfo.DateTimeFormat.ShortTimePattern
                    };

                    AvailableFormats.Add(format);

                    if (cultureInfo.Equals(CultureInfo.CurrentCulture))
                    {
                        SelectedFormat = format;
                    }
                }
            }
        }

        private void OnSetNullExecute()
        {
            TimeValue = null;
        }

        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (TimeValue != null && !string.IsNullOrEmpty(e.PropertyName) && e.HasPropertyChanged(e.PropertyName) && TimeValue.Value != null && SelectedFormat != null)
            {
                var timeSpanFormat = Orc.Controls.TimeSpanFormatter.ChangeFormat(SelectedFormat.FormatValue);
                TimeValueString = TimeValue.Value.ToString(timeSpanFormat);
            }
            else
            {
                TimeValueString = string.Empty;
            }
        }
    }

}
