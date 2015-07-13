// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropDownButton.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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
    using Catel.MVVM.Views;
    using Microsoft.WindowsAPICodePack.Shell;

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
            typeof(DropDownButton), new PropertyMetadata(Brushes.LightGray, (sender, e) => ((DropDownButton)sender).OnAccentColorBrushedChanged()));

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

        public static readonly DependencyProperty EnableTransparentBackgroundProperty = DependencyProperty.Register("EnableTransparentBackground", typeof(bool),
            typeof(DropDownButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        #region Events
        public event EventHandler<EventArgs> ContentLayoutUpdated;
        #endregion

        #region Methods
        private void OnAccentColorBrushedChanged()
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

            AccentColorBrush = TryFindResource("AccentColorBrush") as SolidColorBrush;
        }
        #endregion
    }
}