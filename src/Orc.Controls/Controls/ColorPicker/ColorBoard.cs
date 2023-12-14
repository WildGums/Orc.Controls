﻿namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Automation;
using Catel.Logging;

[TemplatePart(Name = "PART_RootGrid", Type = typeof(FrameworkElement))]
[TemplatePart(Name = "PART_HSVCanvas", Type = typeof(Canvas))]

[TemplatePart(Name = "PART_HSVColorGradientStop", Type = typeof(GradientStop))]
[TemplatePart(Name = "PART_HSVRectangle", Type = typeof(Rectangle))]
[TemplatePart(Name = "PART_HSVEllipse", Type = typeof(Ellipse))]
[TemplatePart(Name = "PART_HSVSlider", Type = typeof(Slider))]


[TemplatePart(Name = "PART_ASlider", Type = typeof(Slider))]
[TemplatePart(Name = "PART_A0GradientStop", Type = typeof(GradientStop))]
[TemplatePart(Name = "PART_A1GradientStop", Type = typeof(GradientStop))]

[TemplatePart(Name = "PART_RSlider", Type = typeof(Slider))]
[TemplatePart(Name = "PART_R0GradientStop", Type = typeof(GradientStop))]
[TemplatePart(Name = "PART_R1GradientStop", Type = typeof(GradientStop))]

[TemplatePart(Name = "PART_GSlider", Type = typeof(Slider))]
[TemplatePart(Name = "PART_G0GradientStop", Type = typeof(GradientStop))]
[TemplatePart(Name = "PART_G1GradientStop", Type = typeof(GradientStop))]

[TemplatePart(Name = "PART_BSlider", Type = typeof(Slider))]
[TemplatePart(Name = "PART_B0GradientStop", Type = typeof(GradientStop))]
[TemplatePart(Name = "PART_B1GradientStop", Type = typeof(GradientStop))]

[TemplatePart(Name = "PART_ATextBox", Type = typeof(TextBox))]
[TemplatePart(Name = "PART_RTextBox", Type = typeof(TextBox))]
[TemplatePart(Name = "PART_GTextBox", Type = typeof(TextBox))]
[TemplatePart(Name = "PART_BTextBox", Type = typeof(TextBox))]

[TemplatePart(Name = "PART_ColorComboBox", Type = typeof(ComboBox))]

[TemplatePart(Name = "PART_ColorBrush", Type = typeof(SolidColorBrush))]
[TemplatePart(Name = "PART_ColorTextBox", Type = typeof(TextBox))]

[TemplatePart(Name = "PART_ThemeColorsListBox", Type = typeof(ListBox))]
[TemplatePart(Name = "PART_RecentColorsListBox", Type = typeof(ListBox))]

[TemplatePart(Name = "PART_SelectButton", Type = typeof(Button))]
[TemplatePart(Name = "PART_CancelButton", Type = typeof(Button))]
public class ColorBoard : Control
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private static readonly Color DefaultColor = Colors.White;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorBoard"/> class.
    /// </summary>
    public ColorBoard()
    {
        DefaultStyleKey = typeof(ColorBoard);

        var items = new List<PredefinedColorItem>(10);
        SetCurrentValue(RecentColorItemsProperty, items);
    }

    /// <summary>
    /// The canvas hsv.
    /// </summary>
    private Canvas? _hsvCanvas;

    /// <summary>
    /// The combo box color.
    /// </summary>
    private ComboBox? _colorComboBox;

    /// <summary>
    /// The ellipse hsv.
    /// </summary>
    private Ellipse? _hsvEllipse;

    /// <summary>
    /// The gradient stop a 0.
    /// </summary>  
    private GradientStop? _a0GradientStop;

    /// <summary>
    /// The gradient stop a 1.
    /// </summary>
    private GradientStop? _a1GradientStop;

    /// <summary>
    /// The gradient stop b 0.
    /// </summary>
    private GradientStop? _b0GradientStop;

    /// <summary>
    /// The gradient stop b 1.
    /// </summary>
    private GradientStop? _b1GradientStop;

    /// <summary>
    /// The gradient stop g 0.
    /// </summary>
    private GradientStop? _g0GradientStop;

    /// <summary>
    /// The gradient stop g 1.
    /// </summary>
    private GradientStop? _g1GradientStop;

    /// <summary>
    /// The gradient stop hsv color.
    /// </summary>
    private GradientStop? _hsvColorGradientStop;

    /// <summary>
    /// The gradient stop r 0.
    /// </summary>
    private GradientStop? _r0GradientStop;

    /// <summary>
    /// The gradient stop r 1.
    /// </summary>
    private GradientStop? _r1GradientStop;

    /// <summary>
    /// The recent colors grid.
    /// </summary>
    private ListBox? _recentColorsListBox;

    /// <summary>
    /// The rectangle hsv.
    /// </summary>
    private Rectangle? _hsvRectangle;

    /// <summary>
    /// The root element.
    /// </summary>
    private FrameworkElement? _rootGrid;

    /// <summary>
    /// The slider a.
    /// </summary>
    private Slider? _aSlider;

    /// <summary>
    /// The slider b.
    /// </summary>
    private Slider? _bSlider;

    /// <summary>
    /// The slider g.
    /// </summary>
    private Slider? _gSlider;

    /// <summary>
    /// The slider hsv.
    /// </summary>
    private Slider? _hsvSlider;

    /// <summary>
    /// The slider r.
    /// </summary>
    private Slider? _rSlider;

    /// <summary>
    /// The text box a.
    /// </summary>
    private TextBox? _aTextBox;

    /// <summary>
    /// The text box b.
    /// </summary>
    private TextBox? _bTextBox;

    /// <summary>
    /// The text box color.
    /// </summary>
    private TextBox? _colorTextBox;

    /// <summary>
    /// The text box g.
    /// </summary>
    private TextBox? _gTextBox;

    /// <summary>
    /// The text box r.
    /// </summary>
    private TextBox? _rTextBox;

    /// <summary>
    /// The theme colors grid.
    /// </summary>
    private ListBox? _themeColorsListBox;

    /// <summary>
    /// The brush color.
    /// </summary>
    private SolidColorBrush? _colorBrush;

    /// <summary>
    /// The dictionary color.
    /// </summary>
    
    private IImmutableDictionary<Color, PredefinedColorItem> _dictionaryColor = ImmutableDictionary<Color, PredefinedColorItem>.Empty;
    
    /// <summary>
    /// The is updating.
    /// </summary>
    private int _isUpdating;
    
    /// <summary>
    /// The tracking hsv.
    /// </summary>
    private bool _trackingHsv;


    public static readonly RoutedEvent CancelClickedEvent = EventManager.RegisterRoutedEvent(nameof(CancelClicked), RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(ColorBoard));

    public event RoutedEventHandler CancelClicked
    {
        add { AddHandler(CancelClickedEvent, value); }
        remove { RemoveHandler(CancelClickedEvent, value); }
    }

    public static readonly RoutedEvent DoneClickedEvent = EventManager.RegisterRoutedEvent(nameof(DoneClicked), RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(ColorBoard));

    public event RoutedEventHandler DoneClicked
    {
        add { AddHandler(DoneClickedEvent, value); }
        remove { RemoveHandler(DoneClickedEvent, value); }
    }

    /// <summary>
    /// Gets or sets the color.
    /// </summary>
    public Color Color
    {
        get { return (Color)GetValue(ColorProperty); }
        set { SetValue(ColorProperty, value); }
    }

    public static readonly DependencyProperty ColorProperty = DependencyProperty.RegisterAttached(
        nameof(Color), typeof(Color), typeof(ColorBoard), new PropertyMetadata(OnColorChanged));

    /// <summary>
    /// Gets or sets the recent color items.
    /// </summary>
    public List<PredefinedColorItem>? RecentColorItems
    {
        get { return (List<PredefinedColorItem>?)GetValue(RecentColorItemsProperty); }
        set { SetValue(RecentColorItemsProperty, value); }
    }

    public static readonly DependencyProperty RecentColorItemsProperty = DependencyProperty.Register(nameof(RecentColorItems),
        typeof(List<PredefinedColorItem>), typeof(ColorBoard), new PropertyMetadata());

    /// <summary>
    /// Gets a value indicating whether updating.
    /// </summary>
    private bool Updating => _isUpdating != 0;

    [MemberNotNullWhen(true, nameof(_rootGrid), 
        nameof(_hsvCanvas), nameof(_hsvColorGradientStop),
        nameof(_hsvRectangle), nameof(_hsvRectangle),
        nameof(_hsvEllipse), nameof(_hsvSlider),
        nameof(_aSlider), nameof(_a0GradientStop),
        nameof(_a1GradientStop), nameof(_rSlider),
        nameof(_r0GradientStop), nameof(_r1GradientStop),
        nameof(_gSlider), nameof(_g0GradientStop),
        nameof(_g1GradientStop), nameof(_bSlider),
        nameof(_b0GradientStop), nameof(_b1GradientStop),
        nameof(_aTextBox), nameof(_rTextBox),
        nameof(_gTextBox), nameof(_bTextBox),
        nameof(_colorComboBox), nameof(_colorBrush),
        nameof(_colorTextBox), nameof(_themeColorsListBox),
        nameof(_recentColorsListBox), nameof(_themeColorsListBox))]
    private bool IsPartsInitialized { get; set; }

    /// <summary>
    /// The on apply template.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _rootGrid = GetTemplateChild("PART_RootGrid") as Grid;
        if (_rootGrid is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_RootGrid'");
        }

        /*HSV slider*/
        _hsvCanvas = GetTemplateChild("PART_HSVCanvas") as Canvas;
        if (_hsvCanvas is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_HSVCanvas'");
        }

        _hsvColorGradientStop = GetTemplateChild("PART_HSVColorGradientStop") as GradientStop;
        if (_hsvColorGradientStop is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_HSVColorGradientStop'");
        }

        _hsvRectangle = GetTemplateChild("PART_HSVRectangle") as Rectangle;
        if (_hsvRectangle is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_HSVRectangle'");
        }
        _hsvRectangle.MouseLeftButtonDown += OnHsvRectangleMouseLeftButtonDown;
        _hsvRectangle.MouseMove += OnHsvRectangleMouseMove;
        _hsvRectangle.MouseLeftButtonUp += OnHsvRectangleMouseLeftButtonUp;
        _hsvRectangle.MouseLeave += OnHsvRectangleMouseLeave;

        _hsvEllipse = GetTemplateChild("PART_HSVEllipse") as Ellipse;
        if (_hsvEllipse is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_HSVEllipse'");
        }

        _hsvSlider = GetTemplateChild("PART_HSVSlider") as Slider;
        if (_hsvSlider is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_HSVSlider'");
        }
        _hsvSlider.ValueChanged += OnHsvSliderValueChanged;

        /*ARGB sliders*/
        _aSlider = GetTemplateChild("PART_ASlider") as Slider;
        if (_aSlider is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ASlider'");
        }
        _aSlider.ValueChanged += OnArgbSliderValueChanged;

        _a0GradientStop = GetTemplateChild("PART_A0GradientStop") as GradientStop;
        if (_a0GradientStop is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_A0GradientStop'");
        }

        _a1GradientStop = GetTemplateChild("PART_A1GradientStop") as GradientStop;
        if (_a1GradientStop is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_A1GradientStop'");
        }

        _rSlider = GetTemplateChild("PART_RSlider") as Slider;
        if (_rSlider is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_RSlider'");
        }
        _rSlider.ValueChanged += OnArgbSliderValueChanged;

        _r0GradientStop = GetTemplateChild("PART_R0GradientStop") as GradientStop;
        if (_r0GradientStop is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_R0GradientStop'");
        }

        _r1GradientStop = GetTemplateChild("PART_R1GradientStop") as GradientStop;
        if (_r1GradientStop is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_R1GradientStop'");
        }

        _gSlider = GetTemplateChild("PART_GSlider") as Slider;
        if (_gSlider is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_GSlider'");
        }
        _gSlider.ValueChanged += OnArgbSliderValueChanged;

        _g0GradientStop = GetTemplateChild("PART_G0GradientStop") as GradientStop;
        if (_g0GradientStop is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_G0GradientStop'");
        }

        _g1GradientStop = GetTemplateChild("PART_G1GradientStop") as GradientStop;
        if (_g1GradientStop is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_G1GradientStop'");
        }

        _bSlider = GetTemplateChild("PART_BSlider") as Slider;
        if (_bSlider is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_BSlider'");
        }
        _bSlider.ValueChanged += OnArgbSliderValueChanged;

        _b0GradientStop = GetTemplateChild("PART_B0GradientStop") as GradientStop;
        if (_b0GradientStop is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_B0GradientStop'");
        }

        _b1GradientStop = GetTemplateChild("PART_B1GradientStop") as GradientStop;
        if (_b1GradientStop is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_B1GradientStop'");
        }
            
        _aTextBox = GetTemplateChild("PART_ATextBox") as TextBox;
        if (_aTextBox is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ATextBox'");
        }
        _aTextBox.LostFocus += OnATextBoxLostFocus;

        _rTextBox = GetTemplateChild("PART_RTextBox") as TextBox;
        if (_rTextBox is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_RTextBox'");
        }
        _rTextBox.LostFocus += OnRTextBoxLostFocus;

        _gTextBox = GetTemplateChild("PART_GTextBox") as TextBox;
        if (_gTextBox is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_GTextBox'");
        }
        _gTextBox.LostFocus += OnGTextBoxLostFocus;

        _bTextBox = GetTemplateChild("PART_BTextBox") as TextBox;
        if (_bTextBox is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_BTextBox'");
        }
        _bTextBox.LostFocus += OnBTextBoxLostFocus;

        /*Color*/
        _colorComboBox = GetTemplateChild("PART_ColorComboBox") as ComboBox;
        if (_colorComboBox is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ColorComboBox'");
        }
        _colorComboBox.SelectionChanged += OnColorComboBoxSelectionChanged;

        _colorBrush = GetTemplateChild("PART_ColorBrush") as SolidColorBrush;
        if (_colorBrush is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ColorBrush'");
        }

        _colorTextBox = GetTemplateChild("PART_ColorTextBox") as TextBox;
        if (_colorTextBox is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ColorTextBox'");
        }
        _colorTextBox.GotFocus += OnColorTextBoxGotFocus;
        _colorTextBox.LostFocus += OnColorTextBoxLostFocus;

        _themeColorsListBox = GetTemplateChild("PART_ThemeColorsListBox") as ListBox;
        if (_themeColorsListBox is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ThemeColorsListBox'");
        }
        _themeColorsListBox.SelectionChanged += OnThemeColorsSelectionChanged;

        _recentColorsListBox = GetTemplateChild("PART_RecentColorsListBox") as ListBox;
        if (_recentColorsListBox is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_RecentColorsListBox'");
        }
        _recentColorsListBox.SelectionChanged += OnRecentColorsSelectionChanged;

        if (GetTemplateChild("PART_SelectButton") is not Button selectButton)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_SelectButton'");
        }
        selectButton.Click += OnsSelectButtonClick;

        if (GetTemplateChild("PART_CancelButton") is not Button cancelButton)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_CancelButton'");
        }
        cancelButton.Click += OnCancelButtonClick;

        IsPartsInitialized = true;

        InitializePredefined();
        InitializeThemeColors();
        UpdateControls(Color, true, true, true);

        KeyDown += OnColorBoardKeyDown;
    }

    /// <summary>
    /// The on done clicked.
    /// </summary>
    public void OnDoneClicked()
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        var recentColorItems = RecentColorItems;
        if (recentColorItems is null)
        {
            return;
        }

        var selectedColor = Color;
        var recentColorsGridItems = _recentColorsListBox.Items;

        var mostRecentColorItem = recentColorItems.FirstOrDefault(i => i.Color == selectedColor);
        if (mostRecentColorItem is not null)
        {
            recentColorsGridItems.Remove(mostRecentColorItem);
            recentColorItems.Remove(mostRecentColorItem);
        }
        else
        {
            mostRecentColorItem = new PredefinedColorItem(selectedColor, selectedColor.ToString());
        }

        recentColorItems.Insert(0, mostRecentColorItem);
        recentColorsGridItems.Insert(0, mostRecentColorItem);

        RaiseEvent(new RoutedEventArgs(DoneClickedEvent));
    }

    #region Methods
    /// <summary>
    /// The on color property changed.
    /// </summary>
    /// <param name="d">The d.</param>
    /// <param name="e">The e.</param>
    private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ColorBoard control || control._rootGrid is null)
        {
            return;
        }

        if (control.Updating)
        {
            return;
        }

        var color = (Color)e.NewValue;
        control.UpdateControls(color, true, true, true);
    }

    /// <summary>
    /// The begin update.
    /// </summary>
    private void BeginUpdate()
    {
        _isUpdating++;
    }

    /// <summary>
    /// The color board_ key down.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnColorBoardKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            OnDoneClicked();
        }
    }

    /// <summary>
    /// The end update.
    /// </summary>
    private void EndUpdate()
    {
        _isUpdating--;
    }

    /// <summary>
    /// The get hsv color.
    /// </summary>
    /// <returns>The <see cref="Color" />.</returns>
    private Color GetHsvColor()
    {
        if (!IsPartsInitialized)
        {
            return DefaultColor;
        }

        var h = _hsvSlider.Value;

        var x = (double)_hsvEllipse.GetValue(Canvas.LeftProperty) + _hsvEllipse.ActualWidth / 2;
        var y = (double)_hsvEllipse.GetValue(Canvas.TopProperty) + _hsvEllipse.ActualHeight / 2;

        var s = x / (_hsvRectangle.ActualWidth - 1);
        s = s switch
        {
            < 0d => 0d,
            > 1d => 1d,
            _ => s
        };

        var v = 1 - y / (_hsvRectangle.ActualHeight - 1);
        v = v switch
        {
            < 0d => 0d,
            > 1d => 1d,
            _ => v
        };

        return ColorHelper.HSV2RGB(h, s, v);
    }

    /// <summary>
    /// The get rgb color.
    /// </summary>
    /// <returns>The <see cref="Color" />.</returns>
    private Color GetRgbColor()
    {
        if (!IsPartsInitialized)
        {
            return DefaultColor;
        }

        var a = (byte)_aSlider.Value;
        var r = (byte)_rSlider.Value;
        var g = (byte)_gSlider.Value;
        var b = (byte)_bSlider.Value;

        return Color.FromArgb(a, r, g, b);
    }

    /// <summary>
    /// The hs v_ mouse leave.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnHsvRectangleMouseLeave(object sender, MouseEventArgs e)
    {
        _trackingHsv = false;
        _hsvRectangle?.ReleaseMouseCapture();
    }

    /// <summary>
    /// The hs v_ mouse left button down.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnHsvRectangleMouseLeftButtonDown(object? sender, MouseButtonEventArgs e)
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        e.Handled = true;
        _trackingHsv = _hsvRectangle.CaptureMouse();

        var point = e.GetPosition(_hsvRectangle);

        _hsvEllipse.SetCurrentValue(Canvas.LeftProperty, point.X - _hsvEllipse.ActualWidth / 2);
        _hsvEllipse.SetCurrentValue(Canvas.TopProperty, point.Y - _hsvEllipse.ActualHeight / 2);

        if (Updating)
        {
            return;
        }

        var color = GetHsvColor();
        UpdateControls(color, false, true, true);
    }

    /// <summary>
    /// The hs v_ mouse left button up.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnHsvRectangleMouseLeftButtonUp(object? sender, MouseButtonEventArgs e)
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        e.Handled = true;
        _trackingHsv = false;

        _hsvRectangle.ReleaseMouseCapture();
    }

    /// <summary>
    /// The hs v_ mouse move.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnHsvRectangleMouseMove(object? sender, MouseEventArgs e)
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        if (!_trackingHsv)
        {
            return;
        }

        var point = e.GetPosition(_hsvRectangle);

        double ellipseX;
        if (point.X < 0)
        {
            ellipseX = 0 - _hsvEllipse.ActualWidth / 2;
        }
        else
        {
            ellipseX = point.X > _hsvCanvas.ActualWidth
                ? _hsvCanvas.ActualWidth - _hsvEllipse.ActualWidth / 2
                : point.X - _hsvEllipse.ActualWidth / 2;
        }

        double ellipseY;
        if (point.Y < 0)
        {
            ellipseY = 0 - _hsvEllipse.ActualHeight / 2;
        }
        else
        {
            ellipseY = point.Y > _hsvCanvas.ActualHeight
                ? _hsvCanvas.ActualHeight - _hsvEllipse.ActualHeight / 2
                : point.Y - _hsvEllipse.ActualHeight / 2;
        }

        _hsvEllipse.SetCurrentValue(Canvas.LeftProperty, ellipseX);
        _hsvEllipse.SetCurrentValue(Canvas.TopProperty, ellipseY);

        if (Updating)
        {
            return;
        }

        var color = GetHsvColor();
        UpdateControls(color, false, true, true);
    }

    /// <summary>
    /// The initialize predefined.
    /// </summary>
    private void InitializePredefined()
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        var list = PredefinedColor.All;
        var dictionaryColor = new Dictionary<Color, PredefinedColorItem>();
        foreach (var color in list)
        {
            var item = new PredefinedColorItem(color.Value, color.Name);
            _colorComboBox.Items.Add(item);

            if (!dictionaryColor.ContainsKey(color.Value))
            {
                dictionaryColor.Add(color.Value, item);
            }
        }
        _dictionaryColor = dictionaryColor.ToImmutableDictionary();
    }

    /// <summary>
    /// The initialize theme colors.
    /// </summary>
    private void InitializeThemeColors()
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        var list = PredefinedColor.AllThemeColors;
        var themeColors = new Dictionary<Color, PredefinedColorItem>();
        var r = 0;
        var c = 0;

        foreach (var color in list)
        {
            var item = new PredefinedColorItem(color.Value, color.Name);
            item.SetValue(Grid.RowProperty, r);
            item.SetValue(Grid.ColumnProperty, c);

            _themeColorsListBox.Items.Add(item);

            if (!themeColors.ContainsKey(color.Value))
            {
                themeColors.Add(color.Value, item);
            }

            if (r < 5)
            {
                r++;
            }
            else
            {
                r = 0;
                c++;
            }
        }
    }

    public void SetHsvColor(Color color)
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        var h = ColorHelper.GetHSV_H(color);
        var s = ColorHelper.GetHSV_S(color);
        var v = ColorHelper.GetHSV_V(color);

        _hsvSlider.SetCurrentValue(RangeBase.ValueProperty, h);
        _hsvColorGradientStop.SetCurrentValue(GradientStop.ColorProperty, ColorHelper.HSV2RGB(h, 1d, 1d));

        var x = s * (_hsvRectangle.ActualWidth - 1);
        var y = (1 - v) * (_hsvRectangle.ActualHeight - 1);

        _hsvEllipse.SetCurrentValue(Canvas.LeftProperty, x - _hsvEllipse.ActualWidth / 2);
        _hsvEllipse.SetCurrentValue(Canvas.TopProperty, y - _hsvEllipse.ActualHeight / 2);
    }

    /// <summary>
    /// The update controls.
    /// </summary>
    /// <param name="color">The color.</param>
    /// <param name="hsv">The hsv.</param>
    /// <param name="rgb">The rgb.</param>
    /// <param name="predefined">The predefined.</param>
    private void UpdateControls(Color color, bool hsv, bool rgb, bool predefined)
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        if (Updating)
        {
            return;
        }

        try
        {
            BeginUpdate();

            // HSV
            if (hsv)
            {
                SetHsvColor(color);
            }

            if (rgb)
            {
                var a = color.A;
                var r = color.R;
                var g = color.G;
                var b = color.B;

                _aSlider.SetCurrentValue(RangeBase.ValueProperty, (double)a);
                _a0GradientStop.SetCurrentValue(GradientStop.ColorProperty, Color.FromArgb(0, r, g, b));
                _a1GradientStop.SetCurrentValue(GradientStop.ColorProperty, Color.FromArgb(255, r, g, b));
                _aTextBox.SetCurrentValue(TextBox.TextProperty, a.ToString("X2"));

                _rSlider.SetCurrentValue(RangeBase.ValueProperty, (double)r);
                _r0GradientStop.SetCurrentValue(GradientStop.ColorProperty, Color.FromArgb(255, 0, g, b));
                _r1GradientStop.SetCurrentValue(GradientStop.ColorProperty, Color.FromArgb(255, 255, g, b));
                _rTextBox.SetCurrentValue(TextBox.TextProperty, r.ToString("X2"));

                _gSlider.SetCurrentValue(RangeBase.ValueProperty, (double)g);
                _g0GradientStop.SetCurrentValue(GradientStop.ColorProperty, Color.FromArgb(255, r, 0, b));
                _g1GradientStop.SetCurrentValue(GradientStop.ColorProperty, Color.FromArgb(255, r, 255, b));
                _gTextBox.SetCurrentValue(TextBox.TextProperty, g.ToString("X2"));

                _bSlider.SetCurrentValue(RangeBase.ValueProperty, (double)b);
                _b0GradientStop.SetCurrentValue(GradientStop.ColorProperty, Color.FromArgb(255, r, g, 0));
                _b1GradientStop.SetCurrentValue(GradientStop.ColorProperty, Color.FromArgb(255, r, g, 255));
                _bTextBox.SetCurrentValue(TextBox.TextProperty, b.ToString("X2"));
            }

            if (predefined)
            {
                _colorBrush.SetCurrentValue(SolidColorBrush.ColorProperty, color);
                if (_dictionaryColor.ContainsKey(color))
                {
                    _colorComboBox.SetCurrentValue(Selector.SelectedItemProperty, _dictionaryColor[color]);
                    _colorTextBox.SetCurrentValue(TextBox.TextProperty, string.Empty);
                }
                else
                {
                    _colorComboBox.SetCurrentValue(Selector.SelectedItemProperty, null);
                    _colorTextBox.SetCurrentValue(TextBox.TextProperty, color.ToString());
                }

                Keyboard.Focus(_colorComboBox);
            }

            SetCurrentValue(ColorProperty, color);
        }
        finally
        {
            EndUpdate();
        }
    }

    /// <summary>
    /// The button cancel_ click.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnCancelButtonClick(object sender, RoutedEventArgs e)
    {
        RaiseEvent(new RoutedEventArgs(CancelClickedEvent));
    }

    /// <summary>
    /// The button done_ click.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnsSelectButtonClick(object sender, RoutedEventArgs e)
    {
        OnDoneClicked();
    }

    /// <summary>
    /// The combo box color_ selection changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnColorComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!IsPartsInitialized || Updating)
        {
            return;
        }

        if (_colorComboBox.SelectedItem is PredefinedColorItem colorItem)
        {
            SetCurrentValue(ColorProperty, colorItem.Color);
        }
    }

    /// <summary>
    /// The recent colors grid_ selection changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnRecentColorsSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        if (_recentColorsListBox.SelectedItems.Count != 1)
        {
            return;
        }

        var c = ((PredefinedColorItem)_recentColorsListBox.SelectedItem).Color;
        UpdateControls(c, true, true, true);
    }

    /// <summary>
    /// The slider a_ value changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnArgbSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (Updating)
        {
            return;
        }

        var color = GetRgbColor();
        UpdateControls(color, true, true, true);
    }

    /// <summary>
    /// The slider hs v_ value changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnHsvSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (!IsPartsInitialized || Updating)
        {
            return;
        }

        _hsvColorGradientStop.SetCurrentValue(GradientStop.ColorProperty, ColorHelper.HSV2RGB(e.NewValue, 1d, 1d));

        var color = GetHsvColor();
        UpdateControls(color, false, true, true);
    }

    /// <summary>
    /// The text box a_ lost focus.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnATextBoxLostFocus(object sender, RoutedEventArgs e)
    {
        if (!IsPartsInitialized || Updating)
        {
            return;
        }

        if (int.TryParse(_aTextBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var value))
        {
            _aSlider.SetCurrentValue(RangeBase.ValueProperty, (double)value);
        }
    }

    /// <summary>
    /// The text box b_ lost focus.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnBTextBoxLostFocus(object sender, RoutedEventArgs e)
    {
        if (!IsPartsInitialized || Updating)
        {
            return;
        }

        if (int.TryParse(_bTextBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var value))
        {
            _bSlider.SetCurrentValue(RangeBase.ValueProperty, (double)value);
        }
    }

    /// <summary>
    /// The text box color_ got focus.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnColorTextBoxGotFocus(object sender, RoutedEventArgs e)
    {
        if (!IsPartsInitialized || Updating)
        {
            return;
        }

        try
        {
            BeginUpdate();

            _colorComboBox.SetCurrentValue(Selector.SelectedItemProperty, null);
            _colorTextBox.SetCurrentValue(TextBox.TextProperty, Color.ToString());
        }
        finally
        {
            EndUpdate();
        }
    }

    /// <summary>
    /// The text box color_ lost focus.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnColorTextBoxLostFocus(object sender, RoutedEventArgs e)
    {
        if (!IsPartsInitialized || Updating)
        {
            return;
        }

        var text = _colorTextBox.Text.TrimStart('#');
        if (uint.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var value))
        {
            var b = (byte)(value & 0xFF);
            value >>= 8;
            var g = (byte)(value & 0xFF);
            value >>= 8;
            var r = (byte)(value & 0xFF);
            value >>= 8;
            var a = (byte)(value & 0xFF);

            if (text.Length <= 6)
            {
                a = 0xFF;
            }

            var color = Color.FromArgb(a, r, g, b);
            SetCurrentValue(ColorProperty, color);
        }
        else
        {
            SetCurrentValue(ColorProperty, Colors.White);
        }
    }

    /// <summary>
    /// The text box g_ lost focus.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnGTextBoxLostFocus(object sender, RoutedEventArgs e)
    {
        if (!IsPartsInitialized || Updating)
        {
            return;
        }

        if (int.TryParse(_gTextBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var value))
        {
            _gSlider.SetCurrentValue(RangeBase.ValueProperty, (double)value);
        }
    }

    /// <summary>
    /// The text box r_ lost focus.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnRTextBoxLostFocus(object sender, RoutedEventArgs e)
    {
        if (!IsPartsInitialized || Updating)
        {
            return;
        }

        if (int.TryParse(_rTextBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var value))
        {
            _rSlider.SetCurrentValue(RangeBase.ValueProperty, (double)value);
        }
    }

    /// <summary>
    /// The theme colors grid_ selection changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    private void OnThemeColorsSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!IsPartsInitialized)
        {
            return;
        }

        if (_themeColorsListBox.SelectedItems.Count != 1)
        {
            return;
        }

        var c = ((PredefinedColorItem)_themeColorsListBox.SelectedItem).Color;
        UpdateControls(c, true, true, true);
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new ColorBoardAutomationPeer(this);
    }
    #endregion
}
