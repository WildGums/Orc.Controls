// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatePickerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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

    public class DatePickerViewModel : ViewModelBase
    {
        #region Constructors
        public DatePickerViewModel()
        {
            AvailableFormats = new FastObservableCollection<CultureFormat>();
            DateValue = DateTime.Today;
            DateValueString = string.Empty;

            SetNull = new Command(OnSetNullExecute);
        }
        #endregion

        #region Properties
        public DateTime? DateValue { get; set; }
        public string DateValueString { get; set; }
        public FastObservableCollection<CultureFormat> AvailableFormats { get; private set; }
        public CultureFormat SelectedFormat { get; set; }

        public Command SetNull { get; private set; }
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
                        CultureCode = string.Format("[{0}]", cultureInfo.IetfLanguageTag),
                        FormatValue = cultureInfo.DateTimeFormat.ShortDatePattern
                    };

                    AvailableFormats.Add(format);
                    if (cultureInfo.Equals(CultureInfo.CurrentCulture))
                    {
                        SelectedFormat = format;
                        DateValueString = DateValue.Value.ToString(format.FormatValue);
                    }
                }
            }
        }

        private void OnSetNullExecute()
        {
            DateValue = null;
        }

        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (DateValue != null && !string.IsNullOrEmpty(e.PropertyName) && e.HasPropertyChanged(e.PropertyName) && DateValue.Value != null && SelectedFormat != null)
            {
                DateValueString = DateValue.Value.ToString(SelectedFormat.FormatValue);
            }
            else
            {
                DateValueString = string.Empty;
            }
        }
        #endregion
    }
}