﻿namespace Orc.Controls;

using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media;
using Automation;

/// <summary>
/// Interaction logic for BusyIndicator.xaml
/// </summary>
public partial class BusyIndicator
{
    private MediaElementThreadInfo? _mediaElementThreadInfo;
    private Grid? _grid;
    private Brush? _fluidProgressBarForeground;
    private FluidProgressBar? _fluidProgressBar;

    private int _ignoreUnloadedEventCounter;

    /// <summary>
    /// Initializes a new instance of the <see cref="BusyIndicator"/> class.
    /// </summary>
    public BusyIndicator()
    {
        InitializeComponent();

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    public Brush Foreground
    {
        get { return (Brush)GetValue(ForegroundProperty); }
        set { SetValue(ForegroundProperty, value); }
    }

    public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
        nameof(Foreground), typeof(Brush), typeof(BusyIndicator),
        new PropertyMetadata(Brushes.White, (sender, _) => ((BusyIndicator)sender).OnForegroundChanged()));

    public int IgnoreUnloadedEventCount
    {
        get { return (int)GetValue(IgnoreUnloadedEventCountProperty); }
        set { SetValue(IgnoreUnloadedEventCountProperty, value); }
    }

    public static readonly DependencyProperty IgnoreUnloadedEventCountProperty = DependencyProperty.Register(nameof(IgnoreUnloadedEventCount),
        typeof(int), typeof(BusyIndicator), new PropertyMetadata(0, (sender, _) => ((BusyIndicator)sender).OnIgnoreUnloadedEventCountChanged()));

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (_mediaElementThreadInfo is not null)
        {
            return;
        }

        _mediaElementThreadInfo = MediaElementThreadFactory.CreateMediaElementsOnWorkerThread(CreateBusyIndicator);
        Child = _mediaElementThreadInfo.HostVisual;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (_ignoreUnloadedEventCounter > 0)
        {
            _ignoreUnloadedEventCounter--;
            return;
        }

        if (_mediaElementThreadInfo is not null)
        {
            _mediaElementThreadInfo.Dispose();
            _mediaElementThreadInfo = null;
        }
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);

        if (_grid is not null && sizeInfo.WidthChanged)
        {
            _grid.Dispatcher.BeginInvoke(() =>
            {
                _grid.SetCurrentValue(WidthProperty, sizeInfo.NewSize.Width);
            });
        }
    }

    private void OnIgnoreUnloadedEventCountChanged()
    {
        _ignoreUnloadedEventCounter = IgnoreUnloadedEventCount;
    }

    private void OnForegroundChanged()
    {
        _fluidProgressBarForeground = Foreground;
            
        _mediaElementThreadInfo?.Dispatcher?.Invoke(() =>
        {
            _fluidProgressBar?.SetCurrentValue(Control.ForegroundProperty, _fluidProgressBarForeground);
        });
    }

    private FrameworkElement CreateBusyIndicator()
    {
        _fluidProgressBar = new FluidProgressBar
        {
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = _fluidProgressBarForeground
        };

        var grid = new Grid
        {
            Width = ActualWidth,
            Height = ActualHeight,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center
        };
        grid.Children.Add(_fluidProgressBar);

        _grid = grid;

        return grid;
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new BusyIndicatorAutomationPeer(this);
    }
}
