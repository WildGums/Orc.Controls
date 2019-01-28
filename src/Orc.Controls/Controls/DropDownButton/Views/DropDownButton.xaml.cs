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

        public static readonly DependencyProperty ArrowLocationProperty = DependencyProperty.Register(nameof(ArrowLocation), typeof(DropdownArrowLocation), typeof(DropDownButton), new PropertyMetadata(DropdownArrowLocation.Right));

        public Thickness ArrowMargin
        {
            get { return (Thickness) GetValue(ArrowMarginProperty); }
            set { SetValue(ArrowMarginProperty, value); }
        }

        public static readonly DependencyProperty ArrowMarginProperty = DependencyProperty.Register(nameof(ArrowMargin), typeof(Thickness), typeof(DropDownButton), new PropertyMetadata(default(Thickness)));

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(object),
            typeof(DropDownButton), new FrameworkPropertyMetadata(string.Empty));

        public ContextMenu DropDown
        {
            get { return (ContextMenu)GetValue(DropDownProperty); }
            set { SetValue(DropDownProperty, value); }
        }

        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register(nameof(DropDown), typeof(ContextMenu),
            typeof(DropDownButton), new UIPropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand),
            typeof(DropDownButton), new UIPropertyMetadata(null));

        public bool IsArrowVisible
        {
            get { return (bool)GetValue(IsArrowVisibleProperty); }
            set { SetValue(IsArrowVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsArrowVisibleProperty = DependencyProperty.Register(nameof(IsArrowVisible), typeof(bool), typeof(DropDownButton),
            new PropertyMetadata(true));

        [ObsoleteEx(TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0", Message = "Use AccentColorBrush markup extension instead")]
        public Brush AccentColorBrush
        {
            get { return (Brush)GetValue(AccentColorBrushProperty); }
            set { SetValue(AccentColorBrushProperty, value); }
        }

        [ObsoleteEx(TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0", Message = "Use AccentColorBrush markup extension instead")]
        public static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.Register(nameof(AccentColorBrush), typeof(Brush),
            typeof(DropDownButton), new PropertyMetadata(Brushes.LightGray, (sender, e) => ((DropDownButton)sender).OnAccentColorBrushChanged()));

        public bool ShowDefaultButton
        {
            get { return (bool)GetValue(ShowDefaultButtonProperty); }
            set { SetValue(ShowDefaultButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowDefaultButtonProperty = DependencyProperty.Register(nameof(ShowDefaultButton), typeof(bool),
            typeof(DropDownButton), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool EnableTransparentBackground
        {
            get { return (bool)GetValue(EnableTransparentBackgroundProperty); }
            set { SetValue(EnableTransparentBackgroundProperty, value); }
        }

        public static readonly DependencyProperty EnableTransparentBackgroundProperty = DependencyProperty.Register(nameof(EnableTransparentBackground), typeof(bool),
            typeof(DropDownButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
            
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(DropDownButton), new PropertyMetadata(default(object)));
        #endregion

        #region Events
        public event EventHandler<EventArgs> ContentLayoutUpdated;
        #endregion

        #region Methods
        private void OnAccentColorBrushChanged()
        {
            if (AccentColorBrush is SolidColorBrush brush)
            {
                var accentColor = brush.Color;
                accentColor.CreateAccentColorResourceDictionary(nameof(DropDownButton));
            }
        }

        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            ContentLayoutUpdated?.Invoke(this, e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetCurrentValue(AccentColorBrushProperty, TryFindResource("AccentColorBrush") as SolidColorBrush);
        }
        #endregion
    }
}
