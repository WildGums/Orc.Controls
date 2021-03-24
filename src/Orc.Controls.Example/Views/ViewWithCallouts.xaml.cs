namespace Orc.Controls.Example.Views
{
    using System;
    using System.Windows;
    using Orc.Controls.Example.ViewModels;

    /// <summary>
    /// Interaction logic for ViewWithCallouts.xaml
    /// </summary>
    public partial class ViewWithCallouts
    {
        private CalloutViewModel _printButtonCalloutVM;

        public ViewWithCallouts()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(EventArgs e)
        {
            base.OnLoaded(e);

            if (ViewModel is ViewWithCalloutsViewModel vm)
            {
                _printButtonCalloutVM = new CalloutViewModel()
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

            if (ViewModel is ViewWithCalloutsViewModel vm)
            {
                vm.CalloutManager.UnRegister(_printButtonCalloutVM);
            }
        }
    }
}
