namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for StepBar.xaml
    /// </summary>
    public sealed partial class StepBar
    {
        #region Constructors
        public StepBar()
        {
            Loaded += OnLoaded;

            InitializeComponent();
        }
        #endregion Constructors

        #region Properties
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation),
            typeof(StepBar), new PropertyMetadata(Orientation.Vertical));

        public IList<IStepBarItem> Items
        {
            get { return (IList<IStepBarItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(IList<IStepBarItem>),
            typeof(StepBar));

        public bool AllowQuickNavigation
        {
            get { return (bool)GetValue(AllowQuickNavigationProperty); }
            set { SetValue(AllowQuickNavigationProperty, value); }
        }

        public static readonly DependencyProperty AllowQuickNavigationProperty = DependencyProperty.Register(nameof(AllowQuickNavigation), typeof(bool),
            typeof(StepBar), new PropertyMetadata(true));
        #endregion Properties

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel is StepBarViewModel && Items != null)
            {
                var vm = (StepBarViewModel)ViewModel;
                vm.AllowQuickNavigation = AllowQuickNavigation;
                vm.Items = Items;
                vm.SelectedItem = Items.First();
                vm.SetSelectedItem(0);
            }
        }
    }
}
