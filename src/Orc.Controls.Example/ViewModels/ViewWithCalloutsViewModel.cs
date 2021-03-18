namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Orc.Controls.Controls.Callout.Models;
    using Orc.Controls.Example.Views;

    public class ViewWithCalloutsViewModel : ViewModelBase
    {
        public ViewWithCalloutsViewModel()
        {
            ContentLoaded = new TaskCommand<ViewWithCallouts>(ContentLoadedExecuteAsync, ContentLoadedCanExecute);
            CalloutManager = new CalloutManager();
        }

        public CalloutManager CalloutManager { get; set; }

        public TaskCommand<ViewWithCallouts> ContentLoaded { get; private set; }

        public List<ViewWithCalloutsViewModel> Callouts { get; set; }

        public bool ContentLoadedCanExecute(ViewWithCallouts callout)
            => true;

        public Task ContentLoadedExecuteAsync(ViewWithCallouts callout)
        {
            var x = callout.Content;
            return Task.CompletedTask;
        }
    }
}
