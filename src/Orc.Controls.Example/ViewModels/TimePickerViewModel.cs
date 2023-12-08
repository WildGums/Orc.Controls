namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Enums;

    public class TimePickerViewModel : ViewModelBase
    {
        #region Constructors
        public TimePickerViewModel()
        {
            Time = TimeSpan.Zero;
            TimeValueString = string.Empty;
            AmPm = Meridiem.AM;
            Is24Hour = true;
            HourThickness = 6;
            MinuteThickness = 4;
            HourTickThickness = 3;
            MinuteTickThickness = 2;
            ClockBorderThickness = 0;
            SetNull = new Command(OnSetNullExecute);
        }
        #endregion

        #region Properties
        public TimeSpan? Time { get; set; }
        public string TimeValueString { get; set; }
        public Meridiem AmPm { get; set; }
        public bool Is24Hour { get; set; }
        public double HourThickness { get; set; }
        public double MinuteThickness { get; set; }
        public double HourTickThickness { get; set; }
        public double MinuteTickThickness { get; set; }
        public double ClockBorderThickness { get; set; }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (Time is not null && !string.IsNullOrEmpty(e.PropertyName) && e.HasPropertyChanged(e.PropertyName))
            {
                if (Is24Hour)
                {
                    if (AmPm == Meridiem.PM)
                    {
                        if (Time.Value.Hours < 12)
                        {
                            var newTimeValue = new TimeSpan(Time.Value.Hours + 12, Time.Value.Minutes, Time.Value.Seconds);
                            TimeValueString = newTimeValue.ToString();
                        }
                        else
                        {
                            var newTimeValue = new TimeSpan(Time.Value.Hours - 12, Time.Value.Minutes, Time.Value.Seconds);
                            TimeValueString = newTimeValue.ToString();
                        }
                    }
                    else 
                    {
                        TimeValueString = Time.Value.ToString();
                    }

                }
                else
                {
                    TimeValueString = Time.Value.ToString() + " " + AmPm.ToString();
                }

            }
            else
            {
                TimeValueString = string.Empty;
            }
        }
        #endregion

        #region Commands
        public Command SetNull { get; }
        private void OnSetNullExecute()
        {
            Time = null;
        }
        #endregion
    }

}
