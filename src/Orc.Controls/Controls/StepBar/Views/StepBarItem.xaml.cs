namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public sealed partial class StepBarItem
    {
        public StepBarItem()
        {
            InitializeComponent();
        }

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation),
            typeof(StepBarItem), new PropertyMetadata(Orientation.Horizontal));

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
        }
    }
}
