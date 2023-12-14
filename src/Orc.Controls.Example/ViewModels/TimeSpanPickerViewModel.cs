namespace Orc.Controls.Example.ViewModels
{
    using System;
    using Catel.MVVM;

    public class TimeSpanPickerViewModel : ViewModelBase
    {
        public TimeSpanPickerViewModel()
        {
            TimeSpanValue = new TimeSpan(10, 11, 12, 13);
            SetNull = new Command(OnSetNullExecute);
        }

        public Command SetNull { get; private set; }

        public TimeSpan? TimeSpanValue { get; set; }

        private void OnSetNullExecute()
        {
            TimeSpanValue = null;
        }
    }
}