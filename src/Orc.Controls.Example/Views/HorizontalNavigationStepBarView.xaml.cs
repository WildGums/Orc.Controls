namespace Orc.Controls.Example.Views
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for HorizontalNavigationWizardView.xaml
    /// </summary>
    public partial class HorizontalNavigationStepBarView
    {
        public HorizontalNavigationStepBarView()
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

