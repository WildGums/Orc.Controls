namespace Orc.Controls;

using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Automation;
using Services;

public class AnimatingTextBlock : UserControl, IStatusRepresenter
{
    private Grid? _contentGrid;
    private int _currentIndex = 0;

    public AnimatingTextBlock()
    {
        Loaded += OnLoaded;
    }

    #region Dependency properties
    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(AnimatingTextBlock),
        new PropertyMetadata(string.Empty, (sender, _) => ((AnimatingTextBlock)sender).OnTextChanged()));
        
    public Storyboard? HideStoryboard
    {
        get { return (Storyboard?)GetValue(HideStoryboardProperty); }
        set { SetValue(HideStoryboardProperty, value); }
    }      
        
    public static readonly DependencyProperty HideStoryboardProperty =
        DependencyProperty.Register(nameof(HideStoryboard), typeof(Storyboard), typeof(AnimatingTextBlock), new PropertyMetadata(null));

    public Storyboard? ShowStoryboard 
    {
        get { return (Storyboard?)GetValue(ShowStoryboardProperty); }
        set { SetValue(ShowStoryboardProperty, value); }
    }    
        
    public static readonly DependencyProperty ShowStoryboardProperty =
        DependencyProperty.Register(nameof(ShowStoryboard), typeof(Storyboard), typeof(AnimatingTextBlock), new PropertyMetadata(null));

    #endregion

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        Loaded -= OnLoaded;

        ApplyTemplate();
    }

    private void OnTextChanged()
    {
        if (_contentGrid is null)
        {
            return;
        }

        var textBlockToHide = _contentGrid.Children[_currentIndex];
        if (textBlockToHide.Opacity == 0.0d)
        {
            OnHideComplete();
            return;
        }

        var hideStoryboard = HideStoryboard;
        if (hideStoryboard is not null)
        {
            hideStoryboard.Stop();
            Storyboard.SetTarget(hideStoryboard, textBlockToHide);

            hideStoryboard.Completed += OnHideStoryboardComplete;
            hideStoryboard.Begin();
        }
        else
        {
            textBlockToHide.SetCurrentValue(OpacityProperty, 0d);
            OnHideComplete();
        }
    }

    private void OnHideStoryboardComplete(object? sender, EventArgs e)
    {
        if (sender is Storyboard storyBoard)
        {
            storyBoard.Completed -= OnHideStoryboardComplete;
        }

        OnHideComplete();
    }

    private void OnHideComplete()
    {
        if (string.IsNullOrEmpty(Text))
        {
            return;
        }

        if (_contentGrid is null)
        {
            return;
        }

        _currentIndex++;
        if (_currentIndex >= _contentGrid.Children.Count)
        {
            _currentIndex = 0;
        }

        var textBlockToShow = (TextBlock)_contentGrid.Children[_currentIndex];
        textBlockToShow.SetCurrentValue(TextBlock.TextProperty, Text);

        var showStoryboard = ShowStoryboard;
        if (showStoryboard is not null)
        {
            showStoryboard.Stop();
            Storyboard.SetTarget(showStoryboard, textBlockToShow);

            showStoryboard.Completed += OnShowStoryboardComplete;
            showStoryboard.Begin();
        }
        else
        {
            textBlockToShow.SetCurrentValue(OpacityProperty, 1d);
        }
    }

    private void OnShowStoryboardComplete(object? sender, EventArgs e)
    {
        if (sender is Storyboard storyBoard)
        {
            storyBoard.Completed -= OnShowStoryboardComplete;
        }
    }

    /// <summary>
    /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        InitializeControl();
    }

    private void InitializeControl()
    {
        _contentGrid = CreateContent();
        SetCurrentValue(ContentProperty, _contentGrid);
    }

    private Grid CreateContent()
    {
        var grid = new Grid();

        var renderTransform = RenderTransform;
        SetCurrentValue(RenderTransformProperty, null);

        for (var i = 0; i < 2; i++)
        {
            var textBlock = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 0d,
                RenderTransform = renderTransform
            };

            grid.Children.Add(textBlock);
        }

        return grid;
    }

    /// <summary>
    /// Updates the status.
    /// </summary>
    /// <param name="status">The status.</param>
    public void UpdateStatus(string status)
    {
        Dispatcher.BeginInvoke(() => SetCurrentValue(TextProperty, status));
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new AnimatingTextBlockAutomationPeer(this);
    }
}
