namespace Orc.Controls.Controls.StepBar.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using Orc.Controls.Controls.StepBar.ViewModels;

    /// <summary>
    /// Interaction logic for StepBar.xaml
    /// </summary>
    public partial class StepBar
    {
        private StepBarViewModel _vm;

        public StepBar()
        {
            InitializeComponent();
        }

        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            if (ViewModel is StepBarViewModel vm)
            {
                _vm = vm;
            }
        }

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation),
            typeof(StepBar), new PropertyMetadata(Orientation.Vertical));

        public void MoveForwardAsync()
            => _vm?.MoveForwardAsync();

        public void MoveBackAsync()
            => _vm?.MoveBackAsync();
    }
}
