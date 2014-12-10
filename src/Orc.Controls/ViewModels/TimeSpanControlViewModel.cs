using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Orc.Controls
{
    public class TimeSpanControlViewModel: INotifyPropertyChanged
    {
        private TimeSpan _value;
        public TimeSpan Value
        {
            get { return _value; }
            set
            {
                if (_value == value) return;
                _value = value;
                OnPropertyChanged("Days");
                OnPropertyChanged("Hours");
                OnPropertyChanged("Minutes");
                OnPropertyChanged("Seconds");
                OnPropertyChanged("Value");
            }
        }

        public int Days
        {
            get { return _value.Days; }
            set
            {
                if (_value.Days == value) return;
                Value = new TimeSpan(value, Hours, Minutes, Seconds);
            }
        }

        public int Hours
        {
            get { return _value.Hours; }
            set
            {
                if (_value.Hours == value) return;
                Value = new TimeSpan(Days, value, Minutes, Seconds);
            }
        }

        public int Minutes
        {
            get { return _value.Minutes; }
            set
            {
                if (_value.Minutes==value)return;
                Value = new TimeSpan(Days, Hours, value, Seconds);
            }
        }

        public int Seconds
        {
            get { return _value.Seconds; }
            set
            {
                if (_value.Seconds == value) return;
                Value = new TimeSpan(Days, Hours, Minutes, value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}