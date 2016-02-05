// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorBoard.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// The color board.
    /// </summary>
    public class ColorBoard : Control
    {
        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorBoard"/> class.
        /// </summary>
        public ColorBoard()
        {
            DefaultStyleKey = typeof (ColorBoard);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether updating.
        /// </summary>
        private bool Updating
        {
            get { return _isUpdating != 0; }
        }
        #endregion

        #region Static Fields
        /// <summary>
        /// The color property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.RegisterAttached(
            "Color", typeof (Color), typeof (ColorBoard), new PropertyMetadata(OnColorPropertyChanged));

        /// <summary>
        /// The recent color items property.
        /// </summary>
        public static readonly DependencyProperty RecentColorItemsProperty =
            DependencyProperty.Register(
                "RecentColorItems",
                typeof (List<PredefinedColorItem>),
                typeof (ColorBoard),
                new PropertyMetadata(new List<PredefinedColorItem>(10)));
        #endregion

        #region Fields
        /// <summary>
        /// The brush color.
        /// </summary>
        private SolidColorBrush _brushColor;

        /// <summary>
        /// The button cancel.
        /// </summary>
        private Button _buttonCancel;

        /// <summary>
        /// The button done.
        /// </summary>
        private Button _buttonDone;

        /// <summary>
        /// The canvas hsv.
        /// </summary>
        private Canvas _canvasHsv;

        /// <summary>
        /// The combo box color.
        /// </summary>
        private ComboBox _comboBoxColor;

        /// <summary>
        /// The dictionary color.
        /// </summary>
        private Dictionary<Color, PredefinedColorItem> _dictionaryColor;

        /// <summary>
        /// The ellipse hsv.
        /// </summary>
        private Ellipse _ellipseHsv;

        /// <summary>
        /// The gradient stop a 0.
        /// </summary>
        private GradientStop _gradientStopA0;

        /// <summary>
        /// The gradient stop a 1.
        /// </summary>
        private GradientStop _gradientStopA1;

        /// <summary>
        /// The gradient stop b 0.
        /// </summary>
        private GradientStop _gradientStopB0;

        /// <summary>
        /// The gradient stop b 1.
        /// </summary>
        private GradientStop _gradientStopB1;

        /// <summary>
        /// The gradient stop g 0.
        /// </summary>
        private GradientStop _gradientStopG0;

        /// <summary>
        /// The gradient stop g 1.
        /// </summary>
        private GradientStop _gradientStopG1;

        /// <summary>
        /// The gradient stop hsv color.
        /// </summary>
        private GradientStop _gradientStopHsvColor;

        /// <summary>
        /// The gradient stop r 0.
        /// </summary>
        private GradientStop _gradientStopR0;

        /// <summary>
        /// The gradient stop r 1.
        /// </summary>
        private GradientStop _gradientStopR1;

        /// <summary>
        /// The is updating.
        /// </summary>
        private int _isUpdating;

        /// <summary>
        /// The recent colors grid.
        /// </summary>
        private ListBox _recentColorsGrid;

        /// <summary>
        /// The rectangle color.
        /// </summary>
        private Rectangle _rectangleColor;

        /// <summary>
        /// The rectangle hsv.
        /// </summary>
        private Rectangle _rectangleHsv;

        /// <summary>
        /// The rectangle root hsv.
        /// </summary>
        private Rectangle _rectangleRootHsv;

        /// <summary>
        /// The root element.
        /// </summary>
        private FrameworkElement rootElement;

        /// <summary>
        /// The slider a.
        /// </summary>
        private Slider _sliderA;

        /// <summary>
        /// The slider b.
        /// </summary>
        private Slider _sliderB;

        /// <summary>
        /// The slider g.
        /// </summary>
        private Slider _sliderG;

        /// <summary>
        /// The slider hsv.
        /// </summary>
        private Slider _sliderHsv;

        /// <summary>
        /// The slider r.
        /// </summary>
        private Slider _sliderR;

        /// <summary>
        /// The text box a.
        /// </summary>
        private TextBox _textBoxA;

        /// <summary>
        /// The text box b.
        /// </summary>
        private TextBox _textBoxB;

        /// <summary>
        /// The text box color.
        /// </summary>
        private TextBox _textBoxColor;

        /// <summary>
        /// The text box g.
        /// </summary>
        private TextBox _textBoxG;

        /// <summary>
        /// The text box r.
        /// </summary>
        private TextBox _textBoxR;

        /// <summary>
        /// The theme colors.
        /// </summary>
        private Dictionary<Color, PredefinedColorItem> _themeColors;

        /// <summary>
        /// The theme colors grid.
        /// </summary>
        private ListBox _themeColorsGrid;

        /// <summary>
        /// The tracking hsv.
        /// </summary>
        private bool _trackingHsv;
        #endregion

        #region Public Events
        /// <summary>
        /// The done clicked.
        /// </summary>
        public event RoutedEventHandler CancelClicked;

        /// <summary>
        /// The done clicked.
        /// </summary>
        public event RoutedEventHandler DoneClicked;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public Color Color
        {
            get { return (Color) GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the recent color items.
        /// </summary>
        public List<PredefinedColorItem> RecentColorItems
        {
            get { return (List<PredefinedColorItem>) GetValue(RecentColorItemsProperty); }

            set { SetValue(RecentColorItemsProperty, value); }
        }
        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// The on apply template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            rootElement = (FrameworkElement) GetTemplateChild("RootElement");

            _canvasHsv = (Canvas) GetTemplateChild("CanvasHSV");
            _rectangleRootHsv = (Rectangle) GetTemplateChild("RectangleRootHSV");
            _gradientStopHsvColor = (GradientStop) GetTemplateChild("GradientStopHSVColor");
            _rectangleHsv = (Rectangle) GetTemplateChild("RectangleHSV");
            _ellipseHsv = (Ellipse) GetTemplateChild("EllipseHSV");
            _sliderHsv = (Slider) GetTemplateChild("SliderHSV");

            _sliderA = (Slider) GetTemplateChild("SliderA");
            _gradientStopA0 = (GradientStop) GetTemplateChild("GradientStopA0");
            _gradientStopA1 = (GradientStop) GetTemplateChild("GradientStopA1");
            _sliderR = (Slider) GetTemplateChild("SliderR");
            _gradientStopR0 = (GradientStop) GetTemplateChild("GradientStopR0");
            _gradientStopR1 = (GradientStop) GetTemplateChild("GradientStopR1");
            _sliderG = (Slider) GetTemplateChild("SliderG");
            _gradientStopG0 = (GradientStop) GetTemplateChild("GradientStopG0");
            _gradientStopG1 = (GradientStop) GetTemplateChild("GradientStopG1");
            _sliderB = (Slider) GetTemplateChild("SliderB");
            _gradientStopB0 = (GradientStop) GetTemplateChild("GradientStopB0");
            _gradientStopB1 = (GradientStop) GetTemplateChild("GradientStopB1");

            _textBoxA = (TextBox) GetTemplateChild("TextBoxA");
            _textBoxR = (TextBox) GetTemplateChild("TextBoxR");
            _textBoxG = (TextBox) GetTemplateChild("TextBoxG");
            _textBoxB = (TextBox) GetTemplateChild("TextBoxB");

            _comboBoxColor = (ComboBox) GetTemplateChild("ComboBoxColor");
            _rectangleColor = (Rectangle) GetTemplateChild("RectangleColor");
            _brushColor = (SolidColorBrush) GetTemplateChild("BrushColor");
            _textBoxColor = (TextBox) GetTemplateChild("TextBoxColor");
            _buttonDone = (Button) GetTemplateChild("ButtonDone");
            _buttonCancel = (Button) GetTemplateChild("ButtonCancel");
            _themeColorsGrid = (ListBox) GetTemplateChild("ThemeColorsGrid");
            _recentColorsGrid = (ListBox) GetTemplateChild("RecentColorsGrid");

            _themeColorsGrid.SelectionChanged += themeColorsGrid_SelectionChanged;
            _recentColorsGrid.SelectionChanged += recentColorsGrid_SelectionChanged;

            _rectangleHsv.MouseLeftButtonDown += HSV_MouseLeftButtonDown;
            _rectangleHsv.MouseMove += HSV_MouseMove;
            _rectangleHsv.MouseLeftButtonUp += HSV_MouseLeftButtonUp;
            _rectangleHsv.MouseLeave += HSV_MouseLeave;

            _sliderHsv.ValueChanged += sliderHSV_ValueChanged;

            _sliderA.ValueChanged += sliderA_ValueChanged;
            _sliderR.ValueChanged += sliderR_ValueChanged;
            _sliderG.ValueChanged += sliderG_ValueChanged;
            _sliderB.ValueChanged += sliderB_ValueChanged;

            _textBoxA.LostFocus += textBoxA_LostFocus;
            _textBoxR.LostFocus += textBoxR_LostFocus;
            _textBoxG.LostFocus += textBoxG_LostFocus;
            _textBoxB.LostFocus += textBoxB_LostFocus;

            _comboBoxColor.SelectionChanged += comboBoxColor_SelectionChanged;
            _textBoxColor.GotFocus += textBoxColor_GotFocus;
            _textBoxColor.LostFocus += textBoxColor_LostFocus;
            _buttonDone.Click += buttonDone_Click;
            _buttonCancel.Click += buttonCancel_Click;

            InitializePredefined();
            InitializeThemeColors();
            UpdateControls(Color, true, true, true);

            KeyDown += ColorBoard_KeyDown;
        }

        /// <summary>
        /// The on cancel clicked.
        /// </summary>
        public void OnCancelClicked()
        {
            if (CancelClicked != null)
            {
                CancelClicked(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// The on done clicked.
        /// </summary>
        public void OnDoneClicked()
        {
            var ci = new PredefinedColorItem(Color, Color.ToString());

            if (RecentColorItems.Where(i => i.Color == ci.Color).Count() > 0)
            {
                _recentColorsGrid.Items.RemoveAt(_recentColorsGrid.Items.IndexOf(RecentColorItems.First(i => i.Color == ci.Color)));
                RecentColorItems.Remove(RecentColorItems.First(i => i.Color == ci.Color));

                _recentColorsGrid.Items.Insert(0, ci);
                RecentColorItems.Insert(0, ci);
            }
            else
            {
                RecentColorItems.Insert(0, ci);
                _recentColorsGrid.Items.Insert(0, ci);
            }

            var doneClicked = DoneClicked;
            if (doneClicked != null)
            {
                doneClicked(this, new RoutedEventArgs());
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// The on color property changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        private static void OnColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ColorBoard;
            if (control != null && control.rootElement != null)
            {
                if (control.Updating)
                {
                    return;
                }

                var color = (Color) e.NewValue;
                control.UpdateControls(color, true, true, true);
            }
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
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ColorBoard_KeyDown(object sender, KeyEventArgs e)
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
        private Color GetHSVColor()
        {
            double h = _sliderHsv.Value;

            double x = (double) _ellipseHsv.GetValue(Canvas.LeftProperty) + _ellipseHsv.ActualWidth/2;
            double y = (double) _ellipseHsv.GetValue(Canvas.TopProperty) + _ellipseHsv.ActualHeight/2;

            double s = x/(_rectangleHsv.ActualWidth - 1);
            if (s < 0d)
            {
                s = 0d;
            }
            else if (s > 1d)
            {
                s = 1d;
            }

            double v = 1 - y/(_rectangleHsv.ActualHeight - 1);
            if (v < 0d)
            {
                v = 0d;
            }
            else if (v > 1d)
            {
                v = 1d;
            }

            return ColorHelper.HSV2RGB(h, s, v);
        }

        /// <summary>
        /// The get rgb color.
        /// </summary>
        /// <returns>The <see cref="Color" />.</returns>
        private Color GetRGBColor()
        {
            var a = (byte) _sliderA.Value;
            var r = (byte) _sliderR.Value;
            var g = (byte) _sliderG.Value;
            var b = (byte) _sliderB.Value;

            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// The hs v_ mouse leave.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void HSV_MouseLeave(object sender, MouseEventArgs e)
        {
            _trackingHsv = false;
            _rectangleHsv.ReleaseMouseCapture();
        }

        /// <summary>
        /// The hs v_ mouse left button down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void HSV_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            _trackingHsv = _rectangleHsv.CaptureMouse();

            Point point = e.GetPosition(_rectangleHsv);

            Size size = _ellipseHsv.RenderSize;

            _ellipseHsv.SetValue(Canvas.LeftProperty, point.X - _ellipseHsv.ActualWidth/2);
            _ellipseHsv.SetValue(Canvas.TopProperty, point.Y - _ellipseHsv.ActualHeight/2);

            if (Updating)
            {
                return;
            }

            Color color = GetHSVColor();
            UpdateControls(color, false, true, true);
        }

        /// <summary>
        /// The hs v_ mouse left button up.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void HSV_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            _trackingHsv = false;

            _rectangleHsv.ReleaseMouseCapture();
        }

        /// <summary>
        /// The hs v_ mouse move.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void HSV_MouseMove(object sender, MouseEventArgs e)
        {
            if (_trackingHsv)
            {
                Point point = e.GetPosition(_rectangleHsv);
                Size size = _ellipseHsv.RenderSize;

                double ellipseX = 0;
                if (point.X < 0)
                {
                    ellipseX = 0 - _ellipseHsv.ActualWidth/2;
                }
                else
                {
                    if (point.X > _canvasHsv.ActualWidth)
                    {
                        ellipseX = _canvasHsv.ActualWidth - _ellipseHsv.ActualWidth/2;
                    }
                    else
                    {
                        ellipseX = point.X - _ellipseHsv.ActualWidth/2;
                    }
                }

                double ellipseY = 0;
                if (point.Y < 0)
                {
                    ellipseY = 0 - _ellipseHsv.ActualHeight/2;
                }
                else
                {
                    if (point.Y > _canvasHsv.ActualHeight)
                    {
                        ellipseY = _canvasHsv.ActualHeight - _ellipseHsv.ActualHeight/2;
                    }
                    else
                    {
                        ellipseY = point.Y - _ellipseHsv.ActualHeight/2;
                    }
                }

                _ellipseHsv.SetValue(Canvas.LeftProperty, ellipseX);
                _ellipseHsv.SetValue(Canvas.TopProperty, ellipseY);

                if (Updating)
                {
                    return;
                }

                Color color = GetHSVColor();
                UpdateControls(color, false, true, true);
            }
        }

        /// <summary>
        /// The initialize predefined.
        /// </summary>
        private void InitializePredefined()
        {
            if (_dictionaryColor != null)
            {
                return;
            }

            List<PredefinedColor> list = PredefinedColor.All;
            _dictionaryColor = new Dictionary<Color, PredefinedColorItem>();
            foreach (PredefinedColor color in list)
            {
                var item = new PredefinedColorItem(color.Value, color.Name);
                _comboBoxColor.Items.Add(item);

                if (!_dictionaryColor.ContainsKey(color.Value))
                {
                    _dictionaryColor.Add(color.Value, item);
                }
            }
        }

        /// <summary>
        /// The initialize theme colors.
        /// </summary>
        private void InitializeThemeColors()
        {
            if (_themeColors != null)
            {
                return;
            }

            List<PredefinedColor> list = PredefinedColor.AllThemeColors;
            _themeColors = new Dictionary<Color, PredefinedColorItem>();
            int r = 0;
            int c = 0;
            foreach (PredefinedColor color in list)
            {
                var item = new PredefinedColorItem(color.Value, color.Name);
                item.SetValue(Grid.RowProperty, r);
                item.SetValue(Grid.ColumnProperty, c);

                // item.Style = themeColorsGrid.Resources["ThemeColorItemStyle"] as Style;
                _themeColorsGrid.Items.Add(item);

                if (!_themeColors.ContainsKey(color.Value))
                {
                    _themeColors.Add(color.Value, item);
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

        /// <summary>
        /// The update controls.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="hsv">The hsv.</param>
        /// <param name="rgb">The rgb.</param>
        /// <param name="predifined">The predifined.</param>
        private void UpdateControls(Color color, bool hsv, bool rgb, bool predifined)
        {
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
                    double h = ColorHelper.GetHSV_H(color);
                    double s = ColorHelper.GetHSV_S(color);
                    double v = ColorHelper.GetHSV_V(color);

                    _sliderHsv.Value = h;
                    _gradientStopHsvColor.Color = ColorHelper.HSV2RGB(h, 1d, 1d);

                    double x = s*(_rectangleHsv.ActualWidth - 1);
                    double y = (1 - v)*(_rectangleHsv.ActualHeight - 1);

                    _ellipseHsv.SetValue(Canvas.LeftProperty, x - _ellipseHsv.ActualWidth/2);
                    _ellipseHsv.SetValue(Canvas.TopProperty, y - _ellipseHsv.ActualHeight/2);
                }

                if (rgb)
                {
                    byte a = color.A;
                    byte r = color.R;
                    byte g = color.G;
                    byte b = color.B;

                    _sliderA.Value = a;
                    _gradientStopA0.Color = Color.FromArgb(0, r, g, b);
                    _gradientStopA1.Color = Color.FromArgb(255, r, g, b);
                    _textBoxA.Text = a.ToString("X2");

                    _sliderR.Value = r;
                    _gradientStopR0.Color = Color.FromArgb(255, 0, g, b);
                    _gradientStopR1.Color = Color.FromArgb(255, 255, g, b);
                    _textBoxR.Text = r.ToString("X2");

                    _sliderG.Value = g;
                    _gradientStopG0.Color = Color.FromArgb(255, r, 0, b);
                    _gradientStopG1.Color = Color.FromArgb(255, r, 255, b);
                    _textBoxG.Text = g.ToString("X2");

                    _sliderB.Value = b;
                    _gradientStopB0.Color = Color.FromArgb(255, r, g, 0);
                    _gradientStopB1.Color = Color.FromArgb(255, r, g, 255);
                    _textBoxB.Text = b.ToString("X2");
                }

                if (predifined)
                {
                    _brushColor.Color = color;
                    if (_dictionaryColor.ContainsKey(color))
                    {
                        _comboBoxColor.SelectedItem = _dictionaryColor[color];
                        _textBoxColor.Text = string.Empty;
                    }
                    else
                    {
                        _comboBoxColor.SelectedItem = null;
                        _textBoxColor.Text = color.ToString();
                    }
                }

                Color = color;
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
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            OnCancelClicked();
        }

        /// <summary>
        /// The button done_ click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void buttonDone_Click(object sender, RoutedEventArgs e)
        {
            OnDoneClicked();
        }

        /// <summary>
        /// The combo box color_ selection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void comboBoxColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            var coloritem = _comboBoxColor.SelectedItem as PredefinedColorItem;
            if (coloritem != null)
            {
                Color = coloritem.Color;
            }
        }

        /// <summary>
        /// The recent colors grid_ selection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void recentColorsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_recentColorsGrid.SelectedItems.Count == 1)
            {
                Color c = ((PredefinedColorItem) _recentColorsGrid.SelectedItem).Color;
                UpdateControls(c, true, true, true);
            }
        }

        /// <summary>
        /// The slider a_ value changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void sliderA_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Updating)
            {
                return;
            }

            Color color = GetRGBColor();
            UpdateControls(color, true, true, true);
        }

        /// <summary>
        /// The slider b_ value changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void sliderB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Updating)
            {
                return;
            }

            Color color = GetRGBColor();
            UpdateControls(color, true, true, true);
        }

        /// <summary>
        /// The slider g_ value changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void sliderG_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Updating)
            {
                return;
            }

            Color color = GetRGBColor();
            UpdateControls(color, true, true, true);
        }

        /// <summary>
        /// The slider hs v_ value changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void sliderHSV_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Updating)
            {
                return;
            }

            _gradientStopHsvColor.Color = ColorHelper.HSV2RGB(e.NewValue, 1d, 1d);

            Color color = GetHSVColor();
            UpdateControls(color, false, true, true);
        }

        /// <summary>
        /// The slider r_ value changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void sliderR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Updating)
            {
                return;
            }

            Color color = GetRGBColor();
            UpdateControls(color, true, true, true);
        }

        /// <summary>
        /// The text box a_ lost focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void textBoxA_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(_textBoxA.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                _sliderA.Value = value;
            }
        }

        /// <summary>
        /// The text box b_ lost focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void textBoxB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(_textBoxB.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                _sliderB.Value = value;
            }
        }

        /// <summary>
        /// The text box color_ got focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void textBoxColor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            try
            {
                BeginUpdate();

                _comboBoxColor.SelectedItem = null;
                _textBoxColor.Text = Color.ToString();
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
        private void textBoxColor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            string text = _textBoxColor.Text.TrimStart('#');
            uint value = 0;
            if (uint.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                var b = (byte) (value & 0xFF);
                value >>= 8;
                var g = (byte) (value & 0xFF);
                value >>= 8;
                var r = (byte) (value & 0xFF);
                value >>= 8;
                var a = (byte) (value & 0xFF);

                if (text.Length <= 6)
                {
                    a = 0xFF;
                }

                Color color = Color.FromArgb(a, r, g, b);
                Color = color;
            }
            else
            {
                Color = Colors.White;
            }
        }

        /// <summary>
        /// The text box g_ lost focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void textBoxG_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(_textBoxG.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                _sliderG.Value = value;
            }
        }

        /// <summary>
        /// The text box r_ lost focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void textBoxR_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(_textBoxR.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                _sliderR.Value = value;
            }
        }

        /// <summary>
        /// The theme colors grid_ selection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void themeColorsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_themeColorsGrid.SelectedItems.Count == 1)
            {
                Color c = ((PredefinedColorItem) _themeColorsGrid.SelectedItem).Color;
                UpdateControls(c, true, true, true);
            }
        }
        #endregion
    }
}