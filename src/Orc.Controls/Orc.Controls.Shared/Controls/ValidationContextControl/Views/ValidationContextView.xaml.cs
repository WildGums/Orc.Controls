// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextControl.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Media;
    using Catel.Data;
    using Catel.MVVM.Views;

    public sealed partial class ValidationContextView
    {
        static ValidationContextView()
        {
            typeof(ValidationContextView).AutoDetectViewPropertiesToSubscribe();
        }

        public ValidationContextView()
        {
            InitializeComponent();
        }

        #region Dependency properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public IValidationContext ValidationContext
        {
            get { return (IValidationContext) GetValue(ValidationContextProperty); }
            set { SetValue(ValidationContextProperty, value); }
        }

        public static readonly DependencyProperty ValidationContextProperty = DependencyProperty.Register(
            "ValidationContext", typeof (IValidationContext), typeof (ValidationContextView), new PropertyMetadata(null));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowFilterBox
        {
            get { return (bool) GetValue(ShowFilterBoxProperty); }
            set { SetValue(ShowFilterBoxProperty, value); }
        }

        public static readonly DependencyProperty ShowFilterBoxProperty = DependencyProperty.Register(
            "ShowFilterBox", typeof (bool), typeof (ValidationContextView), new PropertyMetadata(true));


        public Brush AccentColorBrush
        {
            get { return (Brush)GetValue(AccentColorBrushProperty); }
            set { SetValue(AccentColorBrushProperty, value); }
        }

        public static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.Register(
            "AccentColorBrush", typeof(Brush), typeof(ValidationContextView), new PropertyMetadata(Brushes.LightGray,
                (sender, e) => ((ValidationContextView)sender).OnAccentColorBrushChanged()));


        public bool ShowButtons
        {
            get { return (bool)GetValue(ShowButtonsProperty); }
            set { SetValue(ShowButtonsProperty, value); }
        }

        public static readonly DependencyProperty ShowButtonsProperty = DependencyProperty.Register("ShowButtons", 
            typeof(bool), typeof(ValidationContextView), new PropertyMetadata(true));
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AccentColorBrush = TryFindResource("AccentColorBrush") as SolidColorBrush;
        }

        private void OnAccentColorBrushChanged()
        {
            var solidColorBrush = AccentColorBrush as SolidColorBrush;
            if (solidColorBrush != null)
            {
                var accentColor = ((SolidColorBrush)AccentColorBrush).Color;
                accentColor.CreateAccentColorResourceDictionary("ValidationContextControl");
            }
        }
    }
}