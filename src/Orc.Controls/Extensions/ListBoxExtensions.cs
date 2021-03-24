namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using Catel;
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
            scrollViewer.ScrollToHorizontalOffset((double)e.NewValue);
        }

        public static void CenterSelectedItem(this ListBox listBox)
        {
            Argument.IsNotNull(() => listBox);

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
            var afterOffset = 0d;

            foreach (var item in listBox.Items)
            {
                var currentItem = ReferenceEquals(item, selectedItem);
                if (currentItem)
                {
                    isBefore = false;
                }

                var container = listBox.ItemContainerGenerator.ContainerFromItem(selectedItem) as FrameworkElement;
                if (container is null)
                {
                    return;
                }

                var width = container.ActualWidth;

                if (isBefore)
                {
                    beforeOffset += width;
                }
                else if (currentItem)
                {
                    beforeOffset += width / 2;
                    afterOffset += width / 2;
                }
                else
                {
                    afterOffset += width;
                }
            }

            // We now know the actual center, calculate based on the width
            var scrollableArea = scrollViewer.ActualWidth;
            var toValue = beforeOffset - (scrollableArea / 2);
            if (toValue < 0)
            {
                toValue = 0;
            }

            var horizontalAnimation = new DoubleAnimation();
            horizontalAnimation.From = scrollViewer.HorizontalOffset;
            horizontalAnimation.To = toValue;
            horizontalAnimation.DecelerationRatio = .2;
            horizontalAnimation.Duration = new Duration(StepBarConfiguration.AnimationDuration);

            var storyboard = new Storyboard();
            storyboard.Children.Add(horizontalAnimation);
            Storyboard.SetTarget(horizontalAnimation, scrollViewer);
            Storyboard.SetTargetProperty(horizontalAnimation, new PropertyPath(HorizontalOffsetProperty));
            storyboard.Begin();
        }
    }
}
