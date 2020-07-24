namespace Orc.Controls.Example.ViewModels
{
    using System;
    using Catel.Collections;
    using System.Globalization;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using System.ComponentModel;
    using Catel.Data;
    using Models;
    using Orc.Controls.Enums;

    public class TimePickerViewModel : ViewModelBase
    {
        #region Constructors
        public TimePickerViewModel()
        {
            _time = new AnalogueTime(0, 0, Meridiem.AM);
        }
        #endregion

        #region Properties
        private AnalogueTime _time;

        

        private readonly DigitalTime _digitalTime;
        public DigitalTime MinTime { get { return new DigitalTime(9, 0); } }
        public DigitalTime MaxTime { get { return new DigitalTime(21, 0); } }
        public AnalogueTime Time
        {
            get { return _time; }
            set
            {
                if (!_time.Equals(value))
                {
                    _time = value;
                    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Time)));
                    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DigitalTime)));
                }
            }
        }
        public DigitalTime DigitalTime
        {
            get { return Time.ToDigitalTime(); }
            set
            {
                Time = value.ToAnalogueTime();
            }
        }
        #endregion

        #region Methods
        //public event PropertyChangedEventHandler PropertyChanged;
        //protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        //{
        //    base.OnPropertyChanged(e);

        //    if (Time != null && !string.IsNullOrEmpty(e.PropertyName) && e.HasPropertyChanged(e.PropertyName))
        //    {
        //        Time = Time.ToString();
        //    }
        //    else
        //    {
        //        Time = string.Empty;
        //    }
        //}

        private void AMPMButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Time.Meridiem == Meridiem.AM)
            {
                Time = new AnalogueTime(Time.Hour, Time.Minute, Meridiem.PM);
            }
            else
            {
                Time = new AnalogueTime(Time.Hour, Time.Minute, Meridiem.AM);
            }
        }
        #endregion
    }
}
