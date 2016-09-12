// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatePickerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Catel.Collections;
    using Catel.MVVM;
    using Catel.Threading;

    public class DatePickerViewModel : ViewModelBase
    {
        #region Constructors
        public DatePickerViewModel()
        {
            AvailableFormats = new FastObservableCollection<object>();
            DateValue = DateTime.Today;

            SetNull = new Command(OnSetNullExecute);
        }
        #endregion

        #region Properties
        public DateTime? DateValue { get; set; }
        public FastObservableCollection<object> AvailableFormats { get; private set; }
        public object SelectedFormat { get; set; }

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
                    var format = new
                    {
                        CultureCode = string.Format("[{0}]", cultureInfo.IetfLanguageTag),
                        FormatValue = cultureInfo.DateTimeFormat.ShortDatePattern
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
            DateValue = null;
        }
        #endregion
    }
}