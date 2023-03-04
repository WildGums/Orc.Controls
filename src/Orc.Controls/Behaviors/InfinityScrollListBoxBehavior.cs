namespace Orc.Controls;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Catel.MVVM;
using Catel.Windows;
using Catel.Windows.Interactivity;

public class InfinityScrollListBoxBehavior : BehaviorBase<ListBox>
{
    #region Constants
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(nameof(Command), typeof(TaskCommand), typeof(InfinityScrollListBoxBehavior), new PropertyMetadata(null));

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(InfinityScrollListBoxBehavior), new PropertyMetadata(0));

    public static readonly DependencyProperty IsCommandExecutingProperty =
        DependencyProperty.RegisterAttached(nameof(IsCommandExecuting), typeof(bool), typeof(InfinityScrollListBoxBehavior),
            new PropertyMetadata(false));

    public static readonly DependencyProperty ScrollSizeProperty =
        DependencyProperty.RegisterAttached(nameof(ScrollSize), typeof(int), typeof(InfinityScrollListBoxBehavior), new PropertyMetadata(0));
    #endregion

    #region Fields
    private ScrollViewer _scrollViewer;
    #endregion

    #region Properties
    private ScrollViewer ScrollViewer
    {
        get { return _scrollViewer; }
        set
        {
            if (value != _scrollViewer)
            {
                if (_scrollViewer is not null)
                {
                    _scrollViewer.ScrollChanged -= OnScrollViewerScrollChanged;
                }

                _scrollViewer = value;

                if (_scrollViewer is not null)
                {
                    _scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
                }
            }
        }
    }

    public TaskCommand Command
    {
        get { return (TaskCommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    public object CommandParameter
    {
        get { return GetValue(CommandParameterProperty); }
        set { SetValue(CommandParameterProperty, value); }
    }

    public bool IsCommandExecuting
    {
        get { return (bool)GetValue(IsCommandExecutingProperty); }
        set { SetValue(IsCommandExecutingProperty, value); }
    }

    public int ScrollSize
    {
        get { return (int)GetValue(ScrollSizeProperty); }
        set { SetValue(ScrollSizeProperty, value); }
    }
    #endregion

    #region Methods
    protected override void OnAssociatedObjectLoaded()
    {
        FindVisualScroll();
    }

    protected override void OnAssociatedObjectUnloaded()
    {
        base.OnAssociatedObjectUnloaded();
    }

    private void FindVisualScroll()
    {
        ScrollViewer = AssociatedObject.FindVisualDescendantByType<ScrollViewer>();
    }

    private async void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
    {
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
    #endregion
}
