
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimePicker.cs" company="">
// Clock-like TimePicker control https://github.com/roy-t/TimePicker
// </copyright>
//---------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Catel.Data;
    using Catel.MVVM;
    using Enums;

    public class TimePickerViewModel : ViewModelBase
    {
        public TimePickerViewModel()
        {
            Time = TimeSpan.Zero;
            TimeValueString = string.Empty;
            AmPm = Meridiem.AM;
            SetNull = new Command(OnSetNullExecute);
            HourThickness = 6;
            MinuteThickness = 4;
            HourTickThickness = 3;
            MinuteTickThickness = 2;
            ClockBorderThickness = 0;
        }

        public TimeSpan? Time { get; set; }
        public string TimeValueString { get; set; }
        public Meridiem AmPm { get; set; }
        public double HourThickness { get; set; }
        public double MinuteThickness { get; set; }
        public double HourTickThickness { get; set; }
        public double MinuteTickThickness { get; set; }
        public double ClockBorderThickness { get; set; }
        public Command SetNull { get; }
        public Command SetAmPm { get; }
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        private void OnSetNullExecute()
        {
            Time = null;
        }

        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (Time != null && !string.IsNullOrEmpty(e.PropertyName) && e.HasPropertyChanged(e.PropertyName) && Time.Value != null)
            {
                TimeValueString = Time.Value.ToString() + " " + AmPm.ToString();
            }
            else
            {
                TimeValueString = string.Empty;
            }
        }
    }

}
