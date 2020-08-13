
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimePicker.cs" company="">
// Clock-like TimePicker control https://github.com/roy-t/TimePicker
// </copyright>
//---------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.Views
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using Catel.Data;
    using Enums;

    public partial class TimePicker: INotifyPropertyChanged
    {
        //private AnalogueTime _time;
        //private readonly DigitalTime _digitalTime;

        public TimePicker()
        {
            //_time = new AnalogueTime(0, 0, Meridiem.AM);
            //TimeValueString = string.Empty;
            InitializeComponent();
            //this.DataContext = ViewModels.TimePickerViewModel;
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //public AnalogueTime Time
        //{
        //    get { return _time; }
        //    set
        //    {
        //        if (!_time.Equals(value))
        //        {
        //            _time = value;
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Time)));
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DigitalTime)));
        //        }
        //    }
        //}

        //public string TimeValueString { get; set; }
        //public DigitalTime MinTime { get { return new DigitalTime(9, 0); } }
        //public DigitalTime MaxTime { get { return new DigitalTime(21, 0); } }

        //public DigitalTime DigitalTime
        //{
        //    get { return Time.ToDigitalTime(); }
        //    set
        //    {
        //        Time = value.ToAnalogueTime();
        //    }
        //}
        //private void AMPMButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (Time.Meridiem == Meridiem.AM)
        //    {
        //        Time = new AnalogueTime(Time.Hour, Time.Minute, Meridiem.PM);
        //    }
        //    else
        //    {
        //        Time = new AnalogueTime(Time.Hour, Time.Minute, Meridiem.AM);
        //    }
        //}

        //protected void OnPropertyChanged(PropertyChangedEventArgs e)
        //{
        //    if (Time != null && !string.IsNullOrEmpty(e.PropertyName) && e.HasPropertyChanged(e.PropertyName) && Time != null)
        //    {
        //        TimeValueString = Time.ToString();
        //    }
        //    else
        //    {
        //        TimeValueString = string.Empty;
        //    }
        //}

    }
}
