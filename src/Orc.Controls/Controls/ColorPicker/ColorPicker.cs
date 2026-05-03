namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Automation;
using Catel.Logging;
using Microsoft.Extensions.Logging;

/// <summary>
/// The color picker.
/// </summary>
[TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
public partial class ColorPicker : Control
{
    private readonly ILogger<ColorPicker> _logger;

    /// <summary>
    /// The color board.
    /// </summary>
    private ColorBoard? _colorBoard;

    /// <summary>
    /// The popup.
    /// </summary>
    private Popup? _popup;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorPicker"/> class.
    /// </summary>
    public ColorPicker(ILogger<ColorPicker> logger)
    {
        _logger = logger;

        DefaultStyleKey = typeof(ColorPicker);
    }

    /// <summary>
    /// Gets or sets the color.
    /// </summary>
    public Color Color
    {
        get { return (Color)GetValue(ColorProperty); }
        set { SetValue(ColorProperty, value); }
    }

    /// <summary>
    /// The color property.
    /// </summary>
    public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorPicker),
        new PropertyMetadata(Colors.White, (sender, args) => ((ColorPicker)sender).OnColorChanged(args)));
    
    /// <summary>
    /// Gets or sets the current color.
    /// </summary>
    public Color CurrentColor
    {
        get { return (Color)GetValue(CurrentColorProperty); }
        set { SetValue(CurrentColorProperty, value); }
    }

    /// <summary>
    /// The current color property.
    /// </summary>
    public static readonly DependencyProperty CurrentColorProperty = DependencyProperty.Register(
        nameof(CurrentColor), typeof(Color), typeof(ColorPicker), new PropertyMetadata(Colors.White));

    /// <summary>
    /// Gets or sets a value indicating whether is drop down open.
    /// </summary>
    public bool IsDropDownOpen
    {
        get { return (bool)GetValue(IsDropDownOpenProperty); }
        set { SetValue(IsDropDownOpenProperty, value); }
    }

    /// <summary>
    /// The is drop down open property.
    /// </summary>
    public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
        nameof(IsDropDownOpen),
        typeof(bool),
        typeof(ColorPicker),
        new PropertyMetadata(false));

    // Add to ColorPicker class
    public double PopupScale
    {
        get => (double)GetValue(PopupScaleProperty);
        set => SetValue(PopupScaleProperty, value);
    }

    public static readonly DependencyProperty PopupScaleProperty = DependencyProperty.Register(
        nameof(PopupScale), typeof(double), typeof(ColorPicker),
        new(1.0, (sender, args) => ((ColorPicker)sender).OnPopupScaleChanged(args)));

    private void OnPopupScaleChanged(DependencyPropertyChangedEventArgs e)
    {
        UpdateColorBoardScale();
    }

    /// <summary>
    /// Gets or sets the custom theme colors to be displayed in the color picker.
    /// </summary>
    public IEnumerable<Color>? CustomThemeColors
    {
        get => (IEnumerable<Color>?)GetValue(CustomThemeColorsProperty);
        set => SetValue(CustomThemeColorsProperty, value);
    }

    /// <summary>
    /// The custom theme colors property.
    /// </summary>
    public static readonly DependencyProperty CustomThemeColorsProperty = DependencyProperty.Register(
        nameof(CustomThemeColors), typeof(IEnumerable<Color>), typeof(ColorPicker),
        new(null, (sender, args) => ((ColorPicker)sender).OnCustomThemeColorsChanged(args)));

    private void OnCustomThemeColorsChanged(DependencyPropertyChangedEventArgs e)
    {
        UpdateCustomThemeColors();
    }

    /// <summary>
    /// Gets or sets the popup placement.
    /// </summary>
    public PlacementMode PopupPlacement
    {
        get => (PlacementMode)GetValue(PopupPlacementProperty);
        set => SetValue(PopupPlacementProperty, value);
    }

    /// <summary>
    /// The popup placement property.
    /// </summary>
    public static readonly DependencyProperty PopupPlacementProperty = DependencyProperty.Register(
        nameof(PopupPlacement), typeof(PlacementMode), typeof(ColorPicker), new PropertyMetadata(PlacementMode.Bottom));

    [MemberNotNullWhen(true, nameof(_popup), nameof(_colorBoard))]
    private bool IsPartsInitialized { get; set; }

    /// <summary>
    /// The color changed.
    /// </summary>
    public event EventHandler<ColorChangedEventArgs>? ColorChanged;

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _popup = GetTemplateChild("PART_Popup") as Popup;
        if (_popup is null)
        {
            throw _logger.LogErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_Popup'");
        }

        _colorBoard = new();
        _colorBoard.SetCurrentValue(ColorBoard.ColorProperty, Color);
        _colorBoard.SizeChanged += OnColorBoardSizeChanged;

        _popup.SetCurrentValue(Popup.ChildProperty, _colorBoard);
        _colorBoard.DoneClicked += OnColorBoardDoneClicked;
        _colorBoard.CancelClicked += OnColorBoardCancelClicked;

        var b = new Binding(nameof(ColorBoard.Color))
        {
            Mode = BindingMode.TwoWay,
            Source = _colorBoard
        };
        SetBinding(CurrentColorProperty, b);

        KeyDown += OnColorPickerKeyDown;

        IsPartsInitialized = true;

        // Pass any custom theme colors to the ColorBoard
        if (CustomThemeColors is not null)
        {
            UpdateCustomThemeColors();
        }

        UpdateColorBoardScale();
    }

    private void OnColorChanged(DependencyPropertyChangedEventArgs e)
    {
        RaiseColorChanged((Color)e.NewValue, (Color)e.OldValue);
    }

    /// <summary>
    /// The color picker_ key down.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnColorPickerKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && IsDropDownOpen && IsPartsInitialized)
        {
            _colorBoard.OnDoneClicked();
        }
    }

    /// <summary>
    /// The on color changed.
    /// </summary>
    /// <param name="newColor">The new color.</param>
    /// <param name="oldColor">The old color.</param>
    private void RaiseColorChanged(Color newColor, Color oldColor)
    {
        ColorChanged?.Invoke(this, new ColorChangedEventArgs(newColor, oldColor));
    }

    /// <summary>
    /// The color board_ done clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnColorBoardDoneClicked(object sender, RoutedEventArgs e)
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        SetCurrentValue(ColorProperty, _colorBoard.Color);
        _popup.SetCurrentValue(Popup.IsOpenProperty, false);
    }

    /// <summary>
    /// The color board_ cancel clicked.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnColorBoardCancelClicked(object sender, RoutedEventArgs e)
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        var color = Color;

        SetCurrentValue(CurrentColorProperty, color);
        _colorBoard.SetCurrentValue(ColorBoard.ColorProperty, color);
        _popup.SetCurrentValue(Popup.IsOpenProperty, false);
    }

    /// <summary>
    /// The color board_ size changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnColorBoardSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        if (PopupPlacement == PlacementMode.Bottom)
        {
            _popup.SetCurrentValue(Popup.VerticalOffsetProperty, ActualHeight);
        }

        if (PopupPlacement == PlacementMode.Top)
        {
            _popup.SetCurrentValue(Popup.VerticalOffsetProperty, -1 * _colorBoard.ActualHeight);
        }

        if (PopupPlacement == PlacementMode.Right)
        {
            _popup.SetCurrentValue(Popup.HorizontalOffsetProperty, ActualWidth);
        }

        if (PopupPlacement == PlacementMode.Left)
        {
            _popup.SetCurrentValue(Popup.HorizontalOffsetProperty, -1 * _colorBoard.ActualWidth);
        }
    }


    private void UpdateCustomThemeColors()
    {
        if (!IsPartsInitialized || _colorBoard is null)
        {
            return;
        }

        if (CustomThemeColors is null)
        {
            _colorBoard.SetCurrentValue(ColorBoard.CustomThemeColorsProperty, null);
            return;
        }

        // Convert from IEnumerable<Color> to List<PredefinedColorItem>
        var themeColorItems = CustomThemeColors
            .Select(color => new PredefinedColorItem(color, PredefinedColor.GetColorName(color)))
            .ToList();

        _colorBoard.SetCurrentValue(ColorBoard.CustomThemeColorsProperty, themeColorItems);
    }

    private void UpdateColorBoardScale()
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        _colorBoard.SetCurrentValue(LayoutTransformProperty, new ScaleTransform(PopupScale, PopupScale));
    }

    protected override AutomationPeer OnCreateAutomationPeer() => new ColorPickerAutomationPeer(this);
}
