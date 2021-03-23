namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.Controls.Controls.Callout.Models;
    using Orc.Controls.Example.Views;

    public class ViewWithCalloutsViewModel : ViewModelBase
    {
        public ViewWithCalloutsViewModel()
        {
            CalloutManager = new CalloutManager();
            ContentLoaded = new TaskCommand<ViewWithCallouts>(ContentLoadedExecuteAsync, ContentLoadedCanExecute);
            ContentUnLoaded = new TaskCommand<ViewWithCallouts>(ContentUnLoadedExecuteAsync, ContentUnLoadedCanExecute);
        }

        public CalloutManager CalloutManager { get; set; }

        public TaskCommand<ViewWithCallouts> ContentLoaded { get; private set; }

        public TaskCommand<ViewWithCallouts> ContentUnLoaded { get; private set; }

        public bool ContentLoadedCanExecute(ViewWithCallouts view)
            => true;

        public bool ContentUnLoadedCanExecute(ViewWithCallouts view)
            => true;

        public async Task ContentLoadedExecuteAsync(ViewWithCallouts view)
        {

            var buttonData = view.buttonCallout.DataContext;
        }

        public Task ContentUnLoadedExecuteAsync(ViewWithCallouts view)
        {
            CalloutManager.Callouts.Clear();
            return Task.CompletedTask;
        }
    }
}
