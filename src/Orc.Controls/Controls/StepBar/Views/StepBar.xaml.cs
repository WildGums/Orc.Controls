namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using Catel.MVVM;
    using Catel.MVVM.Views;

    /// <summary>
    /// Interaction logic for StepBar.xaml
    /// </summary>
    public sealed partial class StepBar
    {
        #region Constructors
        static StepBar()
        {
            typeof(StepBar).AutoDetectViewPropertiesToSubscribe();
        }

        public StepBar()
        {
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
            typeof(StepBar), new PropertyMetadata(Orientation.Horizontal));

        [ViewToViewModel]
        public IList<IStepBarItem> Items
        {
            get { return (IList<IStepBarItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(IList<IStepBarItem>),
            typeof(StepBar));

        [ViewToViewModel]
        public IStepBarItem SelectedItem
        {
            get { return (IStepBarItem)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(IStepBarItem),
            typeof(StepBar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion Properties
    }
}
