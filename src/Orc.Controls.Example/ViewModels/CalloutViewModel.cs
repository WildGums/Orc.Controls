namespace Orc.Controls.Example.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.MVVM;

    public class CalloutViewModel : ViewModelBase
    {
        public CalloutViewModel(ICalloutManager calloutManager)
        {
            Argument.IsNotNull(() => calloutManager);

            CalloutManager = calloutManager;
            OpenCallout = new TaskCommand<object>(OpenCalloutExecuteAsync);
        }

        public ICalloutManager CalloutManager { get; }

        public TaskCommand<object> OpenCallout { get; private set; }

        public Task OpenCalloutExecuteAsync(object parameter)
        {
            //CalloutManager.ShowAllCallouts();
            CalloutManager.Callouts.ForEach(x => x.Show());

            return Task.CompletedTask;
        }

        protected override Task CloseAsync()
        {
            base.CloseAsync();

            CalloutManager.HideAllCallouts();

            return Task.CompletedTask;
        }
    }
}
