// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateRangePicker.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Windows;
    using Catel.MVVM.Views;

    public partial class DateRangePicker
    {
        #region Constructors
        static DateRangePicker()
        {
            typeof(DateRangePicker).AutoDetectViewPropertiesToSubscribe();
        }

        public DateRangePicker()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public ObservableCollection<DateRange> Ranges
        {
            get { return (ObservableCollection<DateRange>)GetValue(RangesProperty); }
            set { SetValue(RangesProperty, value); }
        }

        public static readonly DependencyProperty RangesProperty = DependencyProperty.Register(nameof(Ranges), typeof(ObservableCollection<DateRange>),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public DateRange SelectedRange
        {
            get { return (DateRange)GetValue(SelectedRangeProperty); }
            set { SetValue(SelectedRangeProperty, value); }
        }

        public static readonly DependencyProperty SelectedRangeProperty = DependencyProperty.Register(nameof(SelectedRange), typeof(DateRange),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        public static readonly DependencyProperty StartDateProperty = DependencyProperty.Register(nameof(StartDate), typeof(DateTime),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        public static readonly DependencyProperty EndDateProperty = DependencyProperty.Register(nameof(EndDate), typeof(DateTime),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public TimeSpan Span
        {
            get { return (TimeSpan)GetValue(SpanProperty); }
            set { SetValue(SpanProperty, value); }
        }

        public static readonly DependencyProperty SpanProperty = DependencyProperty.Register(nameof(Span), typeof(TimeSpan),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(TimeSpan.Zero, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public bool AllowCopyPaste
        {
            get { return (bool)GetValue(AllowCopyPasteProperty); }
            set { SetValue(AllowCopyPasteProperty, value); }
        }

        public static readonly DependencyProperty AllowCopyPasteProperty = DependencyProperty.Register(nameof(AllowCopyPaste), typeof(bool),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(true));


        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(nameof(Format), typeof(string),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern));


        public bool HideTime
        {
            get { return (bool)GetValue(HideTimeProperty); }
            set { SetValue(HideTimeProperty, value); }
        }

        public static readonly DependencyProperty HideTimeProperty = DependencyProperty.Register(nameof(HideTime), typeof(bool),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(false));


        public bool HideSeconds
        {
            get { return (bool)GetValue(HideSecondsProperty); }
            set { SetValue(HideSecondsProperty, value); }
        }

        public static readonly DependencyProperty HideSecondsProperty = DependencyProperty.Register(nameof(HideSeconds), typeof(bool),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(false));


        public bool ShowOptionsButton
        {
            get { return (bool)GetValue(ShowOptionsButtonProperty); }
            set { SetValue(ShowOptionsButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowOptionsButtonProperty = DependencyProperty.Register(nameof(ShowOptionsButton), typeof(bool),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(true));


        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool),
            typeof(DateRangePicker), new PropertyMetadata(false));
        #endregion
    }
}
