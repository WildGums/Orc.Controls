// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropDownButton.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using Catel;

    /// <summary>
    /// The arrow location.
    /// </summary>
    public enum DropdownArrowLocation
    {
        /// <summary>
        /// The left arrow location
        /// </summary>
        Left,

        /// <summary>
        /// The top arrow location
        /// </summary>
        Top,

        /// <summary>
        /// The right arrow location
        /// </summary>
        Right,

        /// <summary>
        /// The bottom arrow location
        /// </summary>
        Bottom
    }

    /// <summary>
    /// Interaction logic for DropDownButton.xaml
    /// </summary>
    public partial class DropDownButton
    {
        #region Constructors
        public DropDownButton()
        {
            InitializeComponent();

            LayoutUpdated += OnLayoutUpdated;
        }
        #endregion

        #region Properties
        public DropdownArrowLocation ArrowLocation
        {
            get { return (DropdownArrowLocation) GetValue(ArrowLocationProperty); }
            set { SetValue(ArrowLocationProperty, value); }
        }

        public static readonly DependencyProperty ArrowLocationProperty = DependencyProperty.Register("ArrowLocation", typeof(DropdownArrowLocation), typeof(DropDownButton), new PropertyMetadata(DropdownArrowLocation.Right));

        public Thickness ArrowMargin
        {
            get { return (Thickness) GetValue(ArrowMarginProperty); }
            set { SetValue(ArrowMarginProperty, value); }
        }

        public static readonly DependencyProperty ArrowMarginProperty = DependencyProperty.Register("ArrowMargin", typeof(Thickness), typeof(DropDownButton), new PropertyMetadata(default(Thickness)));

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object),
            typeof(DropDownButton), new FrameworkPropertyMetadata(string.Empty));

        public ContextMenu DropDown
        {
            get { return (ContextMenu)GetValue(DropDownProperty); }
            set { SetValue(DropDownProperty, value); }
        }

        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register("DropDown", typeof(ContextMenu),
            typeof(DropDownButton), new UIPropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand),
            typeof(DropDownButton), new UIPropertyMetadata(null));

        public Brush AccentColorBrush
        {
            get { return (Brush)GetValue(AccentColorBrushProperty); }
            set { SetValue(AccentColorBrushProperty, value); }
        }

        public static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.Register("AccentColorBrush", typeof(Brush),
            typeof(DropDownButton), new PropertyMetadata(Brushes.LightGray, (sender, e) => ((DropDownButton)sender).OnAccentColorBrushChanged()));

        public bool ShowDefaultButton
        {
            get { return (bool)GetValue(ShowDefaultButtonProperty); }
            set { SetValue(ShowDefaultButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowDefaultButtonProperty = DependencyProperty.Register("ShowDefaultButton", typeof(bool),
            typeof(DropDownButton), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool EnableTransparentBackground
        {
            get { return (bool)GetValue(EnableTransparentBackgroundProperty); }
            set { SetValue(EnableTransparentBackgroundProperty, value); }
        }

        public object CommandParameter
        {
            get
            {
                return (object)GetValue(CommandParameterProperty);
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        public static readonly DependencyProperty EnableTransparentBackgroundProperty = DependencyProperty.Register("EnableTransparentBackground", typeof(bool),
            typeof(DropDownButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(DropDownButton), new PropertyMetadata(default(object)));
        #endregion

        #region Events
        public event EventHandler<EventArgs> ContentLayoutUpdated;
        #endregion

        #region Methods
        private void OnAccentColorBrushChanged()
        {
            var solidColorBrush = AccentColorBrush as SolidColorBrush;
            if (solidColorBrush != null)
            {
                var accentColor = ((SolidColorBrush) AccentColorBrush).Color;
                accentColor.CreateAccentColorResourceDictionary("DropDownButton");
            }
        }

        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            ContentLayoutUpdated.SafeInvoke(this, e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetCurrentValue(AccentColorBrushProperty, TryFindResource("AccentColorBrush") as SolidColorBrush);
        }
        #endregion
    }
}