namespace Orc.Controls.Example.Views
{
    using System;
    using System.Windows;
    using System.Windows.Threading;
    using Orc.Controls.Example.ViewModels;

    /// <summary>
    /// Interaction logic for ViewWithCallouts.xaml
    /// </summary>
    public partial class CalloutView
    {
        private Controls.CalloutViewModel _printButtonCalloutVM;
        private Controls.CalloutViewModel _needHelpCalloutVM;

        public CalloutView()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(EventArgs e)
        {
            base.OnLoaded(e);

            if (ViewModel is CalloutViewModel vm)
            {
                _printButtonCalloutVM = new Controls.CalloutViewModel()
                {
                    ControlName = "Print Button.",
                    Description = "This is a print button.",
                    Visible = Visibility.Visible,
                    PlacementTarget = printButton,
                };

                vm.CalloutManager.Register(_printButtonCalloutVM, buttonCallout);


                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(10);
                timer.Tick += InitializeTimedPopup;
                timer.Start();
                //_needHelpCalloutVM.IsOpen = true;
                //vm.CalloutManager.Register(_needHelpCalloutVM, calloutStack);
            }
        }

        private void InitializeTimedPopup(object sender, EventArgs e)
        {
            if (ViewModel is CalloutViewModel vm)
            {
                _needHelpCalloutVM = new Controls.CalloutViewModel()
                {
                    ControlName = "Need help ?",
                    Description = "In case if you are confused, this example is responsible for testing various callouts. Try clicking the print button to show it's callout.",
                    Visible = Visibility.Visible,
                    PlacementTarget = calloutStack,
                    IsOpen = true
                };

                vm.CalloutManager.Register(_needHelpCalloutVM, needHelpCallout);
            }
        }

        protected override void OnUnloaded(EventArgs e)
        {
            base.OnUnloaded(e);

            if (ViewModel is CalloutViewModel vm)
            {
                vm.CalloutManager.Unregister(_printButtonCalloutVM);
            }
        }
    }
}
