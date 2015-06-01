// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorBoard.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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
            this.DefaultStyleKey = typeof (ColorBoard);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether updating.
        /// </summary>
        private bool Updating
        {
            get { return this.isUpdating != 0; }
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
        private SolidColorBrush brushColor;

        /// <summary>
        /// The button cancel.
        /// </summary>
        private Button buttonCancel;

        /// <summary>
        /// The button done.
        /// </summary>
        private Button buttonDone;

        /// <summary>
        /// The canvas hsv.
        /// </summary>
        private Canvas canvasHSV;

        /// <summary>
        /// The combo box color.
        /// </summary>
        private ComboBox comboBoxColor;

        /// <summary>
        /// The dictionary color.
        /// </summary>
        private Dictionary<Color, PredefinedColorItem> dictionaryColor;

        /// <summary>
        /// The ellipse hsv.
        /// </summary>
        private Ellipse ellipseHSV;

        /// <summary>
        /// The gradient stop a 0.
        /// </summary>
        private GradientStop gradientStopA0;

        /// <summary>
        /// The gradient stop a 1.
        /// </summary>
        private GradientStop gradientStopA1;

        /// <summary>
        /// The gradient stop b 0.
        /// </summary>
        private GradientStop gradientStopB0;

        /// <summary>
        /// The gradient stop b 1.
        /// </summary>
        private GradientStop gradientStopB1;

        /// <summary>
        /// The gradient stop g 0.
        /// </summary>
        private GradientStop gradientStopG0;

        /// <summary>
        /// The gradient stop g 1.
        /// </summary>
        private GradientStop gradientStopG1;

        /// <summary>
        /// The gradient stop hsv color.
        /// </summary>
        private GradientStop gradientStopHSVColor;

        /// <summary>
        /// The gradient stop r 0.
        /// </summary>
        private GradientStop gradientStopR0;

        /// <summary>
        /// The gradient stop r 1.
        /// </summary>
        private GradientStop gradientStopR1;

        /// <summary>
        /// The is updating.
        /// </summary>
        private int isUpdating;

        /// <summary>
        /// The recent colors grid.
        /// </summary>
        private ListBox recentColorsGrid;

        /// <summary>
        /// The rectangle color.
        /// </summary>
        private Rectangle rectangleColor;

        /// <summary>
        /// The rectangle hsv.
        /// </summary>
        private Rectangle rectangleHSV;

        /// <summary>
        /// The rectangle root hsv.
        /// </summary>
        private Rectangle rectangleRootHSV;

        /// <summary>
        /// The root element.
        /// </summary>
        private FrameworkElement rootElement;

        /// <summary>
        /// The slider a.
        /// </summary>
        private Slider sliderA;

        /// <summary>
        /// The slider b.
        /// </summary>
        private Slider sliderB;

        /// <summary>
        /// The slider g.
        /// </summary>
        private Slider sliderG;

        /// <summary>
        /// The slider hsv.
        /// </summary>
        private Slider sliderHSV;

        /// <summary>
        /// The slider r.
        /// </summary>
        private Slider sliderR;

        /// <summary>
        /// The text box a.
        /// </summary>
        private TextBox textBoxA;

        /// <summary>
        /// The text box b.
        /// </summary>
        private TextBox textBoxB;

        /// <summary>
        /// The text box color.
        /// </summary>
        private TextBox textBoxColor;

        /// <summary>
        /// The text box g.
        /// </summary>
        private TextBox textBoxG;

        /// <summary>
        /// The text box r.
        /// </summary>
        private TextBox textBoxR;

        /// <summary>
        /// The theme colors.
        /// </summary>
        private Dictionary<Color, PredefinedColorItem> themeColors;

        /// <summary>
        /// The theme colors grid.
        /// </summary>
        private ListBox themeColorsGrid;

        /// <summary>
        /// The tracking hsv.
        /// </summary>
        private bool trackingHSV;
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
            get { return (Color) this.GetValue(ColorProperty); }

            set { this.SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the recent color items.
        /// </summary>
        public List<PredefinedColorItem> RecentColorItems
        {
            get { return (List<PredefinedColorItem>) this.GetValue(RecentColorItemsProperty); }

            set { this.SetValue(RecentColorItemsProperty, value); }
        }
        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// The on apply template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.rootElement = (FrameworkElement) this.GetTemplateChild("RootElement");

            this.canvasHSV = (Canvas) this.GetTemplateChild("CanvasHSV");
            this.rectangleRootHSV = (Rectangle) this.GetTemplateChild("RectangleRootHSV");
            this.gradientStopHSVColor = (GradientStop) this.GetTemplateChild("GradientStopHSVColor");
            this.rectangleHSV = (Rectangle) this.GetTemplateChild("RectangleHSV");
            this.ellipseHSV = (Ellipse) this.GetTemplateChild("EllipseHSV");
            this.sliderHSV = (Slider) this.GetTemplateChild("SliderHSV");

            this.sliderA = (Slider) this.GetTemplateChild("SliderA");
            this.gradientStopA0 = (GradientStop) this.GetTemplateChild("GradientStopA0");
            this.gradientStopA1 = (GradientStop) this.GetTemplateChild("GradientStopA1");
            this.sliderR = (Slider) this.GetTemplateChild("SliderR");
            this.gradientStopR0 = (GradientStop) this.GetTemplateChild("GradientStopR0");
            this.gradientStopR1 = (GradientStop) this.GetTemplateChild("GradientStopR1");
            this.sliderG = (Slider) this.GetTemplateChild("SliderG");
            this.gradientStopG0 = (GradientStop) this.GetTemplateChild("GradientStopG0");
            this.gradientStopG1 = (GradientStop) this.GetTemplateChild("GradientStopG1");
            this.sliderB = (Slider) this.GetTemplateChild("SliderB");
            this.gradientStopB0 = (GradientStop) this.GetTemplateChild("GradientStopB0");
            this.gradientStopB1 = (GradientStop) this.GetTemplateChild("GradientStopB1");

            this.textBoxA = (TextBox) this.GetTemplateChild("TextBoxA");
            this.textBoxR = (TextBox) this.GetTemplateChild("TextBoxR");
            this.textBoxG = (TextBox) this.GetTemplateChild("TextBoxG");
            this.textBoxB = (TextBox) this.GetTemplateChild("TextBoxB");

            this.comboBoxColor = (ComboBox) this.GetTemplateChild("ComboBoxColor");
            this.rectangleColor = (Rectangle) this.GetTemplateChild("RectangleColor");
            this.brushColor = (SolidColorBrush) this.GetTemplateChild("BrushColor");
            this.textBoxColor = (TextBox) this.GetTemplateChild("TextBoxColor");
            this.buttonDone = (Button) this.GetTemplateChild("ButtonDone");
            this.buttonCancel = (Button) this.GetTemplateChild("ButtonCancel");
            this.themeColorsGrid = (ListBox) this.GetTemplateChild("ThemeColorsGrid");
            this.recentColorsGrid = (ListBox) this.GetTemplateChild("RecentColorsGrid");

            this.themeColorsGrid.SelectionChanged += this.themeColorsGrid_SelectionChanged;
            this.recentColorsGrid.SelectionChanged += this.recentColorsGrid_SelectionChanged;

            this.rectangleHSV.MouseLeftButtonDown += this.HSV_MouseLeftButtonDown;
            this.rectangleHSV.MouseMove += this.HSV_MouseMove;
            this.rectangleHSV.MouseLeftButtonUp += this.HSV_MouseLeftButtonUp;
            this.rectangleHSV.MouseLeave += this.HSV_MouseLeave;

            this.sliderHSV.ValueChanged += this.sliderHSV_ValueChanged;

            this.sliderA.ValueChanged += this.sliderA_ValueChanged;
            this.sliderR.ValueChanged += this.sliderR_ValueChanged;
            this.sliderG.ValueChanged += this.sliderG_ValueChanged;
            this.sliderB.ValueChanged += this.sliderB_ValueChanged;

            this.textBoxA.LostFocus += this.textBoxA_LostFocus;
            this.textBoxR.LostFocus += this.textBoxR_LostFocus;
            this.textBoxG.LostFocus += this.textBoxG_LostFocus;
            this.textBoxB.LostFocus += this.textBoxB_LostFocus;

            this.comboBoxColor.SelectionChanged += this.comboBoxColor_SelectionChanged;
            this.textBoxColor.GotFocus += this.textBoxColor_GotFocus;
            this.textBoxColor.LostFocus += this.textBoxColor_LostFocus;
            this.buttonDone.Click += this.buttonDone_Click;
            this.buttonCancel.Click += this.buttonCancel_Click;

            this.InitializePredefined();
            this.InitializeThemeColors();
            this.UpdateControls(this.Color, true, true, true);

            this.KeyDown += this.ColorBoard_KeyDown;
        }

        /// <summary>
        /// The on cancel clicked.
        /// </summary>
        public void OnCancelClicked()
        {
            if (this.CancelClicked != null)
            {
                this.CancelClicked(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// The on done clicked.
        /// </summary>
        public void OnDoneClicked()
        {
            var ci = new PredefinedColorItem(this.Color, this.Color.ToString());

            if (this.RecentColorItems.Where(i => i.Color == ci.Color).Count() > 0)
            {
                this.recentColorsGrid.Items.RemoveAt(
                    this.recentColorsGrid.Items.IndexOf(this.RecentColorItems.First(i => i.Color == ci.Color)));
                this.RecentColorItems.Remove(this.RecentColorItems.First(i => i.Color == ci.Color));

                this.recentColorsGrid.Items.Insert(0, ci);
                this.RecentColorItems.Insert(0, ci);
            }
            else
            {
                this.RecentColorItems.Insert(0, ci);
                this.recentColorsGrid.Items.Insert(0, ci);
            }

            if (this.DoneClicked != null)
            {
                this.DoneClicked(this, new RoutedEventArgs());
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// The on color property changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
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
            this.isUpdating++;
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
                this.OnDoneClicked();
            }
        }

        /// <summary>
        /// The end update.
        /// </summary>
        private void EndUpdate()
        {
            this.isUpdating--;
        }

        /// <summary>
        /// The get hsv color.
        /// </summary>
        /// <returns>
        /// The <see cref="Color"/>.
        /// </returns>
        private Color GetHSVColor()
        {
            double h = this.sliderHSV.Value;

            double x = (double) this.ellipseHSV.GetValue(Canvas.LeftProperty) + this.ellipseHSV.ActualWidth/2;
            double y = (double) this.ellipseHSV.GetValue(Canvas.TopProperty) + this.ellipseHSV.ActualHeight/2;

            double s = x/(this.rectangleHSV.ActualWidth - 1);
            if (s < 0d)
            {
                s = 0d;
            }
            else if (s > 1d)
            {
                s = 1d;
            }

            double v = 1 - y/(this.rectangleHSV.ActualHeight - 1);
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
        /// <returns>
        /// The <see cref="Color"/>.
        /// </returns>
        private Color GetRGBColor()
        {
            var a = (byte) this.sliderA.Value;
            var r = (byte) this.sliderR.Value;
            var g = (byte) this.sliderG.Value;
            var b = (byte) this.sliderB.Value;

            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// The hs v_ mouse leave.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void HSV_MouseLeave(object sender, MouseEventArgs e)
        {
            this.trackingHSV = false;
            this.rectangleHSV.ReleaseMouseCapture();
        }

        /// <summary>
        /// The hs v_ mouse left button down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void HSV_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.trackingHSV = this.rectangleHSV.CaptureMouse();

            Point point = e.GetPosition(this.rectangleHSV);

            Size size = this.ellipseHSV.RenderSize;

            this.ellipseHSV.SetValue(Canvas.LeftProperty, point.X - this.ellipseHSV.ActualWidth/2);
            this.ellipseHSV.SetValue(Canvas.TopProperty, point.Y - this.ellipseHSV.ActualHeight/2);

            if (this.Updating)
            {
                return;
            }

            Color color = this.GetHSVColor();
            this.UpdateControls(color, false, true, true);
        }

        /// <summary>
        /// The hs v_ mouse left button up.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void HSV_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.trackingHSV = false;

            this.rectangleHSV.ReleaseMouseCapture();
        }

        /// <summary>
        /// The hs v_ mouse move.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void HSV_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.trackingHSV)
            {
                Point point = e.GetPosition(this.rectangleHSV);
                Size size = this.ellipseHSV.RenderSize;

                double ellipseX = 0;
                if (point.X < 0)
                {
                    ellipseX = 0 - this.ellipseHSV.ActualWidth/2;
                }
                else
                {
                    if (point.X > this.canvasHSV.ActualWidth)
                    {
                        ellipseX = this.canvasHSV.ActualWidth - this.ellipseHSV.ActualWidth/2;
                    }
                    else
                    {
                        ellipseX = point.X - this.ellipseHSV.ActualWidth/2;
                    }
                }

                double ellipseY = 0;
                if (point.Y < 0)
                {
                    ellipseY = 0 - this.ellipseHSV.ActualHeight/2;
                }
                else
                {
                    if (point.Y > this.canvasHSV.ActualHeight)
                    {
                        ellipseY = this.canvasHSV.ActualHeight - this.ellipseHSV.ActualHeight/2;
                    }
                    else
                    {
                        ellipseY = point.Y - this.ellipseHSV.ActualHeight/2;
                    }
                }

                this.ellipseHSV.SetValue(Canvas.LeftProperty, ellipseX);
                this.ellipseHSV.SetValue(Canvas.TopProperty, ellipseY);

                if (this.Updating)
                {
                    return;
                }

                Color color = this.GetHSVColor();
                this.UpdateControls(color, false, true, true);
            }
        }

        /// <summary>
        /// The initialize predefined.
        /// </summary>
        private void InitializePredefined()
        {
            if (this.dictionaryColor != null)
            {
                return;
            }

            List<PredefinedColor> list = PredefinedColor.All;
            this.dictionaryColor = new Dictionary<Color, PredefinedColorItem>();
            foreach (PredefinedColor color in list)
            {
                var item = new PredefinedColorItem(color.Value, color.Name);
                this.comboBoxColor.Items.Add(item);

                if (!this.dictionaryColor.ContainsKey(color.Value))
                {
                    this.dictionaryColor.Add(color.Value, item);
                }
            }
        }

        /// <summary>
        /// The initialize theme colors.
        /// </summary>
        private void InitializeThemeColors()
        {
            if (this.themeColors != null)
            {
                return;
            }

            List<PredefinedColor> list = PredefinedColor.AllThemeColors;
            this.themeColors = new Dictionary<Color, PredefinedColorItem>();
            int r = 0;
            int c = 0;
            foreach (PredefinedColor color in list)
            {
                var item = new PredefinedColorItem(color.Value, color.Name);
                item.SetValue(Grid.RowProperty, r);
                item.SetValue(Grid.ColumnProperty, c);

                // item.Style = themeColorsGrid.Resources["ThemeColorItemStyle"] as Style;
                this.themeColorsGrid.Items.Add(item);

                if (!this.themeColors.ContainsKey(color.Value))
                {
                    this.themeColors.Add(color.Value, item);
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
        /// <param name="color">
        /// The color.
        /// </param>
        /// <param name="hsv">
        /// The hsv.
        /// </param>
        /// <param name="rgb">
        /// The rgb.
        /// </param>
        /// <param name="predifined">
        /// The predifined.
        /// </param>
        private void UpdateControls(Color color, bool hsv, bool rgb, bool predifined)
        {
            if (this.Updating)
            {
                return;
            }

            try
            {
                this.BeginUpdate();

                // HSV
                if (hsv)
                {
                    double h = ColorHelper.GetHSV_H(color);
                    double s = ColorHelper.GetHSV_S(color);
                    double v = ColorHelper.GetHSV_V(color);

                    this.sliderHSV.Value = h;
                    this.gradientStopHSVColor.Color = ColorHelper.HSV2RGB(h, 1d, 1d);

                    double x = s*(this.rectangleHSV.ActualWidth - 1);
                    double y = (1 - v)*(this.rectangleHSV.ActualHeight - 1);

                    this.ellipseHSV.SetValue(Canvas.LeftProperty, x - this.ellipseHSV.ActualWidth/2);
                    this.ellipseHSV.SetValue(Canvas.TopProperty, y - this.ellipseHSV.ActualHeight/2);
                }

                if (rgb)
                {
                    byte a = color.A;
                    byte r = color.R;
                    byte g = color.G;
                    byte b = color.B;

                    this.sliderA.Value = a;
                    this.gradientStopA0.Color = Color.FromArgb(0, r, g, b);
                    this.gradientStopA1.Color = Color.FromArgb(255, r, g, b);
                    this.textBoxA.Text = a.ToString("X2");

                    this.sliderR.Value = r;
                    this.gradientStopR0.Color = Color.FromArgb(255, 0, g, b);
                    this.gradientStopR1.Color = Color.FromArgb(255, 255, g, b);
                    this.textBoxR.Text = r.ToString("X2");

                    this.sliderG.Value = g;
                    this.gradientStopG0.Color = Color.FromArgb(255, r, 0, b);
                    this.gradientStopG1.Color = Color.FromArgb(255, r, 255, b);
                    this.textBoxG.Text = g.ToString("X2");

                    this.sliderB.Value = b;
                    this.gradientStopB0.Color = Color.FromArgb(255, r, g, 0);
                    this.gradientStopB1.Color = Color.FromArgb(255, r, g, 255);
                    this.textBoxB.Text = b.ToString("X2");
                }

                if (predifined)
                {
                    this.brushColor.Color = color;
                    if (this.dictionaryColor.ContainsKey(color))
                    {
                        this.comboBoxColor.SelectedItem = this.dictionaryColor[color];
                        this.textBoxColor.Text = string.Empty;
                    }
                    else
                    {
                        this.comboBoxColor.SelectedItem = null;
                        this.textBoxColor.Text = color.ToString();
                    }
                }

                this.Color = color;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        /// <summary>
        /// The button cancel_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.OnCancelClicked();
        }

        /// <summary>
        /// The button done_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void buttonDone_Click(object sender, RoutedEventArgs e)
        {
            this.OnDoneClicked();
        }

        /// <summary>
        /// The combo box color_ selection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void comboBoxColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Updating)
            {
                return;
            }

            var coloritem = this.comboBoxColor.SelectedItem as PredefinedColorItem;
            if (coloritem != null)
            {
                this.Color = coloritem.Color;
            }
        }

        /// <summary>
        /// The recent colors grid_ selection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void recentColorsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.recentColorsGrid.SelectedItems.Count == 1)
            {
                Color c = ((PredefinedColorItem) this.recentColorsGrid.SelectedItem).Color;
                this.UpdateControls(c, true, true, true);
            }
        }

        /// <summary>
        /// The slider a_ value changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void sliderA_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.Updating)
            {
                return;
            }

            Color color = this.GetRGBColor();
            this.UpdateControls(color, true, true, true);
        }

        /// <summary>
        /// The slider b_ value changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void sliderB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.Updating)
            {
                return;
            }

            Color color = this.GetRGBColor();
            this.UpdateControls(color, true, true, true);
        }

        /// <summary>
        /// The slider g_ value changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void sliderG_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.Updating)
            {
                return;
            }

            Color color = this.GetRGBColor();
            this.UpdateControls(color, true, true, true);
        }

        /// <summary>
        /// The slider hs v_ value changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void sliderHSV_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.Updating)
            {
                return;
            }

            this.gradientStopHSVColor.Color = ColorHelper.HSV2RGB(e.NewValue, 1d, 1d);

            Color color = this.GetHSVColor();
            this.UpdateControls(color, false, true, true);
        }

        /// <summary>
        /// The slider r_ value changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void sliderR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.Updating)
            {
                return;
            }

            Color color = this.GetRGBColor();
            this.UpdateControls(color, true, true, true);
        }

        /// <summary>
        /// The text box a_ lost focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void textBoxA_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(this.textBoxA.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                this.sliderA.Value = value;
            }
        }

        /// <summary>
        /// The text box b_ lost focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void textBoxB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(this.textBoxB.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                this.sliderB.Value = value;
            }
        }

        /// <summary>
        /// The text box color_ got focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void textBoxColor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.Updating)
            {
                return;
            }

            try
            {
                this.BeginUpdate();

                this.comboBoxColor.SelectedItem = null;
                this.textBoxColor.Text = this.Color.ToString();
            }
            finally
            {
                this.EndUpdate();
            }
        }

        /// <summary>
        /// The text box color_ lost focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void textBoxColor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Updating)
            {
                return;
            }

            string text = this.textBoxColor.Text.TrimStart('#');
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
                this.Color = color;
            }
            else
            {
                this.Color = Colors.White;
            }
        }

        /// <summary>
        /// The text box g_ lost focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void textBoxG_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(this.textBoxG.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                this.sliderG.Value = value;
            }
        }

        /// <summary>
        /// The text box r_ lost focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void textBoxR_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(this.textBoxR.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                this.sliderR.Value = value;
            }
        }

        /// <summary>
        /// The theme colors grid_ selection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void themeColorsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.themeColorsGrid.SelectedItems.Count == 1)
            {
                Color c = ((PredefinedColorItem) this.themeColorsGrid.SelectedItem).Color;
                this.UpdateControls(c, true, true, true);
            }
        }
        #endregion
    }
}