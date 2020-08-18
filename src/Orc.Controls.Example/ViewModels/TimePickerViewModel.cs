
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
            TimeValue = TimeSpan.Zero;
            TimeValueString = string.Empty;
            AmPm = Meridiem.AM;
            SetNull = new Command(OnSetNullExecute);
            SetAmPm = new Command(OnSetAmPm);
        }

        public TimeSpan? TimeValue { get; set; }
        public string TimeValueString { get; set; }
        public Meridiem AmPm { get; set; }

        public Command SetNull { get; }
        public Command SetAmPm { get; }
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        private void OnSetNullExecute()
        {
            TimeValue = null;
        }

        private void OnSetAmPm()
        {
            switch
                (AmPm)
            {
                case Meridiem.AM:
                    AmPm = Meridiem.PM;
                    break;
                default:
                    AmPm = Meridiem.AM;
                    break;
            }
        }

        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (TimeValue != null && !string.IsNullOrEmpty(e.PropertyName) && e.HasPropertyChanged(e.PropertyName) && TimeValue.Value != null)
            {
                TimeValueString = TimeValue.Value.ToString() + " " + AmPm.ToString();
            }
            else
            {
                TimeValueString = string.Empty;
            }
        }
    }

}
