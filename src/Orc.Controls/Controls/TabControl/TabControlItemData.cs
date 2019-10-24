// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabControl.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Item data for a tab control item.
    /// </summary>
    public class TabControlItemData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabControlItemData" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="content">The content.</param>
        /// <param name="contentTemplate">The content template.</param>
        /// <param name="item">The item.</param>
        public TabControlItemData(object container, object content, DataTemplate contentTemplate, object item)
        {
            Container = container;
            TabItem = container as TabItem;

            if (TabItem != null)
            {
#pragma warning disable WPF0041 // Set mutable dependency properties using SetCurrentValue.
                TabItem.Content = null;
                TabItem.ContentTemplate = null;
#pragma warning restore WPF0041 // Set mutable dependency properties using SetCurrentValue.
            }

            Content = content;
            ContentTemplate = contentTemplate;
            Item = item;
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public object Container { get; private set; }

        /// <summary>
        /// Gets the tab item.
        /// </summary>
        /// <value>The tab item.</value>
        public TabItem TabItem { get; private set; }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>The content.</value>
        public object Content { get; private set; }

        /// <summary>
        /// Gets the content template.
        /// </summary>
        /// <value>The content.</value>
        public DataTemplate ContentTemplate { get; private set; }

        /// <summary>
        /// The item from which it was generated.
        /// </summary>
        /// <value>The item.</value>
        public object Item { get; private set; }
    }
}
