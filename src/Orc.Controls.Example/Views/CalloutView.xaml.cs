namespace Orc.Controls.Example.Views
{
    using System;
    using System.Windows;
    using Orc.Controls.Example.ViewModels;

    /// <summary>
    /// Interaction logic for ViewWithCallouts.xaml
    /// </summary>
    public partial class CalloutView
    {
        private Controls.CalloutViewModel _printButtonCalloutVM;

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
            }
        }

        protected override void OnUnloaded(EventArgs e)
        {
            base.OnUnloaded(e);

            if (ViewModel is CalloutViewModel vm)
            {
                vm.CalloutManager.UnRegister(_printButtonCalloutVM);
            }
        }
    }
}
