// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Margin.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using Catel.Windows.Data;

    public class Margin : DependencyObject
    {
        #region Left
        public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached(
            "Left", typeof(double), typeof(Margin), new PropertyMetadata(double.NaN, OnMarginDimensionChangedChanged));

        public static double GetLeft(DependencyObject d)
        {
            return (double) d.GetValue(LeftProperty);
        }
        public static void SetLeft(DependencyObject d, double value)
        {
            d.SetValue(LeftProperty, value);
        }
        #endregion

        #region Top
        public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached(
            "Top", typeof(double), typeof(Margin), new PropertyMetadata(double.NaN, OnMarginDimensionChangedChanged));

        public static double GetTop(DependencyObject d)
        {
            return (double) d.GetValue(TopProperty);
        }
        public static void SetTop(DependencyObject d, double value)
        {
            d.SetValue(TopProperty, value);
        }
        #endregion

        #region Right
        public static readonly DependencyProperty RightProperty = DependencyProperty.RegisterAttached(
            "Right", typeof(double), typeof(Margin), new PropertyMetadata(double.NaN, OnMarginDimensionChangedChanged));

        public static double GetRight(DependencyObject d)
        {
            return (double) d.GetValue(RightProperty);
        }
        public static void SetRight(DependencyObject d, double value)
        {
            d.SetValue(RightProperty, value);
        }
        #endregion

        #region Bottom
        public static readonly DependencyProperty BottomProperty = DependencyProperty.RegisterAttached(
            "Bottom", typeof(double), typeof(Margin), new PropertyMetadata(double.NaN, OnMarginDimensionChangedChanged));

        public static double GetBottom(DependencyObject d)
        {
            return (double) d.GetValue(BottomProperty);
        }
        public static void SetBottom(DependencyObject d, double value)
        {
            d.SetValue(BottomProperty, value);
        }
        #endregion

        private static void OnMarginDimensionChangedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = d as FrameworkElement;
            frameworkElement.UnsubscribeFromDependencyProperty(nameof(FrameworkElement.Margin), OnMarginChanged);
            frameworkElement?.SubscribeToDependencyProperty(nameof(FrameworkElement.Margin), OnMarginChanged);
        }

        private static void OnMarginChanged(object sender, DependencyPropertyValueChangedEventArgs e)
        {
            if(!(sender is FrameworkElement frameworkElement))
            {
                return;
            }

            frameworkElement.UnsubscribeFromDependencyProperty(nameof(FrameworkElement.Margin), OnMarginChanged);

            var currentMargin = (Thickness)e.NewValue;

            var left = GetLeft(frameworkElement);
            var top = GetTop(frameworkElement);
            var right = GetRight(frameworkElement);
            var bottom = GetBottom(frameworkElement);

            left = double.IsNaN(left) ? currentMargin.Left : left;
            top = double.IsNaN(top) ? currentMargin.Top : top;
            right = double.IsNaN(right) ? currentMargin.Right : right;
            bottom = double.IsNaN(bottom) ? currentMargin.Bottom : bottom;
            
            frameworkElement.Margin = new Thickness(left, top, right, bottom);
        }
    }
}
