namespace Orc.Controls.Example.ViewModels;

using System;
using Catel.MVVM;

public partial class TimeSpanPickerViewModel : ViewModelBase
{
    public TimeSpanPickerViewModel(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        TimeSpanValue = new TimeSpan(10, 11, 12, 13);

        SetNull = new Command(serviceProvider, OnSetNullExecute);
    }

    public Command SetNull { get; private set; }

    public TimeSpan? TimeSpanValue { get; set; }

    private void OnSetNullExecute()
    {
        TimeSpanValue = null;
    }
}
