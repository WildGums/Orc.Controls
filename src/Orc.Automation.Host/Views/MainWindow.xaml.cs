namespace Orc.Automation.Host.Views
{
    using System.Windows;

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            CanCloseUsingEscape = false;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //Do some temporary test stuff here
        }
    }
}
