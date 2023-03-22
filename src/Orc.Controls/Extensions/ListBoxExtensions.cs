namespace Orc.Controls;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Catel.Windows;

public static class ListBoxExtensions
{
    public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.RegisterAttached("HorizontalOffset",
        typeof(double), typeof(ListBoxExtensions), new UIPropertyMetadata(0.0, OnHorizontalOffsetChanged));

    public static void SetHorizontalOffset(FrameworkElement target, double value)
    {
        target.SetValue(HorizontalOffsetProperty, value);
    }

    public static double GetHorizontalOffset(FrameworkElement target)
    {
        return (double)target.GetValue(HorizontalOffsetProperty);
    }

    private static void OnHorizontalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
    {
        var scrollViewer = target as ScrollViewer;
        scrollViewer?.ScrollToHorizontalOffset((double)e.NewValue);
        //scrollViewer.UpdateLayout();
    }

    public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached("VerticalOffset",
        typeof(double), typeof(ListBoxExtensions), new UIPropertyMetadata(0.0, OnVerticalOffsetChanged));

    public static void SetVerticalOffset(FrameworkElement target, double value)
    {
        target.SetValue(VerticalOffsetProperty, value);
    }

    public static double GetVerticalOffset(FrameworkElement target)
    {
        return (double)target.GetValue(VerticalOffsetProperty);
    }

    private static void OnVerticalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
    {
        var scrollViewer = target as ScrollViewer;
        scrollViewer?.ScrollToVerticalOffset((double)e.NewValue);
        //scrollViewer.UpdateLayout();
    }

    public static void CenterSelectedItem(this ListBox listBox, Orientation orientation)
    {
        ArgumentNullException.ThrowIfNull(listBox);

        var scrollViewer = listBox.FindVisualDescendantByType<ScrollViewer>();
        if (scrollViewer is null)
        {
            return;
        }

        var selectedItem = listBox.SelectedItem;
        if (selectedItem is null)
        {
            return;
        }

        var isBefore = true;
        var beforeOffset = 0d;

        foreach (var item in listBox.Items)
        {
            var currentItem = ReferenceEquals(item, selectedItem);
            if (currentItem)
            {
                isBefore = false;
            }

            if (listBox.ItemContainerGenerator.ContainerFromItem(selectedItem) is not FrameworkElement container)
            {
                return;
            }

            var value = orientation switch
            {
                Orientation.Horizontal => container.ActualWidth,
                Orientation.Vertical => container.ActualHeight,
                _ => 0d
            };

            if (isBefore)
            {
                beforeOffset += value;
            }
            else if (currentItem)
            {
                beforeOffset += value / 2;
            }
        }

        var animation = new DoubleAnimation
        {
            DecelerationRatio = .2,
            Duration = new Duration(StepBarConfiguration.AnimationDuration)
        };

        var storyboard = new Storyboard();
        storyboard.Children.Add(animation);

        switch (orientation)
        {
            case Orientation.Horizontal:
                // We now know the actual center, calculate based on the width
                var horizontalScrollableValue = scrollViewer.ActualWidth;
                var horizontalToValue = beforeOffset - (horizontalScrollableValue / 2);
                if (horizontalToValue < 0)
                {
                    horizontalToValue = 0;
                }

                animation.From = scrollViewer.HorizontalOffset;
                animation.To = horizontalToValue;
                    
                Storyboard.SetTargetProperty(animation, new PropertyPath(HorizontalOffsetProperty));
                break;

            case Orientation.Vertical:
                // We now know the actual center, calculate based on the height
                var verticalScrollableValue = scrollViewer.ActualHeight;
                var verticalToValue = beforeOffset - (verticalScrollableValue / 2);
                if (verticalToValue < 0)
                {
                    verticalToValue = 0;
                }

                animation.From = scrollViewer.VerticalOffset;
                animation.To = verticalToValue;

                Storyboard.SetTargetProperty(animation, new PropertyPath(VerticalOffsetProperty));
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
        }

        Storyboard.SetTarget(animation, scrollViewer);

        storyboard.Begin();
    }
}
