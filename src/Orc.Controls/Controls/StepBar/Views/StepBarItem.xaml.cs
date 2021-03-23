namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public sealed partial class StepBarItem
    {
        #region Constructors
        public StepBarItem()
        {
            InitializeComponent();
        }
        #endregion Constructors

        #region Properties
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string),
            typeof(StepBarItem), new PropertyMetadata(string.Empty));

        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register(nameof(Number), typeof(int),
            typeof(StepBarItem), new PropertyMetadata(0));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation),
            typeof(StepBarItem), new PropertyMetadata(Orientation.Vertical));

        public StepBarItemStates State
        {
            get { return (StepBarItemStates)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(nameof(State), typeof(StepBarItemStates),
            typeof(StepBarItem), new PropertyMetadata(StepBarItemStates.None));
        #endregion Properties

        #region Methods
        protected override void OnLoaded(EventArgs e)
        {
            if (Orientation == Orientation.Horizontal)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.SetCurrentValue(FrameworkElement.MinWidthProperty, 100.0);
                grid.SetCurrentValue(FrameworkElement.MaxWidthProperty, 100.0);
                grid.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness() { Bottom = 8 });
                txtTitle.SetCurrentValue(Grid.ColumnProperty, 0);
                txtTitle.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                pathline.SetCurrentValue(Grid.ColumnProperty, 1);
                pathline.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness(5));
                pathline.Parent.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                pathline.SetCurrentValue(FrameworkElement.WidthProperty, 48.0);
                pathline.SetCurrentValue(FrameworkElement.HeightProperty, 2.0);
                pathline.SetCurrentValue(Canvas.TopProperty, 13.0);
                ellipse.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness(5));
            }
            else
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.SetCurrentValue(FrameworkElement.MinWidthProperty, 240.0);
                grid.SetCurrentValue(FrameworkElement.MaxWidthProperty, 240.0);
                grid.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness() { Bottom = 56 });
                txtTitle.SetCurrentValue(Grid.ColumnProperty, 1);
                txtTitle.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                pathline.SetCurrentValue(Grid.ColumnProperty, 0);
                pathline.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness(2));
                pathline.Parent.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                pathline.SetCurrentValue(FrameworkElement.WidthProperty, 2.0);
                pathline.SetCurrentValue(FrameworkElement.HeightProperty, 48.0);
                pathline.SetCurrentValue(Canvas.TopProperty, 35.0);
                ellipse.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness() { Left = 15, Top = 5, Right = 25, Bottom = 5 });
            }
            if ((State & StepBarItemStates.IsLast) != 0)
            {
                pathline.SetCurrentValue(VisibilityProperty, Visibility.Hidden);
            }
        }
        #endregion Methods
    }
}
