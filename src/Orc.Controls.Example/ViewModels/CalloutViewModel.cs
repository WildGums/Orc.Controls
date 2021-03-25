namespace Orc.Controls.Example.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows;
    using Catel.MVVM;

    public class CalloutViewModel : ViewModelBase
    {
        public CalloutViewModel()
        {
            OpenCallout = new TaskCommand<UIElement>(OpenCalloutExecuteAsync);
        }

        public CalloutManager CalloutManager { get; set; } = new CalloutManager();

        public TaskCommand<UIElement> OpenCallout { get; private set; }

        public Task OpenCalloutExecuteAsync(UIElement element)
        {
            foreach (var callout in CalloutManager.Callouts)
            {
                if (callout.PlacementTarget == element)
                {
                    callout.IsOpen = true;
                }
            }
            return Task.CompletedTask;
        }
    }
}
