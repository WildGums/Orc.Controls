namespace Orc.Controls;

using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Automation;
using ControlzEx.Theming;
using Theming;

public class AlignmentGrid : ContentControl
{
    #region Fields
    private readonly Canvas _containerCanvas = new();
    private readonly ControlzEx.Theming.ThemeManager _themeManager;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="AlignmentGrid"/> class.
    /// </summary>
    public AlignmentGrid()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        SizeChanged += OnAlignmentGridSizeChanged;

        IsHitTestVisible = false;

        HorizontalContentAlignment = HorizontalAlignment.Stretch;
        VerticalContentAlignment = VerticalAlignment.Stretch;
        Content = _containerCanvas;

        _themeManager = ControlzEx.Theming.ThemeManager.Current;
    }

    #endregion

    #region Dependency properties
    /// <summary>
    /// Gets or sets the step to use horizontally.
    /// </summary>
    public Brush? LineBrush
    {
        get { return (Brush?)GetValue(LineBrushProperty); }
        set { SetValue(LineBrushProperty, value); }
    }

    public static readonly DependencyProperty LineBrushProperty = DependencyProperty.Register(nameof(LineBrush),
        typeof(Brush), typeof(AlignmentGrid), new PropertyMetadata(null, (sender, e) => ((AlignmentGrid)sender).Rebuild()));

    /// <summary>
    /// Gets or sets the step to use horizontally.
    /// </summary>
    public double HorizontalStep
    {
        get { return (double)GetValue(HorizontalStepProperty); }
        set { SetValue(HorizontalStepProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="HorizontalStep"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HorizontalStepProperty = DependencyProperty.Register(nameof(HorizontalStep),
        typeof(double), typeof(AlignmentGrid), new PropertyMetadata(20.0, (sender, e) => ((AlignmentGrid)sender).Rebuild()));


    /// <summary>
    /// Gets or sets the step to use horizontally.
    /// </summary>
    public double VerticalStep
    {
        get { return (double)GetValue(VerticalStepProperty); }
        set { SetValue(VerticalStepProperty, value); }
    }

    /// <summary>
    /// Identifies the <see cref="VerticalStep"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty VerticalStepProperty = DependencyProperty.Register(nameof(VerticalStep),
        typeof(double), typeof(AlignmentGrid), new PropertyMetadata(20.0, (sender, e) => ((AlignmentGrid)sender).Rebuild()));
    #endregion

    #region Methods
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Rebuild();

        _themeManager.ThemeChanged += OnThemeManagerThemeChanged;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        _themeManager.ThemeChanged -= OnThemeManagerThemeChanged;
    }

    private void OnThemeManagerThemeChanged(object? sender, ThemeChangedEventArgs e)
    {
        Rebuild();
    }

    private void Rebuild()
    {
        _containerCanvas.Children.Clear();

        var horizontalStep = HorizontalStep;
        var verticalStep = VerticalStep;
        var brush = LineBrush ?? Theming.ThemeManager.Current.GetThemeColorBrush(ThemeColorStyle.AccentColor20);

        for (double x = 0; x < ActualWidth; x += horizontalStep)
        {
            var line = new Rectangle
            {
                Width = 1,
                Height = ActualHeight,
                Fill = brush
            };

            Canvas.SetLeft(line, x);

            _containerCanvas.Children.Add(line);
        }

        for (double y = 0; y < ActualHeight; y += verticalStep)
        {
            var line = new Rectangle
            {
                Width = ActualWidth, 
                Height = 1,
                Fill = brush
            };

            Canvas.SetTop(line, y);

            _containerCanvas.Children.Add(line);
        }
    }

    private void OnAlignmentGridSizeChanged(object sender, SizeChangedEventArgs e)
    {
        Rebuild();
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new AlignmentGridAutomationPeer(this);
    }
    #endregion
}
