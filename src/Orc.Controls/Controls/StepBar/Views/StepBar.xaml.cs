namespace Orc.Controls.Controls.StepBar.Views
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using Orc.Controls.Controls.StepBar.Models;
    using Orc.Controls.Controls.StepBar.ViewModels;

    /// <summary>
    /// Interaction logic for StepBar.xaml
    /// </summary>
    public sealed partial class StepBar
    {
        #region Fields
        private StepBarViewModel _stepBarViewModel;
        #endregion Fields

        #region Constructors
        public StepBar()
        {
            InitializeComponent();
        }
        #endregion Constructors

        #region Methods
        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

            if (ViewModel is StepBarViewModel vm)
            {
                _stepBarViewModel = vm;
            }
        }
        #endregion Methods

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

        #region Commands
        public void MoveForwardAsync()
        {
            _stepBarViewModel?.MoveForwardAsync();
        }

        public void MoveBackAsync()
        {
            _stepBarViewModel?.MoveBackAsync();
        }
        #endregion Commands
    }
}
