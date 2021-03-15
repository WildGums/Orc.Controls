namespace Orc.Controls.Example.Views
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for SideNavigationWizardWindow.xaml
    /// </summary>
    public partial class SideNavigationStepBarView
    {
        public SideNavigationStepBarView()
        {
            InitializeComponent();
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            stepbar.MoveForwardAsync();
        }

        private void Previous_Button_Click(object sender, RoutedEventArgs e)
        {
            stepbar.MoveBackAsync();
        }
    }
}

