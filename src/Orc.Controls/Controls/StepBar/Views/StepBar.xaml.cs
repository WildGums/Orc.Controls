namespace Orc.Controls
{
    using System.Collections.Generic;
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

        public bool AllowSelection
        {
            get { return (bool)GetValue(AllowSelectionProperty); }
            set { SetValue(AllowSelectionProperty, value); }
        }

        public static readonly DependencyProperty AllowSelectionProperty = DependencyProperty.Register(nameof(AllowSelection), typeof(bool),
            typeof(StepBar), new PropertyMetadata(true));
        #endregion Properties

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel is StepBarViewModel && Items != null && Items.Count > 0)
            {
                var vm = (StepBarViewModel)ViewModel;
                vm.Items = Items;
                vm.AllowSelection = AllowSelection;
                Items[Items.Count - 1].State |= StepBarItemStates.IsLast;
                vm.SetSelectedItem(0);
            }
        }
    }
}
