// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatePickerControl.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
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
    /// Interaction logic for DateRangePickerControl.xaml
    /// </summary>
    public partial class DateRangePickerControl
    {
        #region Constructors
        static DateRangePickerControl()
        {
            typeof(DateRangePickerControl).AutoDetectViewPropertiesToSubscribe();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangePickerControl"/> class.
        /// </summary>
        /// <remarks>This method is required for design time support.</remarks>
        public DateRangePickerControl()
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
            typeof(DateRangePickerControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public DateRange SelectedRange
        {
            get { return (DateRange)GetValue(SelectedRangeProperty); }
            set { SetValue(SelectedRangeProperty, value); }
        }

        public static readonly DependencyProperty SelectedRangeProperty = DependencyProperty.Register("SelectedRange", typeof(DateRange),
            typeof(DateRangePickerControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public DateTime DateStart
        {
            get { return (DateTime)GetValue(DateStartProperty); }
            set { SetValue(DateStartProperty, value); }
        }

        public static readonly DependencyProperty DateStartProperty = DependencyProperty.Register("DateStart", typeof(DateTime),
            typeof(DateRangePickerControl), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public DateTime DateEnd
        {
            get { return (DateTime)GetValue(DateEndProperty); }
            set { SetValue(DateEndProperty, value); }
        }

        public static readonly DependencyProperty DateEndProperty = DependencyProperty.Register("DateEnd", typeof(DateTime),
            typeof(DateRangePickerControl), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public TimeSpan Value
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(TimeSpan),
            typeof(DateRangePickerControl), new FrameworkPropertyMetadata(TimeSpan.Zero, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool DateTimePickerAllowCopyPaste
        {
            get { return (bool)GetValue(DateTimePickerAllowCopyPasteProperty); }
            set { SetValue(DateTimePickerAllowCopyPasteProperty, value); }
        }

        public static readonly DependencyProperty DateTimePickerAllowCopyPasteProperty = DependencyProperty.Register("DateTimePickerAllowCopyPaste", typeof(bool),
            typeof(DateRangePickerControl), new FrameworkPropertyMetadata(true));

        public string DateTimePickerFormat
        {
            get { return (string)GetValue(DateTimePickerFormatProperty); }
            set { SetValue(DateTimePickerFormatProperty, value); }
        }

        public static readonly DependencyProperty DateTimePickerFormatProperty = DependencyProperty.Register("DateTimePickerFormat", typeof(string),
            typeof(DateRangePickerControl), new FrameworkPropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern));

        public bool DateTimePickerHideSeconds
        {
            get { return (bool)GetValue(DateTimePickerHideSecondsProperty); }
            set { SetValue(DateTimePickerHideSecondsProperty, value); }
        }

        public static readonly DependencyProperty DateTimePickerHideSecondsProperty = DependencyProperty.Register("DateTimePickerHideSeconds", typeof(bool),
            typeof(DateRangePickerControl), new FrameworkPropertyMetadata(false));

        public bool DateTimePickerShowOptionsButton
        {
            get { return (bool)GetValue(DateTimePickerShowOptionsButtonProperty); }
            set { SetValue(DateTimePickerShowOptionsButtonProperty, value); }
        }

        public static readonly DependencyProperty DateTimePickerShowOptionsButtonProperty = DependencyProperty.Register("DateTimePickerShowOptionsButton", typeof(bool),
            typeof(DateRangePickerControl), new FrameworkPropertyMetadata(true));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool),
            typeof(DateRangePickerControl), new PropertyMetadata(false));
        #endregion
    }
}