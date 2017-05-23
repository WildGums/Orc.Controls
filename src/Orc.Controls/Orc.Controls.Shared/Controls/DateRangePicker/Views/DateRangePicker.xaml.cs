// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateRangePicker.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel.MVVM.Views;
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// Interaction logic for DateRangePicker.xaml
    /// </summary>
    public partial class DateRangePicker
    {
        #region Constructors
        static DateRangePicker()
        {
            typeof(DateRangePicker).AutoDetectViewPropertiesToSubscribe();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangePicker"/> class.
        /// </summary>
        /// <remarks>This method is required for design time support.</remarks>
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

        public static readonly DependencyProperty RangesProperty = DependencyProperty.Register("Ranges", typeof(ObservableCollection<DateRange>),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

			
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public DateRange SelectedRange
        {
            get { return (DateRange)GetValue(SelectedRangeProperty); }
            set { SetValue(SelectedRangeProperty, value); }
        }

        public static readonly DependencyProperty SelectedRangeProperty = DependencyProperty.Register("SelectedRange", typeof(DateRange),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

			
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        public static readonly DependencyProperty StartDateProperty = DependencyProperty.Register("StartDate", typeof(DateTime),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

			
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        public static readonly DependencyProperty EndDateProperty = DependencyProperty.Register("EndDate", typeof(DateTime),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

			
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public TimeSpan Span
        {
            get { return (TimeSpan)GetValue(SpanProperty); }
            set { SetValue(SpanProperty, value); }
        }

        public static readonly DependencyProperty SpanProperty = DependencyProperty.Register("Span", typeof(TimeSpan),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(TimeSpan.Zero, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

			
        public bool AllowCopyPaste
        {
            get { return (bool)GetValue(AllowCopyPasteProperty); }
            set { SetValue(AllowCopyPasteProperty, value); }
        }

        public static readonly DependencyProperty AllowCopyPasteProperty = DependencyProperty.Register("AllowCopyPaste", typeof(bool),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(true));

			
        public string DateTimeFormat
        {
            get { return (string)GetValue(DateTimeFormatProperty); }
            set { SetValue(DateTimeFormatProperty, value); }
        }

        public static readonly DependencyProperty DateTimeFormatProperty = DependencyProperty.Register("DateTimeFormat", typeof(string),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern));

        public bool HideTime
        {
            get { return (bool)GetValue(HideTimeProperty); }
            set { SetValue(HideTimeProperty, value); }
        }

        public static readonly DependencyProperty HideTimeProperty = DependencyProperty.Register("HideTime", typeof(bool),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(false));

        public bool DateTimeHideSeconds
        {
            get { return (bool)GetValue(DateTimeHideSecondsProperty); }
            set { SetValue(DateTimeHideSecondsProperty, value); }
        }

        public static readonly DependencyProperty DateTimeHideSecondsProperty = DependencyProperty.Register("DateTimeHideSeconds", typeof(bool),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(false));

			
        public bool ShowOptionsButton
        {
            get { return (bool)GetValue(ShowOptionsButtonProperty); }
            set { SetValue(ShowOptionsButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowOptionsButtonProperty = DependencyProperty.Register("ShowOptionsButton", typeof(bool),
            typeof(DateRangePicker), new FrameworkPropertyMetadata(true));

			
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool),
            typeof(DateRangePicker), new PropertyMetadata(false));
        #endregion
    }
}