namespace Orc.Controls.Example.Views
{
    using System;
    using System.Windows.Threading;
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

        private DispatcherTimer _popupTimer;

        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            if (ViewModel is CalloutViewModel vm)
            {
                vm.CalloutManager.Register(buttonCallout);
                vm.CalloutManager.Register(needHelpCallout);

                _popupTimer = new DispatcherTimer();
                _popupTimer.Interval = TimeSpan.FromSeconds(5);
                _popupTimer.Tick += InitializeTimedPopup;
                _popupTimer.Start();
                //_needHelpCalloutVM.IsOpen = true;
                //vm.CalloutManager.Register(_needHelpCalloutVM, calloutStack);
            }
        }

        private void InitializeTimedPopup(object sender, EventArgs e)
        {
            if (needHelpCallout.ViewModel is Controls.CalloutViewModel vm)
            {
                vm.IsOpen = true;
                _popupTimer.Stop();
            }
        }

        protected override void OnUnloaded(EventArgs e)
        {
            base.OnUnloaded(e);

            if (ViewModel is CalloutViewModel vm)
            {
                vm.CalloutManager.Unregister(buttonCallout);
                vm.CalloutManager.Unregister(needHelpCallout);
            }
        }
    }
}
