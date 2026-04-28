namespace Orc.Controls;

using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media;
using Automation;

/// <summary>
/// A counter to show the frame number inside an application.
/// </summary>
public partial class FrameCounter : TextBlock
{
    private int _frameCounter;

    // Note: we cache dependency properties for performance
    private int _resetCountThreadSafe;
    private string _prefixThreadSafe;

    public FrameCounter()
    {
        Loaded += OnControlLoaded;
        Unloaded += OnControlUnloaded;

        _resetCountThreadSafe = ResetCount;
        _prefixThreadSafe = Prefix;   
    }

    public string Prefix
    {
        get { return (string)GetValue(PrefixProperty); }
        set { SetValue(PrefixProperty, value); }
    }

    public static readonly DependencyProperty PrefixProperty = DependencyProperty.Register(nameof(Prefix),
        typeof(string), typeof(FrameCounter), new PropertyMetadata("Frame no: ", (sender, e) => ((FrameCounter)sender)._prefixThreadSafe = (string)e.NewValue));


    public int ResetCount
    {
        get { return (int)GetValue(ResetCountProperty); }
        set { SetValue(ResetCountProperty, value); }
    }

    public static readonly DependencyProperty ResetCountProperty = DependencyProperty.Register(nameof(ResetCount), 
        typeof(int), typeof(FrameCounter), new PropertyMetadata(1000, (sender, e) => ((FrameCounter)sender)._resetCountThreadSafe = (int)e.NewValue));


    private void OnControlLoaded(object _, RoutedEventArgs e)
    {
        CompositionTarget.Rendering += OnRendering;
    }

    private void OnControlUnloaded(object _, RoutedEventArgs e)
    {
        CompositionTarget.Rendering -= OnRendering;
    }

    private void OnRendering(object? _, EventArgs e)
    {
        _frameCounter++;

        if (_frameCounter > _resetCountThreadSafe)
        {
            _frameCounter = 0;
        }

        Update();
    }

    private void Update()
    {
        var text = $"{_prefixThreadSafe}{_frameCounter}";

        SetCurrentValue(TextProperty, text);
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new FrameCounterAutomationPeer(this);
    }
}
