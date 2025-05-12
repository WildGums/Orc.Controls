namespace Orc.Controls;

using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Automation;
using Catel.Logging;
using Services;

[StyleTypedProperty(Property = nameof(TextBlockStyle), StyleTargetType = typeof(TextBlock))]
public class AnimatingTextBlock : UserControl, IStatusRepresenter
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private static readonly Storyboard DefaultShowStoryboard;
    private static readonly Storyboard DefaultHideStoryboard;

    private Grid? _contentGrid;
    private int _currentIndex = 0;

    static AnimatingTextBlock()
    {
        var showStoryboard = new Storyboard();

        var showAnimation = new DoubleAnimation
        {
            To = 1d,
            Duration = TimeSpan.FromMilliseconds(500),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };

        Storyboard.SetTargetProperty(showAnimation, new PropertyPath(nameof(TextBlock.Opacity)));

        showStoryboard.Children.Add(showAnimation);

        DefaultShowStoryboard = showStoryboard;

        var hideStoryboard = new Storyboard();

        var hideAnimation = new DoubleAnimation
        {
            To = 0d,
            Duration = TimeSpan.FromMilliseconds(500),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };

        Storyboard.SetTargetProperty(hideAnimation, new PropertyPath(nameof(TextBlock.Opacity)));

        hideStoryboard.Children.Add(hideAnimation);

        DefaultHideStoryboard = hideStoryboard;
    }

    public AnimatingTextBlock()
    {
        SetCurrentValue(ShowStoryboardProperty, DefaultShowStoryboard);
        SetCurrentValue(HideStoryboardProperty, DefaultHideStoryboard);

        Loaded += OnLoaded;
    }

    #region Dependency properties


    public Style? TextBlockStyle
    {
        get { return (Style?)GetValue(TextBlockStyleProperty); }
        set { SetValue(TextBlockStyleProperty, value); }
    }

    public static readonly DependencyProperty TextBlockStyleProperty = DependencyProperty.Register(nameof(TextBlockStyle), 
        typeof(Style), typeof(AnimatingTextBlock), new PropertyMetadata(null));



    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(AnimatingTextBlock),
        new PropertyMetadata(string.Empty, (sender, _) => ((AnimatingTextBlock)sender).OnTextChanged()));


    public TextAlignment TextAlignment
    {
        get { return (TextAlignment)GetValue(TextAlignmentProperty); }
        set { SetValue(TextAlignmentProperty, value); }
    }

    public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), 
        typeof(AnimatingTextBlock), new PropertyMetadata(TextAlignment.Left));


    public Storyboard? HideStoryboard
    {
        get { return (Storyboard?)GetValue(HideStoryboardProperty); }
        set { SetValue(HideStoryboardProperty, value); }
    }      
        
    public static readonly DependencyProperty HideStoryboardProperty =
        DependencyProperty.Register(nameof(HideStoryboard), typeof(Storyboard), typeof(AnimatingTextBlock), new PropertyMetadata());


    public Storyboard? ShowStoryboard 
    {
        get { return (Storyboard?)GetValue(ShowStoryboardProperty); }
        set { SetValue(ShowStoryboardProperty, value); }
    }    
        
    public static readonly DependencyProperty ShowStoryboardProperty =
        DependencyProperty.Register(nameof(ShowStoryboard), typeof(Storyboard), typeof(AnimatingTextBlock), new PropertyMetadata());

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
        var text = Text;
        if (string.IsNullOrEmpty(text))
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
        textBlockToShow.SetCurrentValue(TextBlock.TextProperty, text);

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

        var textBlockStyle = TextBlockStyle;

        for (var i = 0; i < 2; i++)
        {
            var textBlock = new TextBlock
            {
                HorizontalAlignment = HorizontalContentAlignment,
                VerticalAlignment = VerticalContentAlignment,
                Opacity = 0d,
                RenderTransform = renderTransform,
                TextAlignment = TextAlignment,
                TextWrapping = TextWrapping.NoWrap,
                TextTrimming = TextTrimming.CharacterEllipsis,
            };

            if (textBlockStyle is not null)
            {
                textBlock.Style = textBlockStyle;
            }

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
