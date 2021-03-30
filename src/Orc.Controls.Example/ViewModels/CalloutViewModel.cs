namespace Orc.Controls.Example.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows;
    using Catel.MVVM;

    public class CalloutViewModel : ViewModelBase
    {
        public CalloutViewModel()
        {
            CalloutManager = new CalloutManager();
            OpenCallout = new TaskCommand<UIElement>(OpenCalloutExecuteAsync);
        }

        public CalloutManager CalloutManager { get; }

        public TaskCommand<UIElement> OpenCallout { get; private set; }

        public Task OpenCalloutExecuteAsync(UIElement element)
        {
            foreach (var callout in CalloutManager.Callouts)
            {
                if (callout.ViewModel is Controls.CalloutViewModel vm)
                {
                    if (vm.PlacementTarget == element)
                    {
                        vm.IsOpen = true;
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
