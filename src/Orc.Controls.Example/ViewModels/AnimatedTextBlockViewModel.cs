namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Windows.Threading;

    public class AnimatedTextBlockViewModel : ViewModelBase
    {
        private DispatcherTimerEx _dispatcherTimerEx;
        private readonly Random _random = new Random();

        private int _currentIndex;

        public AnimatedTextBlockViewModel(IDispatcherService dispatcherService)
        {
            _dispatcherTimerEx = new DispatcherTimerEx(dispatcherService);
        }

        public string Status { get; private set; }

        protected override Task InitializeAsync()
        {
            _dispatcherTimerEx.Tick += OnDispatcherTimerTick;
            _dispatcherTimerEx.Start();

            return base.InitializeAsync();
        }

        protected override Task CloseAsync()
        {
            _dispatcherTimerEx.Tick -= OnDispatcherTimerTick;
            _dispatcherTimerEx.Stop();

            return base.CloseAsync();
        }

        private void OnDispatcherTimerTick(object sender, EventArgs e)
        {
            _dispatcherTimerEx.Stop();
            _dispatcherTimerEx.Interval = TimeSpan.FromMilliseconds(_random.Next(100, 5000));
            _dispatcherTimerEx.Start();

            Status = $"Status {1 + _currentIndex++}";
        }
    }
}
