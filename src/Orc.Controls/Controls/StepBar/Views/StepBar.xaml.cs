namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using Catel.MVVM.Views;

    /// <summary>
    /// Interaction logic for StepBar.xaml
    /// </summary>
    public partial class StepBar
    {
        static StepBar()
        {
            typeof(StepBar).AutoDetectViewPropertiesToSubscribe();
        }

        public StepBar()
        {
            InitializeComponent();
        }

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation),
            typeof(StepBar), new PropertyMetadata(Orientation.Vertical));
    }
}
