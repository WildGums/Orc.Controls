// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScreenHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Media;

    internal static class ScreenHelper
    {
        public static Size GetWindowSize(this Window window)
        {
            if (window == null)
            {
                window = System.Windows.Application.Current.MainWindow;
            }

            return new Size(window.ActualWidth, window.ActualHeight);
        }

        public static bool IsParentOf(this DependencyObject parent, DependencyObject child)
        {
            if (parent == null || child == null)
            {
                return false;
            }

            DependencyObject element;

            do
            {
                if (child is Visual)
                {
                    element = VisualTreeHelper.GetParent(child);
                }
                else
                {
                    element = LogicalTreeHelper.GetParent(child);
                }

                if (ReferenceEquals(element, parent))
                {
                    return true;
                }

                child = element;
            } while (element != null);

            return false;
        }
    }
}