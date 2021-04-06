namespace Orc.Controls.Example.Views
{
    using System;
    using System.Windows;
    using System.Windows.Threading;
    using Catel.MVVM;
    using Orc.Controls.Example.ViewModels;

    /// <summary>
    /// Interaction logic for ViewWithCallouts.xaml
    /// </summary>
    public partial class CalloutView
    {
        public CalloutView()
        {
            InitializeComponent();
        }

        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            if (ViewModel is CalloutViewModel vm)
            {
                buttonCallout.ViewModelChanged += RegisterCallout;
                needHelpCallout.ViewModelChanged += RegisterCallout;

                if (needHelpCallout.ViewModel is Controls.CalloutViewModel controlvm)
                {
                    controlvm.IsOpen = true;
                }
            }
        }

        private void RegisterCallout(object sender, EventArgs e)
        {
            if (ViewModel is CalloutViewModel vm && (sender as Callout)?.ViewModel is IViewModel cvm)
            {
                vm.CalloutManager.Register(cvm);
            }
        }

        protected override void OnUnloaded(EventArgs e)
        {
            base.OnUnloaded(e);

            if (ViewModel is CalloutViewModel vm)
            {
                vm.CalloutManager.Unregister(buttonCallout.ViewModel);
                vm.CalloutManager.Unregister(needHelpCallout.ViewModel);
            }
        }
    }
}
