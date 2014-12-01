using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TimeSpanControl
{
    public class MainViewViewModel: INotifyPropertyChanged
    {
        private TimeSpan _timeSpanValue;
        public TimeSpan TimeSpanValue
        {
            get
            {
                return _timeSpanValue;
            }
            set
            {
                if(_timeSpanValue == value) return;
                _timeSpanValue = value;
                OnPropertyChanged("TimeSpanValue");
            }
        }

        public MainViewViewModel() 
        {
            TimeSpanValue = new TimeSpan(10, 11, 12, 13);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
