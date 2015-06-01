// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorPicker.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// The color picker.
    /// </summary>
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_ToggleDropDown", Type = typeof(ToggleButton))]
    public class ColorPicker : Control
    {
        #region Static Fields

        /// <summary>
        /// The color property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(ColorPicker), new PropertyMetadata(Colors.White, OnColorPropertyChaged));

        /// <summary>
        /// The current color property.
        /// </summary>
        public static readonly DependencyProperty CurrentColorProperty = DependencyProperty.Register(
            "CurrentColor", typeof(Color), typeof(ColorPicker), new PropertyMetadata(Colors.White));

        /// <summary>
        /// The is drop down open property.
        /// </summary>
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
            "IsDropDownOpen",
            typeof(bool),
            typeof(ColorPicker),
            new PropertyMetadata(false, OnIsDropDownOpenPropertyChanged));

        /// <summary>
        /// The popup placement property.
        /// </summary>
        public static readonly DependencyProperty PopupPlacementProperty = DependencyProperty.Register(
            "PopupPlacement", typeof(PlacementMode), typeof(ColorPicker), new PropertyMetadata(PlacementMode.Bottom));

        #endregion

        #region Fields

        /// <summary>
        /// The color board.
        /// </summary>
        private ColorBoard colorBoard;

        /// <summary>
        /// The popup.
        /// </summary>
        private Popup popup;

        /// <summary>
        /// The toggle drop down.
        /// </summary>
        private ToggleButton toggleDropDown;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker"/> class.
        /// </summary>
        public ColorPicker()
        {
            this.DefaultStyleKey = typeof(ColorPicker);
        }

        #endregion

        #region Public Events

        /// <summary>
        /// The color changed.
        /// </summary>
        public event EventHandler<ColorChangedEventArgs> ColorChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public Color Color
        {
            get
            {
                return (Color)this.GetValue(ColorProperty);
            }

            set
            {
                this.SetValue(ColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the current color.
        /// </summary>
        public Color CurrentColor
        {
            get
            {
                return (Color)this.GetValue(CurrentColorProperty);
            }

            set
            {
                this.SetValue(CurrentColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is drop down open.
        /// </summary>
        public bool IsDropDownOpen
        {
            get
            {
                return (bool)this.GetValue(IsDropDownOpenProperty);
            }

            set
            {
                this.SetValue(IsDropDownOpenProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the popup placement.
        /// </summary>
        public PlacementMode PopupPlacement
        {
            get
            {
                return (PlacementMode)this.GetValue(PopupPlacementProperty);
            }

            set
            {
                this.SetValue(PopupPlacementProperty, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The on apply template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.popup = (Popup)this.GetTemplateChild("PART_Popup");
            this.toggleDropDown = (ToggleButton)this.GetTemplateChild("PART_ToggleButton");
            this.colorBoard = new ColorBoard();
            this.colorBoard.Color = this.Color;
            this.colorBoard.SizeChanged += this.colorBoard_SizeChanged;
            this.popup.Child = this.colorBoard;
            this.colorBoard.DoneClicked += this.colorBoard_DoneClicked;
            this.colorBoard.CancelClicked += this.colorBoard_CancelClicked;

            var b = new Binding("Color");
            b.Mode = BindingMode.TwoWay;
            b.Source = this.colorBoard;
            this.SetBinding(CurrentColorProperty, b);

            this.KeyDown += this.ColorPicker_KeyDown;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on color property chaged.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void OnColorPropertyChaged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cp = (ColorPicker)d;
            if (cp == null)
            {
                return;
            }

            cp.OnColorChanged((Color)e.NewValue, (Color)e.OldValue);
        }

        /// <summary>
        /// The on is drop down open property changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void OnIsDropDownOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cp = (ColorPicker)d;
            if (cp == null)
            {
                return;
            }
        }

        /// <summary>
        /// The color picker_ key down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ColorPicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && this.IsDropDownOpen)
            {
                this.colorBoard.OnDoneClicked();
            }
        }

        /// <summary>
        /// The on color changed.
        /// </summary>
        /// <param name="newColor">
        /// The new color.
        /// </param>
        /// <param name="oldColor">
        /// The old color.
        /// </param>
        private void OnColorChanged(Color newColor, Color oldColor)
        {
            if (this.ColorChanged != null)
            {
                this.ColorChanged(this, new ColorChangedEventArgs(newColor, oldColor));
            }
        }

        /// <summary>
        /// The color board_ done clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void colorBoard_DoneClicked(object sender, RoutedEventArgs e)
        {
            this.Color = this.colorBoard.Color;
            this.popup.IsOpen = false;
        }

        /// <summary>
        /// The color board_ cancel clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        void colorBoard_CancelClicked(object sender, RoutedEventArgs e)
        {
            this.colorBoard.Color = this.CurrentColor = this.Color;
            this.popup.IsOpen = false;
        }

        /// <summary>
        /// The color board_ size changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void colorBoard_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.PopupPlacement == PlacementMode.Bottom)
            {
                this.popup.VerticalOffset = this.ActualHeight;
            }

            if (this.PopupPlacement == PlacementMode.Top)
            {
                this.popup.VerticalOffset = -1 * this.colorBoard.ActualHeight;
            }

            if (this.PopupPlacement == PlacementMode.Right)
            {
                this.popup.HorizontalOffset = this.ActualWidth;
            }

            if (this.PopupPlacement == PlacementMode.Left)
            {
                this.popup.HorizontalOffset = -1 * this.colorBoard.ActualWidth;
            }
        }

        #endregion
    }
}