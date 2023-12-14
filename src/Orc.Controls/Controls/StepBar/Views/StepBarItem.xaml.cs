namespace Orc.Controls;

using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using Automation;

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
        typeof(StepBarItem), new PropertyMetadata(Orientation.Horizontal, (sender, e) => ((StepBarItem)sender).OnOrientationChanged()));

    protected override void OnLoaded(EventArgs e)
    {
        base.OnLoaded(e);

        UpdateOrientation();
    }

    private void OnOrientationChanged()
    {
        UpdateOrientation();
    }

    private void UpdateOrientation()
    {
        grid.ColumnDefinitions.Clear();
        grid.RowDefinitions.Clear();

        if (Orientation == Orientation.Horizontal)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            grid.SetCurrentValue(FrameworkElement.MinWidthProperty, 100.0);
            grid.SetCurrentValue(FrameworkElement.MaxWidthProperty, 100.0);
            grid.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness() { Bottom = 8 });

            TitleTextBlock.SetCurrentValue(Grid.ColumnProperty, 0);
            TitleTextBlock.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);

            pathlineCanvas.SetCurrentValue(Grid.ColumnProperty, 1);
            pathlineCanvas.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness(5));
            pathlineCanvas.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            pathline.SetCurrentValue(FrameworkElement.WidthProperty, 48.0);
            pathline.SetCurrentValue(FrameworkElement.HeightProperty, 2.0);
            pathline.SetCurrentValue(Canvas.TopProperty, 13.0);
            pathline.SetCurrentValue(Canvas.LeftProperty, 24.0);

            ellipseVisited.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness(5));
            ellipseCurrent.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness(5));
            ellipseToVisit.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness(5));
        }
        else
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            grid.SetCurrentValue(FrameworkElement.MinWidthProperty, 240.0);
            grid.SetCurrentValue(FrameworkElement.MaxWidthProperty, 240.0);
            grid.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness() { Bottom = 56 });

            TitleTextBlock.SetCurrentValue(Grid.ColumnProperty, 1);
            TitleTextBlock.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);

            pathlineCanvas.SetCurrentValue(Grid.ColumnProperty, 0);
            pathlineCanvas.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness(2));
            pathlineCanvas.SetCurrentValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            pathline.SetCurrentValue(FrameworkElement.WidthProperty, 2.0);
            pathline.SetCurrentValue(FrameworkElement.HeightProperty, 48.0);
            pathline.SetCurrentValue(Canvas.TopProperty, 35.0);
            pathline.SetCurrentValue(Canvas.LeftProperty, 24.0);

            ellipseVisited.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness() { Left = 15, Top = 5, Right = 25, Bottom = 5 });
            ellipseCurrent.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness() { Left = 15, Top = 5, Right = 25, Bottom = 5 });
            ellipseToVisit.Parent.SetCurrentValue(FrameworkElement.MarginProperty, new Thickness() { Left = 15, Top = 5, Right = 25, Bottom = 5 });
        }
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new StepBarItemAutomationPeer(this);
    }
}