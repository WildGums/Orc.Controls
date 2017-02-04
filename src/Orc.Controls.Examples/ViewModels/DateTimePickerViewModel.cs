// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePickerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using System;
    using Catel.Collections;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Threading;
    using System.Collections.Generic;

    public class DateTimePickerViewModel : ViewModelBase
    {
        #region Constructors
        public DateTimePickerViewModel()
        {
            AvailableFormats = new FastObservableCollection<object>();
            DateTimeValue = DateTime.Now;

            SetNull = new Command(OnSetNullExecute);
        }
        #endregion

        #region Properties
        public DateTime? DateTimeValue { get; set; }
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
                        FormatValue = cultureInfo.DateTimeFormat.ShortDatePattern + " " + cultureInfo.DateTimeFormat.LongTimePattern
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
        #endregion
    }
}