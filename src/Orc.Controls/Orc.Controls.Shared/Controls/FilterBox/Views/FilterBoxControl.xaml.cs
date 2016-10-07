// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBoxControl.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Media;
    using Catel.MVVM.Views;

    /// <summary>
    /// Interaction logic for FilterBox.xaml
    /// </summary>
    public partial class FilterBoxControl
    {
        #region Constructors
        static FilterBoxControl()
        {
            typeof (FilterBoxControl).AutoDetectViewPropertiesToSubscribe();
        }

        public FilterBoxControl()
        {
            InitializeComponent();

            Focusable = true;
        }
        #endregion

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public IEnumerable FilterSource
        {
            get { return (IEnumerable)GetValue(FilterSourceProperty); }
            set { SetValue(FilterSourceProperty, value); }
        }

        public static readonly DependencyProperty FilterSourceProperty = DependencyProperty.Register("FilterSource", typeof(IEnumerable), 
            typeof(FilterBoxControl), new FrameworkPropertyMetadata(null));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), 
            typeof(FilterBoxControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(FilterBoxControl),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public Brush AccentColorBrush
        {
            get { return (Brush)GetValue(AccentColorBrushProperty); }
            set { SetValue(AccentColorBrushProperty, value); }
        }

        public static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.Register("AccentColorBrush", typeof(Brush),
            typeof(FilterBoxControl), new FrameworkPropertyMetadata(Brushes.LightGray, (sender, e) => ((FilterBoxControl)sender).OnAccentColorBrushChanged()));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string),
            typeof(FilterBoxControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        #region Methods
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            filterTextBox.Focus();
        }

        private void OnAccentColorBrushChanged()
        {
            var solidColorBrush = AccentColorBrush as SolidColorBrush;
            if (solidColorBrush != null)
            {
                var accentColor = ((SolidColorBrush)AccentColorBrush).Color;
                accentColor.CreateAccentColorResourceDictionary("FilterBox");
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AccentColorBrush = TryFindResource("AccentColorBrush") as SolidColorBrush;
        }
        #endregion
    }
}