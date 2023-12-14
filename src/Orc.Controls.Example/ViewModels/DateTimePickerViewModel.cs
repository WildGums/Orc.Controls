﻿namespace Orc.Controls.Example.ViewModels
{
    using System;
    using Catel.Collections;
    using System.Globalization;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using System.ComponentModel;
    using Models;

    public class DateTimePickerViewModel : ViewModelBase
    {
        #region Constructors
        public DateTimePickerViewModel()
        {
            AvailableFormats = new FastObservableCollection<CultureFormat>();
            DateTimeValue = DateTime.Now;
            DateTimeValueString = string.Empty;
            SetNull = new Command(OnSetNullExecute);
        }
        #endregion

        #region Properties
        public DateTime? DateTimeValue { get; set; }
        public string DateTimeValueString { get; set; }
        public FastObservableCollection<CultureFormat> AvailableFormats { get; private set; }
        public CultureFormat SelectedFormat { get; set; }
        public Command SetNull { get; }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            using (AvailableFormats.SuspendChangeNotifications())
            {
                foreach (var cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
                {
                    var format = new CultureFormat
                    {
                        Culture = cultureInfo,
                        FormatValue = cultureInfo.DateTimeFormat.ShortDatePattern + " " + cultureInfo.DateTimeFormat.LongTimePattern
                    };

                    AvailableFormats.Add(format);

                    format = new CultureFormat
                    {
                        Culture = cultureInfo,
                        FormatValue = cultureInfo.DateTimeFormat.ShortDatePattern
                    };

                    AvailableFormats.Add(format);

                    format = new CultureFormat
                    {
                        Culture = cultureInfo,
                        FormatValue = cultureInfo.DateTimeFormat.ShortDatePattern + " " + cultureInfo.DateTimeFormat.ShortTimePattern
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
            DateTimeValue = null;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (DateTimeValue is not null && !string.IsNullOrEmpty(e.PropertyName) && e.HasPropertyChanged(e.PropertyName) && SelectedFormat is not null)
            {
                DateTimeValueString = DateTimeValue.Value.ToString(SelectedFormat.FormatValue);
            }
            else
            {
                DateTimeValueString = string.Empty;
            }
        }
        #endregion
    }
}
