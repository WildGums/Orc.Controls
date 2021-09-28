// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeViewItemExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows.Controls;
    using System.Windows.Media;

    public static class TreeViewItemExtensions
    {
        #region Methods
        public static int GetDepth(this TreeViewItem item)
        {
            var depth = 0;
            TreeViewItem parent;

            while ((parent = GetParent(item)) is not null)
            {
                depth++;
                item = parent;
            }

            return depth;
        }

        private static TreeViewItem GetParent(TreeViewItem item)
        {
            var parent = VisualTreeHelper.GetParent(item);

            while (parent is not (TreeViewItem or TreeView))
            {
                if (parent is null)
                {
                    return null;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as TreeViewItem;
        }
        #endregion
    }
}
