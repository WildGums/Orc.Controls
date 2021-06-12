namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Threading;
    using Catel;
    using Catel.MVVM;

    public class CalloutViewModel : ViewModelBase
    {
        private readonly DispatcherTimer _showCalloutDispatcherTimer = new DispatcherTimer();

        public CalloutViewModel(ICalloutManager calloutManager)
        {
            Argument.IsNotNull(() => calloutManager);

            CalloutManager = calloutManager;
            OpenCallout = new TaskCommand<object>(OpenCalloutExecuteAsync);
            ToggleShowRepeatedly = new TaskCommand(OnShowRepeatedlyExecuteAsync);
        }

        public ICalloutManager CalloutManager { get; }

        public TaskCommand<object> OpenCallout { get; private set; }

        public Task OpenCalloutExecuteAsync(object parameter)
        {
            //CalloutManager.ShowAllCallouts();
            CalloutManager.Callouts.ForEach(x => x.Show());

            return Task.CompletedTask;
        }

        public TaskCommand ToggleShowRepeatedly { get; private set; }

        private async Task OnShowRepeatedlyExecuteAsync()
        {
            if (_showCalloutDispatcherTimer.IsEnabled)
            {
                _showCalloutDispatcherTimer.Stop();
                return;
            }
            _showCalloutDispatcherTimer.Start();
        }

        private async void OnShowCalloutDispatcherTimerTick(object sender, EventArgs e)
        {
            await OpenCalloutExecuteAsync(null);
        }

        protected override async Task InitializeAsync()
        {
            _showCalloutDispatcherTimer.Interval = TimeSpan.FromSeconds(3);
            _showCalloutDispatcherTimer.Tick += OnShowCalloutDispatcherTimerTick;

            await base.InitializeAsync();
        }

        protected override Task CloseAsync()
        {
            base.CloseAsync();

            CalloutManager.HideAllCallouts();
            _showCalloutDispatcherTimer.Stop();
            _showCalloutDispatcherTimer.Tick -= OnShowCalloutDispatcherTimerTick;

            return Task.CompletedTask;
        }
    }
}
