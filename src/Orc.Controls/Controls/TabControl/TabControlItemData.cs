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

            if (container is TabItem tabItem)
            {
#pragma warning disable WPF0041 // Set mutable dependency properties using SetCurrentValue.
                tabItem.Content = null;
                tabItem.ContentTemplate = null;
#pragma warning restore WPF0041 // Set mutable dependency properties using SetCurrentValue.

                TabItem = tabItem;
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
