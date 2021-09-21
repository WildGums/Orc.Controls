namespace Orc.Controls.Example.Views
{
    using System.Windows.Input;

    public partial class SpinButtonView
    {
        public SpinButtonView()
        {
            InitializeComponent();
        }

        private void OnIncreaseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private void OnCanIncrease(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void OnDecreaseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private void OnCanDecrease(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
