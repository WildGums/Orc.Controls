namespace Orc.Controls;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Catel.MVVM;
using Catel.Windows;
using Catel.Windows.Interactivity;

public class InfinityScrollListBoxBehavior : BehaviorBase<ListBox>
{
    private ScrollViewer? _scrollViewer;

    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(nameof(Command), typeof(TaskCommand), typeof(InfinityScrollListBoxBehavior), new PropertyMetadata(null));

    public TaskCommand Command
    {
        get { return (TaskCommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(InfinityScrollListBoxBehavior), new PropertyMetadata(0));

    public object CommandParameter
    {
        get { return GetValue(CommandParameterProperty); }
        set { SetValue(CommandParameterProperty, value); }
    }

    public static readonly DependencyProperty IsCommandExecutingProperty =
        DependencyProperty.RegisterAttached(nameof(IsCommandExecuting), typeof(bool), typeof(InfinityScrollListBoxBehavior),
            new PropertyMetadata(false));

    public bool IsCommandExecuting
    {
        get { return (bool)GetValue(IsCommandExecutingProperty); }
        set { SetValue(IsCommandExecutingProperty, value); }
    }

    public static readonly DependencyProperty ScrollSizeProperty =
        DependencyProperty.RegisterAttached(nameof(ScrollSize), typeof(int), typeof(InfinityScrollListBoxBehavior), new PropertyMetadata(0));

    public int ScrollSize
    {
        get { return (int)GetValue(ScrollSizeProperty); }
        set { SetValue(ScrollSizeProperty, value); }
    }
    
    protected override void OnAssociatedObjectLoaded()
    {
        FindVisualScroll();
    }

    protected override void OnAssociatedObjectUnloaded()
    {
        if (_scrollViewer is not null)
        {
            _scrollViewer.ScrollChanged -= OnScrollViewerScrollChanged;
        }
    }

    private void FindVisualScroll()
    {
        if (_scrollViewer is not null)
        {
            _scrollViewer.ScrollChanged -= OnScrollViewerScrollChanged;
        }

        _scrollViewer = AssociatedObject.FindVisualDescendantByType<ScrollViewer>();

        if (_scrollViewer is not null)
        {
            _scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
        }
    }

    private async void OnScrollViewerScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (_scrollViewer is null)
        {
            return;
        }

        var scrolled = _scrollViewer.VerticalOffset;
        var last = _scrollViewer.ViewportHeight + scrolled;

        if (ScrollSize > last)
        {
            return;
        }

        if (_scrollViewer.ViewportHeight > 0 && last >= AssociatedObject.Items.Count)
        {
            await ExecuteLoadingItemsCommandAsync();
            _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset);
        }
    }

    private async Task ExecuteLoadingItemsCommandAsync()
    {
        Command?.Execute(CommandParameter);
    }
}
