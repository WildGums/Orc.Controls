namespace Orc.Controls.Settings;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

internal static class WpfTreeHelper
{
    /// <summary>
    /// Gets the next parent in the hierarchy, with special handling for TabControl virtualization.
    /// </summary>
    /// <param name="element">The current element</param>
    /// <returns>The next parent element, or null if none found</returns>
    public static DependencyObject? GetNextParent(DependencyObject? element)
    {
        if (element is null)
        {
            return null;
        }

        // Try standard parent resolution first
        var parent = GetVisualParent(element) ?? GetLogicalParent(element);

        // If we found a parent, return it
        if (parent is not null)
        {
            return parent;
        }

        // If no parent found and element is a FrameworkElement, 
        // check if it's the root content of a TabItem (virtualized scenario)
        if (element is FrameworkElement frameworkElement)
        {
            var tabControl = FindOwningTabControl(frameworkElement);
            if (tabControl is not null)
            {
                // Found the TabControl that owns this element, continue traversal from there
                return tabControl;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the visual parent of the specified element.
    /// </summary>
    public static DependencyObject? GetVisualParent(DependencyObject? element) =>
        element is not null ? VisualTreeHelper.GetParent(element) : null;

    /// <summary>
    /// Gets the logical parent of the specified element.
    /// </summary>
    public static DependencyObject? GetLogicalParent(DependencyObject? element) =>
        element is not null ? LogicalTreeHelper.GetParent(element) : null;

    /// <summary>
    /// Finds the TabControl that owns this element when it's in virtualized TabItem content.
    /// This is the key method that bridges the gap when visual tree is broken.
    /// </summary>
    /// <param name="element">The element that might be TabItem content</param>
    /// <returns>The owning TabControl if found, null otherwise</returns>
    public static TabControl? FindOwningTabControl(FrameworkElement element)
    {
        if (element is null)
        {
            return null;
        }

        // Strategy 1: Check if element is directly set as TabItem.Content
        var tabControl = FindTabControlByDirectContent(element);
        if (tabControl is not null)
        {
            return tabControl;
        }

        // Strategy 2: Check if element is part of a content template
        tabControl = FindTabControlByContentTemplate(element);
        if (tabControl is not null)
        {
            return tabControl;
        }

        // Strategy 3: Check DataContext relationships
        tabControl = FindTabControlByDataContext(element);
        if (tabControl is not null)
        {
            return tabControl;
        }

        return null;
    }

    /// <summary>
    /// Finds TabControl where element is directly set as TabItem.Content.
    /// </summary>
    private static TabControl? FindTabControlByDirectContent(FrameworkElement element)
    {
        var allTabControls = GetAllTabControls();

        foreach (var tabControl in allTabControls)
        {
            foreach (var item in tabControl.Items)
            {
                if (item is TabItem tabItem && ReferenceEquals(tabItem.Content, element))
                {
                    return tabControl;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Finds TabControl where element is part of a ContentTemplate.
    /// </summary>
    private static TabControl? FindTabControlByContentTemplate(FrameworkElement element)
    {
        var allTabControls = GetAllTabControls();

        foreach (var tabControl in allTabControls)
        {
            foreach (var item in tabControl.Items)
            {
                if (item is TabItem tabItem && IsElementInContentTemplate(element, tabItem))
                {
                    return tabControl;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Checks if element is part of TabItem's content template by walking up the logical tree.
    /// </summary>
    private static bool IsElementInContentTemplate(FrameworkElement element, TabItem tabItem)
    {
        // Walk up the logical tree to see if we reach the TabItem
        var current = element as DependencyObject;
        var visited = new HashSet<DependencyObject>();

        while (current is not null && visited.Add(current))
        {
            if (ReferenceEquals(current, tabItem))
            {
                return true;
            }

            // Check if current element's TemplatedParent is the TabItem
            if (current is FrameworkElement fe && ReferenceEquals(fe.TemplatedParent, tabItem))
            {
                return true;
            }

            current = GetLogicalParent(current);
        }

        return false;
    }

    /// <summary>
    /// Gets all TabControls in the application.
    /// </summary>
    private static IEnumerable<TabControl> GetAllTabControls()
    {
        var tabControls = new List<TabControl>();

        if (Application.Current is not null)
        {
            // Check main window
            if (Application.Current.MainWindow is not null)
            {
                tabControls.AddRange(GetVisualDescendants<TabControl>(Application.Current.MainWindow));
            }

            // Check all other windows
            foreach (Window window in Application.Current.Windows)
            {
                if (window != Application.Current.MainWindow)
                {
                    tabControls.AddRange(GetVisualDescendants<TabControl>(window));
                }
            }
        }

        return tabControls;
    }

    /// <summary>
    /// Finds a TabItem through the element's naming scope or templated parent.
    /// </summary>
    private static TabItem? FindTabItemFromNameScope(FrameworkElement element)
    {
        if (element is null)
        {
            return null;
        }

        // Check naming scope
        var nameScope = NameScope.GetNameScope(element);
        if (nameScope is TabItem tabItem)
        {
            return tabItem;
        }

        // Check templated parent
        if (element.TemplatedParent is TabItem templatedTabItem)
        {
            return templatedTabItem;
        }

        return null;
    }

    /// <summary>
    /// Finds the TabControl by examining DataContext relationships.
    /// </summary>
    private static TabControl? FindTabControlByDataContext(FrameworkElement element)
    {
        if (element is null)
        {
            return null;
        }

        var allTabControls = GetAllTabControls();

        foreach (var tabControl in allTabControls)
        {
            foreach (var item in tabControl.Items)
            {
                if (item is TabItem tabItem && IsDataContextRelated(element, tabItem))
                {
                    return tabControl;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the window containing the specified element.
    /// </summary>
    private static Window? GetContainingWindow(FrameworkElement element)
    {
        if (element is null)
        {
            return null;
        }

        var window = Window.GetWindow(element);
        if (window is not null)
        {
            return window;
        }

        // Fallback to application windows
        if (Application.Current is not null)
        {
            window = Application.Current.MainWindow;
            if (window is not null)
            {
                return window;
            }

            if (Application.Current.Windows.Count > 0)
            {
                return Application.Current.Windows[0];
            }
        }

        return null;
    }

    /// <summary>
    /// Finds a TabItem in the specified TabControl that contains the element.
    /// </summary>
    private static TabItem? FindTabItemByDataContextInTabControl(FrameworkElement element, TabControl tabControl)
    {
        if (element is null || tabControl is null)
        {
            return null;
        }

        foreach (var item in tabControl.Items)
        {
            if (item is TabItem tabItem)
            {
                if (IsDataContextRelated(element, tabItem) ||
                    IsElementLogicallyContainedInTabItem(element, tabItem))
                {
                    return tabItem;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Checks if the element's DataContext is related to the TabItem's DataContext.
    /// </summary>
    private static bool IsDataContextRelated(FrameworkElement element, TabItem tabItem)
    {
        if (element is null || tabItem is null)
        {
            return false;
        }

        var elementContext = element.DataContext;
        var tabItemContext = tabItem.DataContext;

        // Direct match
        if (elementContext is not null && tabItemContext is not null)
        {
            if (ReferenceEquals(elementContext, tabItemContext) || elementContext.Equals(tabItemContext))
            {
                return true;
            }
        }

        // Check if element's DataContext is the same as TabItem's bound data
        if (tabItemContext is not null && ReferenceEquals(elementContext, tabItemContext))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the element is logically contained within the TabItem.
    /// This method may temporarily force content creation.
    /// </summary>
    private static bool IsElementLogicallyContainedInTabItem(FrameworkElement element, TabItem tabItem)
    {
        if (element is null || tabItem is null)
        {
            return false;
        }

        var originalContent = tabItem.Content;

        try
        {
            // Check logical parent chain
            var logicalParent = element as DependencyObject;
            while (logicalParent is not null)
            {
                if (logicalParent == tabItem)
                {
                    return true;
                }

                logicalParent = GetLogicalParent(logicalParent);
            }

            return false;
        }
        finally
        {
            // Ensure we don't change the TabItem's state
            if (tabItem.Content != originalContent)
            {
                tabItem.SetCurrentValue(ContentControl.ContentProperty, originalContent);
            }
        }
    }

    /// <summary>
    /// Gets all visual descendants of the specified type.
    /// </summary>
    public static IEnumerable<T> GetVisualDescendants<T>(DependencyObject? parent) where T : DependencyObject
    {
        if (parent is null)
        {
            yield break;
        }

        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is T typedChild)
            {
                yield return typedChild;
            }

            foreach (var grandChild in GetVisualDescendants<T>(child))
            {
                yield return grandChild;
            }
        }
    }

    /// <summary>
    /// Gets all logical descendants of the specified type.
    /// </summary>
    public static IEnumerable<T> GetLogicalDescendants<T>(DependencyObject? parent) where T : DependencyObject
    {
        if (parent is null)
        {
            yield break;
        }

        foreach (var child in LogicalTreeHelper.GetChildren(parent).OfType<DependencyObject>())
        {
            if (child is T typedChild)
            {
                yield return typedChild;
            }

            foreach (var grandChild in GetLogicalDescendants<T>(child))
            {
                yield return grandChild;
            }
        }
    }

    /// <summary>
    /// Finds the first ancestor of the specified type.
    /// </summary>
    public static T? FindAncestor<T>(DependencyObject? element) where T : DependencyObject
    {
        var current = element;
        while (current is not null)
        {
            if (current is T ancestor)
            {
                return ancestor;
            }

            current = GetNextParent(current);
        }

        return null;
    }

    /// <summary>
    /// Finds the first descendant of the specified type.
    /// </summary>
    public static T? FindDescendant<T>(DependencyObject? parent) where T : DependencyObject =>
        GetVisualDescendants<T>(parent).FirstOrDefault() ??
        GetLogicalDescendants<T>(parent).FirstOrDefault();

    /// <summary>
    /// Gets all parent elements in the hierarchy.
    /// </summary>
    public static IEnumerable<DependencyObject> GetParentChain(DependencyObject? element)
    {
        var current = element;
        while (current is not null)
        {
            current = GetNextParent(current);
            if (current is not null)
            {
                yield return current;
            }
        }
    }

    /// <summary>
    /// Searches for elements by name in the visual tree.
    /// </summary>
    public static IEnumerable<T> FindElementsByName<T>(DependencyObject? parent, string name) where T : FrameworkElement
    {
        if (parent is null || string.IsNullOrEmpty(name))
        {
            yield break;
        }

        foreach (var element in GetVisualDescendants<T>(parent))
        {
            if (string.Equals(element.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                yield return element;
            }
        }
    }

    /// <summary>
    /// Searches for the first element by name in the visual tree.
    /// </summary>
    public static T? FindElementByName<T>(DependencyObject? parent, string name) where T : FrameworkElement =>
        FindElementsByName<T>(parent, name).FirstOrDefault();

    /// <summary>
    /// Searches for elements using a predicate function.
    /// </summary>
    public static IEnumerable<T> FindElements<T>(DependencyObject? parent, Func<T, bool> predicate)
        where T : DependencyObject
    {
        if (parent is null || predicate is null)
        {
            yield break;
        }

        foreach (var element in GetVisualDescendants<T>(parent))
        {
            if (predicate(element))
            {
                yield return element;
            }
        }
    }

    /// <summary>
    /// Searches for the first element matching a predicate function.
    /// </summary>
    public static T? FindElement<T>(DependencyObject? parent, Func<T, bool> predicate) where T : DependencyObject =>
        FindElements(parent, predicate).FirstOrDefault();

    /// <summary>
    /// Searches for elements by type in both visual and logical trees.
    /// </summary>
    public static IEnumerable<T> FindAllElements<T>(DependencyObject? parent) where T : DependencyObject
    {
        if (parent is null)
        {
            yield break;
        }

        var visualElements = GetVisualDescendants<T>(parent);
        var logicalElements = GetLogicalDescendants<T>(parent);

        // Combine and deduplicate
        var allElements = visualElements.Concat(logicalElements).Distinct();

        foreach (var element in allElements)
        {
            yield return element;
        }
    }

    /// <summary>
    /// Searches for elements with specific attached property values.
    /// </summary>
    public static IEnumerable<T> FindElementsByProperty<T>(DependencyObject? parent, DependencyProperty property,
        object? value) where T : DependencyObject
    {
        if (parent is null || property is null)
        {
            yield break;
        }

        foreach (var element in GetVisualDescendants<T>(parent))
        {
            var elementValue = element.GetValue(property);
            if (Equals(elementValue, value))
            {
                yield return element;
            }
        }
    }

    /// <summary>
    /// Searches for elements with specific attached property values.
    /// </summary>
    public static T? FindElementByProperty<T>(DependencyObject? parent, DependencyProperty property, object? value)
        where T : DependencyObject => FindElementsByProperty<T>(parent, property, value).FirstOrDefault();

    /// <summary>
    /// Searches for elements by DataContext.
    /// </summary>
    public static IEnumerable<T> FindElementsByDataContext<T>(DependencyObject? parent, object? dataContext)
        where T : FrameworkElement
    {
        if (parent is null)
        {
            yield break;
        }

        foreach (var element in GetVisualDescendants<T>(parent))
        {
            if (Equals(element.DataContext, dataContext))
            {
                yield return element;
            }
        }
    }

    /// <summary>
    /// Searches for the first element by DataContext.
    /// </summary>
    public static T? FindElementByDataContext<T>(DependencyObject? parent, object? dataContext)
        where T : FrameworkElement => FindElementsByDataContext<T>(parent, dataContext).FirstOrDefault();

    /// <summary>
    /// Searches for elements by Tag property.
    /// </summary>
    public static IEnumerable<T> FindElementsByTag<T>(DependencyObject? parent, object? tag) where T : FrameworkElement
    {
        if (parent is null)
        {
            yield break;
        }

        foreach (var element in GetVisualDescendants<T>(parent))
        {
            if (Equals(element.Tag, tag))
            {
                yield return element;
            }
        }
    }

    /// <summary>
    /// Searches for the first element by Tag property.
    /// </summary>
    public static T? FindElementByTag<T>(DependencyObject? parent, object? tag) where T : FrameworkElement =>
        FindElementsByTag<T>(parent, tag).FirstOrDefault();

    /// <summary>
    /// Searches ancestors for a specific type or condition.
    /// </summary>
    public static IEnumerable<T> FindAncestors<T>(DependencyObject? element) where T : DependencyObject
    {
        var current = element;
        while (current is not null)
        {
            current = GetNextParent(current);
            if (current is T ancestor)
            {
                yield return ancestor;
            }
        }
    }

    /// <summary>
    /// Searches ancestors using a predicate function.
    /// </summary>
    public static IEnumerable<T> FindAncestors<T>(DependencyObject? element, Func<T, bool> predicate)
        where T : DependencyObject
    {
        if (predicate is null)
        {
            yield break;
        }

        foreach (var ancestor in FindAncestors<T>(element))
        {
            if (predicate(ancestor))
            {
                yield return ancestor;
            }
        }
    }

    /// <summary>
    /// Searches for child elements at a specific depth level.
    /// </summary>
    public static IEnumerable<T> FindElementsAtDepth<T>(DependencyObject? parent, int depth) where T : DependencyObject
    {
        if (parent is null || depth < 0)
        {
            yield break;
        }

        if (depth == 0)
        {
            if (parent is T element)
            {
                yield return element;
            }

            yield break;
        }

        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            foreach (var descendant in FindElementsAtDepth<T>(child, depth - 1))
            {
                yield return descendant;
            }
        }
    }

    /// <summary>
    /// Searches for elements within a specific distance from the parent.
    /// </summary>
    public static IEnumerable<T> FindElementsWithinDistance<T>(DependencyObject? parent, int maxDistance)
        where T : DependencyObject
    {
        if (parent is null || maxDistance < 0)
        {
            yield break;
        }

        for (var depth = 0; depth <= maxDistance; depth++)
        {
            foreach (var element in FindElementsAtDepth<T>(parent, depth))
            {
                yield return element;
            }
        }
    }
}
