// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyObjectExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    public static class DependencyObjectExtensions
    {
        #region Methods
        public static IEnumerable<DependencyObject> GetDescendents(this DependencyObject root)
        {
            var count = VisualTreeHelper.GetChildrenCount(root);
            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                yield return child;

                foreach (var descendent in GetDescendents(child))
                {
                    yield return descendent;
                }
            }
        }

        public static DependencyObject GetVisualRoot(this DependencyObject dependencyObject)
        {
            DependencyObject root;
            var parent = dependencyObject;

            do
            {
                root = parent;
                parent = VisualTreeHelper.GetParent(root);
            } while (parent is not null);

            return root;
        }
        #endregion
    }
}
