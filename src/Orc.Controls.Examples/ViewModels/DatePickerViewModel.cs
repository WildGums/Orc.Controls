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
        }
        #endregion

        #region Properties
        public DateTime DateValue { get; set; }
        public FastObservableCollection<object> AvailableFormats { get; private set; }
        public object SelectedFormat { get; set; }
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
        #endregion
    }
}