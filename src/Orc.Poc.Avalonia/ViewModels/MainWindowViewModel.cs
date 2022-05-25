namespace Orc.Poc.Avalonia.ViewModels
{
    using System;
    using global::Avalonia.Threading;

    public class MainWindowViewModel : Catel.MVVM.ViewModelBase
    {
        private readonly DispatcherTimer _dispatcherTimer;
        
        public MainWindowViewModel()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(4000);
            _dispatcherTimer.Tick += OnTimerTick;
            
            _dispatcherTimer.Start();
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            _dispatcherTimer.Stop();
            
            Name = "Title_1";
        }

        public string? Name { get; set; } = "This is name title";
    }
}
