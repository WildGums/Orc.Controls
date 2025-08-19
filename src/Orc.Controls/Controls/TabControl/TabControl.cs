namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Catel.Windows.Data;

/// <summary>
/// TabControl that will not remove the tab items from the visual tree. This way, views can be re-used.
/// Now supports tear-off functionality for tabs.
/// </summary>
[TemplatePart(Name = "PART_ItemsHolder", Type = typeof(Panel))]
public class TabControl : System.Windows.Controls.TabControl
{
    /// <summary>
    /// Dependency property registration for the <see cref="LoadTabItems"/> property.
    /// </summary>
    public static readonly DependencyProperty LoadTabItemsProperty = DependencyProperty.Register(nameof(LoadTabItems),
        typeof(LoadTabItemsBehavior), typeof(TabControl), new PropertyMetadata(LoadTabItemsBehavior.LazyLoading,
            (sender, e) => ((TabControl)sender).OnLoadTabItemsChanged()));

    /// <summary>
    /// Dependency property for enabling tear-off functionality
    /// </summary>
    public static readonly DependencyProperty AllowTearOffProperty =
        DependencyProperty.Register(nameof(AllowTearOff), typeof(bool), typeof(TabControl),
            new PropertyMetadata(false));

    private readonly ConditionalWeakTable<object, object> _wrappedContainers = new ConditionalWeakTable<object, object>();
    private readonly List<TearOffWindow> _tearOffWindows = new List<TearOffWindow>();
    private Panel? _itemsHolder;

    /// <summary>
    /// Event raised when a tab is torn off
    /// </summary>
    public static readonly RoutedEvent TabTornOffEvent =
        EventManager.RegisterRoutedEvent(nameof(TabTornOff), RoutingStrategy.Bubble,
            typeof(TearOffEventHandler), typeof(TabControl));

    /// <summary>
    /// Event raised when a tab is docked back
    /// </summary>
    public static readonly RoutedEvent TabDockedEvent =
        EventManager.RegisterRoutedEvent(nameof(TabDocked), RoutingStrategy.Bubble,
            typeof(TearOffEventHandler), typeof(TabControl));

    /// <summary>
    /// Initializes a new instance of the <see cref="TabControl"/>.class.
    /// </summary>
    static TabControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(typeof(TabControl)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Windows.Controls.TabControl"/>.class.
    /// </summary>
    /// <remarks></remarks>
    public TabControl()
    {
        // this is necessary so that we get the initial databound selected item
        ItemContainerGenerator.StatusChanged += OnItemContainerGeneratorStatusChanged;
        Loaded += OnTabControlLoaded;

        this.SubscribeToDependencyProperty(nameof(TabControl.SelectedItem), OnSelectedItemChanged);

        // Subscribe to tear-off events
        AddHandler(TearOffTabItem.TabTornOffEvent, new TearOffEventHandler(OnTabTornOff));
        AddHandler(TearOffTabItem.TabDockedEvent, new TearOffEventHandler(OnTabDocked));
    }

    /// <summary>
    /// Gets or sets whether tabs can be torn off from this control
    /// </summary>
    public bool AllowTearOff
    {
        get => (bool)GetValue(AllowTearOffProperty);
        set => SetValue(AllowTearOffProperty, value);
    }

    /// <summary>
    /// Gets or sets the load tab items.
    /// <para />
    /// The default value is <see cref="LoadTabItemsBehavior.LazyLoading"/>.
    /// </summary>
    /// <value>
    /// The load tab items.
    /// </value>
    public LoadTabItemsBehavior LoadTabItems
    {
        get { return (LoadTabItemsBehavior)GetValue(LoadTabItemsProperty); }
        set { SetValue(LoadTabItemsProperty, value); }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this tab control uses any of the lazy loading options.
    /// </summary>
    /// <value><c>true</c> if this instance is lazy loading; otherwise, <c>false</c>.</value>
    public bool IsLazyLoading
    {
        get
        {
            var loadTabItems = LoadTabItems;
            return loadTabItems is LoadTabItemsBehavior.LazyLoading or LoadTabItemsBehavior.LazyLoadingUnloadOthers;
        }
    }

    /// <summary>
    /// Event raised when a tab is torn off
    /// </summary>
    public event TearOffEventHandler TabTornOff
    {
        add => AddHandler(TabTornOffEvent, value);
        remove => RemoveHandler(TabTornOffEvent, value);
    }

    /// <summary>
    /// Event raised when a tab is docked back
    /// </summary>
    public event TearOffEventHandler TabDocked
    {
        add => AddHandler(TabDockedEvent, value);
        remove => RemoveHandler(TabDockedEvent, value);
    }

    /// <summary>
    /// Gets the currently torn-off tab items
    /// </summary>
    public IEnumerable<TearOffTabItem> TornOffTabs =>
        Items.OfType<TearOffTabItem>().Where(tab => tab.IsTornOff);

    /// <summary>
    /// Creates or identifies the element that is used to display the given item.
    /// </summary>
    /// <returns>The element that is used to display the given item.</returns>
    protected override DependencyObject GetContainerForItemOverride()
    {
        var container = AllowTearOff ? new TearOffTabItem() : new TabItem();

        if (container is TearOffTabItem tearOffTabItem)
        {
            tearOffTabItem.CanTearOff = AllowTearOff;
        }

        return container;
    }

    /// <summary>
    /// Determines if the specified item is (or is eligible to be) its own container.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns>true if the item is (or is eligible to be) its own container; otherwise, false.</returns>
    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is TabItem;
    }

    private void OnTabTornOff(object sender, TearOffEventArgs e)
    {
        // Raise the event on the TabControl
        var args = new TearOffEventArgs(TabTornOffEvent, e.TabItem);
        RaiseEvent(args);

        // Update selection if the torn-off tab was selected
        if (e.TabItem.IsSelected)
        {
            SelectNextAvailableTab();
        }
    }

    private void OnTabDocked(object sender, TearOffEventArgs e)
    {
        // Raise the event on the TabControl
        var args = new TearOffEventArgs(TabDockedEvent, e.TabItem);
        RaiseEvent(args);

        //Select docked tab (give it a time)
        new PostponeDispatcherOperation(() => e.TabItem.SetCurrentValue(TabItem.IsSelectedProperty, true))
            .Execute(500);

        // Update the items holder
        InitializeItems();
    }

    private void SelectNextAvailableTab()
    {
        var availableTabs = Items.OfType<TabItem>()
            .Where(tab => tab.Visibility == Visibility.Visible &&
                         tab is not TearOffTabItem { IsTornOff: true })
            .ToList();

        if (availableTabs.Any())
        {
            availableTabs.First().SetCurrentValue(TabItem.IsSelectedProperty, true);
        }
    }

    /// <summary>
    /// Docks all currently torn-off tabs back to the control
    /// </summary>
    public void DockAllTabs()
    {
        foreach (var tornTab in TornOffTabs.ToList())
        {
            tornTab.DockBack();
        }
    }

    /// <summary>
    /// Called when the tab control is loaded.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
    private void OnTabControlLoaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnTabControlLoaded;

        InitializeItems();
    }

    /// <summary>
    /// If containers are done, generate the selected item.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void OnItemContainerGeneratorStatusChanged(object? sender, EventArgs e)
    {
        if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
        {
            ItemContainerGenerator.StatusChanged -= OnItemContainerGeneratorStatusChanged;

            InitializeItems();
        }
    }

    /// <summary>
    /// Get the ItemsHolder and generate any children.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _itemsHolder = GetTemplateChild("PART_ItemsHolder") as Panel;

        InitializeItems();
    }

    /// <summary>
    /// Eager loads the tab item at the specified index.
    /// </summary>
    /// <param name="index">the index of the tab item to load.</param>
    public virtual void LoadTabItem(int index)
    {
        if (_itemsHolder?.Children[index] is not ContentPresenter child)
        {
            return;
        }

        LoadTabItem(child);
    }

    /// <summary>
    /// Eager loads the specified tab item.
    /// </summary>
    public virtual void LoadTabItem(ContentPresenter tabItem)
    {
        EagerLoadTab(tabItem);
    }

    private void OnLoadTabItemsChanged()
    {
        InitializeItems();
    }

    /// <summary>
    /// When the items change we remove any generated panel children and add any new ones as necessary
    /// </summary>
    /// <param name="e">The event data for the <see cref="E:System.Windows.Controls.ItemContainerGenerator.ItemsChanged"/> event.</param>
    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnItemsChanged(e);

        if (_itemsHolder is null)
        {
            return;
        }

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Reset:
                _itemsHolder.Children.Clear();
                break;

            case NotifyCollectionChangedAction.Add:
            case NotifyCollectionChangedAction.Remove:
            case NotifyCollectionChangedAction.Replace:
                {
                    if (e.OldItems is not null)
                    {
                        foreach (var item in e.OldItems)
                        {
                            var cp = FindChildContentPresenter(item);
                            if (cp is not null)
                            {
                                _itemsHolder.Children.Remove(cp);
                            }
                        }
                    }

                    // don't do anything with new items because we don't want to
                    // create visuals that aren't being shown
                    break;
                }
        }

        InitializeItems();
    }

    private void OnSelectedItemChanged(object? sender, DependencyPropertyValueChangedEventArgs e)
    {
        UpdateItems();
    }

    private void InitializeItems()
    {
        if (_itemsHolder is null)
        {
            return;
        }

        if (ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
        {
            return;
        }

        foreach (var item in Items)
        {
            if (item is not null)
            {
                CreateChildContentPresenter(item);
            }
        }

        if (LoadTabItems == LoadTabItemsBehavior.EagerLoading || IsLoaded && !IsLazyLoading)
        {
            EagerInitializeAllTabs();
        }

        if (SelectedItem is not null)
        {
            UpdateItems();
        }
    }

    private void EagerInitializeAllTabs()
    {
        if (_itemsHolder is null)
        {
            return;
        }

        foreach (ContentPresenter child in _itemsHolder.Children)
        {
            InitializeTab(child);
        }
    }

    private void InitializeTab(ContentPresenter child)
    {
        var tabControlItemData = child.Tag as TabControlItemData;
        if (tabControlItemData is null)
        {
            return;
        }

        var tabItem = tabControlItemData.TabItem;
        if (tabItem is null)
        {
            return;
        }

        ShowChildContent(child, tabControlItemData);

        // Collapsed is hidden + not loaded
#pragma warning disable WPF0041 // Set mutable dependency properties using SetCurrentValue.
        child.Visibility = Visibility.Collapsed;
#pragma warning restore WPF0041 // Set mutable dependency properties using SetCurrentValue.

        if (LoadTabItems == LoadTabItemsBehavior.EagerLoading)
        {
            EagerLoadAllTabs();
        }
    }

    private void EagerLoadAllTabs()
    {
        if (_itemsHolder is null)
        {
            return;
        }

        foreach (ContentPresenter child in _itemsHolder.Children)
        {
            EagerLoadTab(child);
        }
    }

    private void EagerLoadTab(ContentPresenter child)
    {
        if (child.Tag is not TabControlItemData tabControlItemData)
        {
            return;
        }

        var tabItem = tabControlItemData.TabItem;
        if (tabItem is not null)
        {
            ShowChildContent(child, tabControlItemData);
        }

        // Always start invisible, the selection will take care of visibility
#pragma warning disable WPF0041 // Set mutable dependency properties using SetCurrentValue.
        child.Visibility = Visibility.Hidden;
#pragma warning restore WPF0041 // Set mutable dependency properties using SetCurrentValue.
    }

    private void UpdateItems()
    {
        if (_itemsHolder is null)
        {
            return;
        }

        if (SelectedItem is not null && !IsLazyLoading)
        {
            EagerLoadAllTabs();
        }

        // Show the right child first (to prevent flickering)
        var itemsToHide = ShowItems();

        // Now hide so we have prevented flickering
        foreach (var itemToHide in itemsToHide)
        {
            var child = itemToHide;

            // Note: hidden, not collapsed otherwise items will not be loaded
#pragma warning disable WPF0041 // Set mutable dependency properties using SetCurrentValue.
            child.Key.Visibility = Visibility.Hidden;
#pragma warning restore WPF0041 // Set mutable dependency properties using SetCurrentValue.

            if (LoadTabItems == LoadTabItemsBehavior.LazyLoadingUnloadOthers)
            {
                HideChildContent(child.Key, child.Value);
            }
        }
    }

    private Dictionary<ContentPresenter, TabControlItemData> ShowItems()
    {
        var itemsToHide = new Dictionary<ContentPresenter, TabControlItemData>();

        if (_itemsHolder is null)
        {
            return itemsToHide;
        }

        foreach (ContentPresenter child in _itemsHolder.Children)
        {
            if (child.Tag is not TabControlItemData tabControlItemData)
            {
                continue;
            }

            var tabItem = tabControlItemData.TabItem;
            if (tabItem is not null && tabItem.IsSelected)
            {
                // Don't show content for torn-off tabs
                if (tabItem is TearOffTabItem tearOffTabItem && tearOffTabItem.IsTornOff)
                {
                    itemsToHide.Add(child, tabControlItemData);
                    continue;
                }

                if (child.Content is null)
                {
                    ShowChildContent(child, tabControlItemData);
                }

#pragma warning disable WPF0041 // Set mutable dependency properties using SetCurrentValue.
                child.Visibility = Visibility.Visible;
#pragma warning restore WPF0041 // Set mutable dependency properties using SetCurrentValue.
            }
            else
            {
                itemsToHide.Add(child, tabControlItemData);
            }
        }

        return itemsToHide;
    }

    /// <summary>
    /// Create the child ContentPresenter for the given item (could be data or a TabItem)
    /// </summary>
    /// <param name="item">The item.</param>
    private void CreateChildContentPresenter(object item)
    {
        if (_itemsHolder is null)
        {
            return;
        }

        if (_wrappedContainers.TryGetValue(item, out _))
        {
            return;
        }

        _wrappedContainers.Add(item, new object());

        var cp = FindChildContentPresenter(item);
        if (cp is not null)
        {
            return;
        }

        // the actual child to be added.  cp.Tag is a reference to the TabItem
        cp = new ContentPresenter();

        var container = GetContentContainer(item);
        var content = GetContent(item);

        var tabItemData = new TabControlItemData(container, content, ContentTemplate, item);

#pragma warning disable WPF0041 // Set mutable dependency properties using SetCurrentValue.
        cp.Tag = tabItemData;
#pragma warning restore WPF0041 // Set mutable dependency properties using SetCurrentValue.

#pragma warning disable WPF0041 // Set mutable dependency properties using SetCurrentValue.
        cp.ContentTemplateSelector = ContentTemplateSelector;
        cp.ContentStringFormat = SelectedContentStringFormat;
#pragma warning restore WPF0041 // Set mutable dependency properties using SetCurrentValue.

        _itemsHolder.Children.Add(cp);

        if (!IsLazyLoading)
        {
            ShowChildContent(cp, tabItemData);
        }
    }

    private object GetContent(object item)
    {
        if (item is TabItem itemAsTabItem)
        {
            return itemAsTabItem.Content;
        }

        return item;
    }

    private object GetContentContainer(object item)
    {
        if (item is TabItem itemAsTabItem)
        {
            return itemAsTabItem;
        }

        return ItemContainerGenerator.ContainerFromItem(item);
    }

    private void ShowChildContent(ContentPresenter child, TabControlItemData tabControlItemData)
    {
        child.Content ??= tabControlItemData.Content;
        child.ContentTemplate ??= tabControlItemData.ContentTemplate;

        var tabItem = tabControlItemData.TabItem;
        if (tabItem is not null)
        {
            tabItem.Content = child;
        }
    }

    private void HideChildContent(ContentPresenter child, TabControlItemData tabControlItemData)
    {
        child.SetCurrentValue(ContentPresenter.ContentProperty, null);
        child.SetCurrentValue(ContentPresenter.ContentTemplateProperty, null);

        var tabItem = tabControlItemData.TabItem;
        if (tabItem is not null)
        {
            tabItem.Content = null;
        }
    }

    /// <summary>
    /// Find the CP for the given object.  data could be a TabItem or a piece of data.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    private ContentPresenter? FindChildContentPresenter(object data)
    {
        if (data is TabItem dataAsTabItem)
        {
            data = dataAsTabItem.Content;
        }

        return _itemsHolder?.Children.Cast<ContentPresenter>()
            .FirstOrDefault(cp => ReferenceEquals(((TabControlItemData)cp.Tag).Item, data));
    }

    /// <summary>
    /// Copied from TabControl; wish it were protected in that class instead of private.
    /// </summary>
    /// <returns></returns>
    protected TabItem? GetSelectedTabItem()
    {
        var selectedItem = SelectedItem;
        if (selectedItem is null)
        {
            return null;
        }

        return selectedItem as TabItem ?? ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as TabItem;
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new Orc.Automation.TabControlAutomationPeer(this);
    }
}
