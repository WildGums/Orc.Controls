// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WatermarkTextBox.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


#if NET || NETCORE

namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Catel.Windows.Data;

    /// <summary>
    /// WatermarkTextBox which is a simple <see cref="TextBox"/> that is able to show simple and complex watermarks.
    /// </summary>
    [TemplatePart(Name = "PART_WatermarkHost", Type = typeof(ContentPresenter))]
    public class WatermarkTextBox : TextBox
    {
        #region Fields
        private ContentPresenter _watermarkHost;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Controls.TextBox"/> class.
        /// </summary>
        static WatermarkTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkTextBox), new FrameworkPropertyMetadata(typeof(WatermarkTextBox)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Controls.TextBox"/> class.
        /// </summary>
        public WatermarkTextBox()
        {
            this.SubscribeToDependencyProperty(nameof(Padding), OnPaddingChanged);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether all text should be selected when the control receives the focus.
        /// </summary>
        /// <value><c>true</c> if all text should be selected when the control receives the focus; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        private bool SelectAllOnGotFocus
        {
            get { return (bool)GetValue(SelectAllOnGotFocusProperty); }
            set { SetValue(SelectAllOnGotFocusProperty, value); }
        }

        /// <summary>
        /// Dependency property registration for the <see cref="SelectAllOnGotFocus"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectAllOnGotFocusProperty = DependencyProperty.Register(nameof(SelectAllOnGotFocus),
            typeof(bool), typeof(WatermarkTextBox), new PropertyMetadata(false));


        /// <summary>
        /// Gets or sets the watermark to show.
        /// </summary>
        /// <value>The watermark.</value>
        /// <remarks></remarks>
        public object Watermark
        {
            get { return GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        /// <summary>
        /// Dependency property registration for the <see cref="Watermark"/> property.
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(nameof(Watermark),
            typeof(object), typeof(WatermarkTextBox), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the watermark template to show.
        /// </summary>
        /// <value>The watermark template.</value>
        /// <remarks></remarks>
        public DataTemplate WatermarkTemplate
        {
            get { return (DataTemplate)GetValue(WatermarkTemplateProperty); }
            set { SetValue(WatermarkTemplateProperty, value); }
        }

        /// <summary>
        /// Dependency property registration for the <see cref="WatermarkTemplate"/> property.
        /// </summary>
        public static readonly DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register(nameof(WatermarkTemplate),
            typeof(DataTemplate), typeof(WatermarkTextBox), new PropertyMetadata(null));
        #endregion

        #region Methods
        /// <summary>
        /// Invoked whenever an unhandled <c>System.Windows.Input.Keyboard.GotKeyboardFocus</c> attached routed event reaches an element derived from this class in its route. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">Provides data about the event.</param>
        /// <remarks></remarks>
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            if (SelectAllOnGotFocus)
            {
                SelectAll();
            }
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.UIElement.PreviewMouseLeftButtonDown"/> routed event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. The event data reports that the left mouse button was pressed.</param>
        /// <remarks></remarks>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!IsKeyboardFocused && SelectAllOnGotFocus)
            {
                e.Handled = true;
                Focus();
            }

            base.OnPreviewMouseLeftButtonDown(e);
        }

        public override void OnApplyTemplate()
        {
            _watermarkHost = GetTemplateChild("PART_WatermarkHost") as ContentPresenter;

            UpdateWaterMarkMargin();

            base.OnApplyTemplate();
        }

        private void OnPaddingChanged(object sender, DependencyPropertyValueChangedEventArgs e)
        {
            UpdateWaterMarkMargin();
        }

        private void UpdateWaterMarkMargin()
        {
            if (_watermarkHost is null)
            {
                return;
            }

            var padding = Padding;
            var margin = new Thickness(padding.Left + 2d, padding.Top, padding.Right + 2d, padding.Bottom);
            _watermarkHost.SetCurrentValue(MarginProperty, margin);
        }
        #endregion
    }
}

#endif
