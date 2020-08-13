namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Catel.Threading;
    using Catel.Windows;
    using Catel.Windows.Input;
    using Converters;
    using Extensions;
    using ClockFace = Orc.Controls.ClockFace;

    [TemplatePart(Name = "PART_HourNumericTextBox", Type = typeof(NumericTextBox))]
    [TemplatePart(Name = "PART_MinuteNumericTextBox", Type = typeof(NumericTextBox))]
    [TemplatePart(Name = "PART_SecondNumericTextBox", Type = typeof(NumericTextBox))]

    [TemplatePart(Name = "PART_HourMinuteSeparatorTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_MinuteSecondSeparatorTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_SecondAmPmSeparatorTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_AmPmSeparatorTextBlock", Type = typeof(TextBlock))]

    [TemplatePart(Name = "PART_HourToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_MinuteToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_SecondToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_AmPmToggleButton", Type = typeof(ToggleButton))]

    [TemplatePart(Name = "PART_ClockFaceIconDropDownButton", Type = typeof(DropDownButton))]

    [TemplatePart(Name = "PART_NowMenuItem", Type = typeof(MenuItem))]
    [TemplatePart(Name = "PART_SelectTimeMenuItem", Type = typeof(MenuItem))]
    [TemplatePart(Name = "PART_ClearMenuItem", Type = typeof(MenuItem))]
    [TemplatePart(Name = "PART_CopyMenuItem", Type = typeof(MenuItem))]
    [TemplatePart(Name = "PART_PasteMenuItem", Type = typeof(MenuItem))]

    [TemplatePart(Name = "PART_MainGrid", Type = typeof(Grid))]

    [TemplatePart(Name = "PART_AmPmListTextBox", Type = typeof(ListTextBox))]

    [TemplatePart(Name = "PART_ClockFacePopup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_ClockFace", Type = typeof(Orc.Controls.ClockFace))]

    public class TimePicker : Control, IEditableControl
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private bool _isTemplateApplied;

        private TimeSpan _nowValue;

        private List<TextBox> _textBoxes;
        private DateTimeFormatInfo _formatInfo;

        private readonly int _defaultSecondFormatPosition = 5;
        private readonly int _defaultAmPmFormatPosition = 6;

        private NumericTextBox _hourNumericTextBox;
        private NumericTextBox _minuteNumericTextBox;
        private NumericTextBox _secondNumericTextBox;

        private TextBlock _hourMinuteSeparatorTextBlock;
        private TextBlock _minuteSecondSeparatorTextBlock;
        private TextBlock _secondAmPmSeparatorTextBlock;
        private TextBlock _amPmSeparatorTextBlock;

        private ToggleButton _hourToggleButton;
        private ToggleButton _minuteToggleButton;
        private ToggleButton _secondToggleButton;
        private ToggleButton _amPmToggleButton;

        private DropDownButton _clockFaceIconDropDownButton;

        private MenuItem _nowMenuItem;
        private MenuItem _selectTimeMenuItem;
        private MenuItem _clearMenuItem;
        private MenuItem _copyMenuItem;
        private MenuItem _pasteMenuItem;

        private Grid _mainGrid;

        private ListTextBox _amPmListTextBox;

        private Popup _clockFacePopup;
        private Controls.ClockFace _clockFace;

        #region Dependency properties

        public CultureInfo Culture
        {
            get { return (CultureInfo)GetValue(CultureProperty); }
            set { SetValue(CultureProperty, value); }
        }

        public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(
            nameof(Culture), typeof(CultureInfo), typeof(TimePicker), new PropertyMetadata(default(CultureInfo),
                (sender, args) => ((TimePicker)sender).OnCultureChanged(args)));

        public TimeSpan? Value
        {
            get { return (TimeSpan?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(TimeSpan?),
            typeof(TimePicker), new FrameworkPropertyMetadata(TimeSpan.Zero, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => ((TimePicker)sender).OnValueChanged((TimeSpan?)e.OldValue, (TimeSpan?)e.NewValue)));

        public bool ShowOptionsButton
        {
            get { return (bool)GetValue(ShowOptionsButtonProperty); }
            set { SetValue(ShowOptionsButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowOptionsButtonProperty = DependencyProperty.Register(nameof(ShowOptionsButton), typeof(bool),
            typeof(TimePicker), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, args) => ((TimePicker)sender).OnShowOptionsButtonChanged()));

        public bool AllowNull
        {
            get { return (bool)GetValue(AllowNullProperty); }
            set { SetValue(AllowNullProperty, value); }
        }

        public static readonly DependencyProperty AllowNullProperty = DependencyProperty.Register(nameof(AllowNull), typeof(bool),
            typeof(TimePicker), new PropertyMetadata(false));

        public bool AllowCopyPaste
        {
            get { return (bool)GetValue(AllowCopyPasteProperty); }
            set { SetValue(AllowCopyPasteProperty, value); }
        }

        public static readonly DependencyProperty AllowCopyPasteProperty = DependencyProperty.Register(nameof(AllowCopyPaste), typeof(bool),
            typeof(TimePicker), new PropertyMetadata(true));

       public bool HideSeconds
        {
            get { return (bool)GetValue(HideSecondsProperty); }
            set { SetValue(HideSecondsProperty, value); }
        }

        public static readonly DependencyProperty HideSecondsProperty = DependencyProperty.Register(nameof(HideSeconds), typeof(bool),
            typeof(TimePicker), new FrameworkPropertyMetadata(false, (sender, e) => ((TimePicker)sender).OnHideSecondsChanged()));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool),
            typeof(TimePicker), new PropertyMetadata(false));

        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(nameof(Format), typeof(string),
            typeof(TimePicker), new FrameworkPropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern, (sender, e) => ((TimePicker)sender).OnFormatChanged()));

        public bool IsHour12Format
        {
            get { return (bool)GetValue(IsHour12FormatProperty); }
            private set { SetValue(IsHour12FormatPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsHour12FormatPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsHour12Format), typeof(bool),
            typeof(TimePicker), new PropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.Contains("h")));

        public static readonly DependencyProperty IsHour12FormatProperty = IsHour12FormatPropertyKey.DependencyProperty;

        public bool IsAmPmShortFormat
        {
            get { return (bool)GetValue(IsAmPmShortFormatProperty); }
            private set { SetValue(IsAmPmShortFormatPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey IsAmPmShortFormatPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsAmPmShortFormat), typeof(bool),
            typeof(TimePicker), new PropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.Count(x => x == 't') < 2));

        public static readonly DependencyProperty IsAmPmShortFormatProperty = IsAmPmShortFormatPropertyKey.DependencyProperty;
        #endregion

        #region Properties
        public bool IsInEditMode { get; private set; }

        private int? Hour
        {
            get => (int?)_hourNumericTextBox.Value;
#pragma warning disable WPF0035 // Use SetValue in setter.
            set => _hourNumericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, (double?)value);
#pragma warning restore WPF0035 // Use SetValue in setter.
        }

        private int? Minute
        {
            get => (int?)_minuteNumericTextBox.Value;
#pragma warning disable WPF0035 // Use SetValue in setter.
            set => _minuteNumericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, (double?)value);
#pragma warning restore WPF0035 // Use SetValue in setter.
        }

        private int? Second
        {
            get => (int?)_secondNumericTextBox.Value;
#pragma warning disable WPF0035 // Use SetValue in setter.
            set => _secondNumericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, (double?)value);
#pragma warning restore WPF0035 // Use SetValue in setter.
        }

        private string AmPm
        {
            get => _amPmListTextBox.Value;
#pragma warning disable WPF0035 // Use SetValue in setter.
            set => _amPmListTextBox.SetCurrentValue(ListTextBox.ValueProperty, value);
#pragma warning restore WPF0035 // Use SetValue in setter.
        }
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            /*Hour numeric text box*/
            _hourNumericTextBox = GetTemplateChild("PART_HourNumericTextBox") as NumericTextBox;
            if (_hourNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_HourNumericTextBox'");
            }
            _hourNumericTextBox.ValueChanged += OnHourValueChanged;

            /*Hour numeric text box*/
            _minuteNumericTextBox = GetTemplateChild("PART_MinuteNumericTextBox") as NumericTextBox;
            if (_minuteNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_MinuteNumericTextBox'");
            }
            _minuteNumericTextBox.ValueChanged += OnMinuteValueChanged;

            _secondNumericTextBox = GetTemplateChild("PART_SecondNumericTextBox") as NumericTextBox;
            if (_secondNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_SecondNumericTextBox'");
            }
            _secondNumericTextBox.ValueChanged += OnSecondValueChanged;

            _amPmListTextBox = GetTemplateChild("PART_AmPmListTextBox") as ListTextBox;
            if (_amPmListTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_AmPmListTextBox'");
            }
            _amPmListTextBox.ValueChanged += OnAmPmListTextBoxValueChanged;

            /*Separators*/
           _hourMinuteSeparatorTextBlock = GetTemplateChild("PART_HourMinuteSeparatorTextBlock") as TextBlock;
            if (_hourMinuteSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_HourMinuteSeparatorTextBlock'");
            }

            _minuteSecondSeparatorTextBlock = GetTemplateChild("PART_MinuteSecondSeparatorTextBlock") as TextBlock;
            if (_minuteSecondSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_MinuteSecondSeparatorTextBlock'");
            }

            _secondAmPmSeparatorTextBlock = GetTemplateChild("PART_SecondAmPmSeparatorTextBlock") as TextBlock;
            if (_secondAmPmSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_SecondAmPmSeparatorTextBlock'");
            }

            _amPmSeparatorTextBlock = GetTemplateChild("PART_AmPmSeparatorTextBlock") as TextBlock;
            if (_amPmSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_AmPmSeparatorTextBlock'");
            }

            /*Toggles*/
            _hourToggleButton = GetTemplateChild("PART_HourToggleButton") as ToggleButton;
            if (_hourToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_HourToggleButton'");
            }
            _hourToggleButton.Checked += OnToggleButtonChecked;

            _minuteToggleButton = GetTemplateChild("PART_MinuteToggleButton") as ToggleButton;
            if (_minuteToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_MinuteToggleButton'");
            }
            _minuteToggleButton.Checked += OnToggleButtonChecked;

            _secondToggleButton = GetTemplateChild("PART_SecondToggleButton") as ToggleButton;
            if (_secondToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_SecondToggleButton'");
            }
            _secondToggleButton.Checked += OnToggleButtonChecked;

            _amPmToggleButton = GetTemplateChild("PART_AmPmToggleButton") as ToggleButton;
            if (_amPmToggleButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_AmPmToggleButton'");
            }
            _amPmToggleButton.Checked += OnToggleButtonChecked;

            _clockFaceIconDropDownButton = GetTemplateChild("PART_ClockFaceIconDropDownButton") as DropDownButton;
            if (_clockFaceIconDropDownButton is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ClockFaceIconDropDownButton'");
            }
            _clockFaceIconDropDownButton.SetCurrentValue(VisibilityProperty, ShowOptionsButton ? Visibility.Visible : Visibility.Hidden);

            /*Menu items*/
            _nowMenuItem = GetTemplateChild("PART_NowMenuItem") as MenuItem;
            if (_nowMenuItem is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_NowMenuItem'");
            }
            _nowMenuItem.Click += OnNowMenuItemClick;

            _selectTimeMenuItem = GetTemplateChild("PART_SelectTimeMenuItem") as MenuItem;
            if (_selectTimeMenuItem is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_SelectTimeMenuItem'");
            }
            _selectTimeMenuItem.Click += OnSelectTimeMenuItemClick;

            _clearMenuItem = GetTemplateChild("PART_ClearMenuItem") as MenuItem;
            if (_clearMenuItem is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ClearMenuItem'");
            }
            _clearMenuItem.Click += OnClearMenuItemClick;

            _copyMenuItem = GetTemplateChild("PART_CopyMenuItem") as MenuItem;
            if (_copyMenuItem is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_CopyMenuItem'");
            }
            _copyMenuItem.Click += OnCopyMenuItemClick;

            _pasteMenuItem = GetTemplateChild("PART_PasteMenuItem") as MenuItem;
            if (_pasteMenuItem is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_PasteMenuItem'");
            }
            _pasteMenuItem.Click += OnPasteMenuItemClick;

            /*Main grid*/
            _mainGrid = GetTemplateChild("PART_MainGrid") as Grid;
            if (_mainGrid is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_MainGrid'");
            }
            _mainGrid.MouseEnter += OnMouseEnter;
            _mainGrid.MouseLeave += OnMouseLeave;
            _mainGrid.IsKeyboardFocusWithinChanged += OnIsKeyboardFocusWithinChanged;

            /*Pop up*/
            _clockFacePopup = GetTemplateChild("PART_ClockFacePopup") as Popup;
            if (_clockFacePopup is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ClockFacePopup'");
            }
            //_clockFacePopup.Closed += OnClockFacePopupClosed;

#pragma warning disable WPF0131 // Use correct [TemplatePart] type.
            _clockFace = GetTemplateChild("PART_ClockFace") as Orc.Controls.ClockFace;
#pragma warning restore WPF0131 // Use correct [TemplatePart] type.
            if (_clockFace is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_ClockFace'");
            }


            _textBoxes = new List<TextBox>
            {
                _hourNumericTextBox,
                _minuteNumericTextBox,
                _secondNumericTextBox,
                _amPmListTextBox
            };

            _nowValue = new TimeSpan(DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes, DateTime.Now.TimeOfDay.Seconds);
            
            _isTemplateApplied = true;

            ApplyFormat();

            UpdateUi();
        }

        private void OnClockFacePopupClosed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnIsKeyboardFocusedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusedChanged(e);

            var textBox = _textBoxes[0];

            textBox.SetCurrentValue(FocusableProperty, true);
            Keyboard.Focus(textBox);
        }

        private void OnNowMenuItemClick(object sender, RoutedEventArgs e)
        {
            UpdateTime(_nowValue);

            RaiseStopEdit();
        }

        private void OnSelectTimeMenuItemClick(object sender, RoutedEventArgs e)
        {
            UnsubscribeFromClockFaceEvents();

            _clockFacePopup.SetCurrentValue(Popup.IsOpenProperty, true);

            var time = Value ?? _nowValue;
            _clockFace.SetCurrentValue(Controls.ClockFace.TimeValueProperty, time);
            //_clockFace.SetCurrentValue(Controls.ClockFace.SelectedTimeProperty, Value);

            _clockFace.Focus();
            Keyboard.Focus(_clockFace);

            SubscribeToClockFaceEvents();
        }

        private void SubscribeToClockFaceEvents()
        {
            _clockFace.PreviewKeyDown += ClockFaceOnPreviewKeyDown;
            _clockFace.MouseLeftButtonUp += _clockFace_MouseLeftButtonUp; ;
        }

        private void _clockFace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Controls.ClockFace clockFace))
            {
                return;
            }

            if (_clockFaceSelectionChangedByKey)
            {
                _clockFaceSelectionChangedByKey = false;

                return;
            }

            if (AllowNull || clockFace.TimeValue != null)
            {
                UpdateTime(clockFace.TimeValue);
            }

            ((Popup)clockFace.Parent).SetCurrentValue(Popup.IsOpenProperty, _clockFaceSelectionChangedByKey);

            _clockFaceSelectionChangedByKey = false;
        }

        private void UnsubscribeFromClockFaceEvents()
        {
            _clockFace.PreviewKeyDown -= ClockFaceOnPreviewKeyDown;
            _clockFace.MouseLeftButtonUp -= _clockFace_MouseLeftButtonUp;
        }

        private void OnClearMenuItemClick(object sender, RoutedEventArgs e)
        {
            UpdateTime(null);

            RaiseStopEdit();
        }

        private void OnCopyMenuItemClick(object sender, RoutedEventArgs e)
        {
            var value = Value;
            if (value != null)
            {
                Clipboard.SetText(value.Value.ToString(), TextDataFormat.Text);
            }
        }

        private void OnPasteMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (!Clipboard.ContainsData(DataFormats.Text))
            {
                return;
            }

            var text = Clipboard.GetText(TextDataFormat.Text);
            if (!string.IsNullOrEmpty(text) && TimeSpan.TryParseExact(text, Format, null, out var value) ||
                TimeSpan.TryParseExact(text, CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern, null, out value) || 
                TimeSpan.TryParseExact(text, CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern, null, out value))
            {
                SetCurrentValue(ValueProperty, new TimeSpan(value.Hours, value.Minutes, value.Seconds));
            }
        }

        private void OnFormatChanged()
        {
            ApplyFormat();

            UpdateUi();
        }


        private void OnCultureChanged(DependencyPropertyChangedEventArgs args)
        {
            ApplyFormat();

            UpdateUi();
        }

        private void ApplyFormat()
        {
            if (!_isTemplateApplied)
            {
                return;
            }

            var format = Format;
            _formatInfo = DateTimeFormatHelper.GetDateTimeFormatInfo(format, false);
            var hasLongTimeFormat = !(_formatInfo.HourFormat is null
                                    || _formatInfo.MinuteFormat is null
                                    || _formatInfo.SecondFormat is null);
                
            var culture = GetCulture();

            //_clockFace.SetCurrentValue(TimeProperty, );

            if (!hasLongTimeFormat)
            {
                var timePattern = DateTimeFormatHelper.ExtractTimePatternFromFormat(format);
                if (!string.IsNullOrEmpty(timePattern))
                {
                    timePattern = DateTimeFormatHelper.FindMatchedLongTimePattern(culture, timePattern);
                }

                if (string.IsNullOrEmpty(timePattern))
                {
                    timePattern = culture.DateTimeFormat.LongTimePattern;
                }

                _formatInfo = DateTimeFormatHelper.GetDateTimeFormatInfo(timePattern, false);
            }

            UpdateUiPartVisibility();

            var hourPosition = _formatInfo?.HourPosition;
            if (hourPosition is null)
            {
                return;
            }

            var minutePosition = _formatInfo.MinutePosition;
            if (minutePosition is null)
            {
                return;
            }

            var isHour12Format = _formatInfo.IsHour12Format ?? true;
            IsHour12Format = isHour12Format;
            _hourNumericTextBox.SetCurrentValue(NumericTextBox.MinValueProperty, (double)(isHour12Format ? 1 : 0));
            _hourNumericTextBox.SetCurrentValue(NumericTextBox.MaxValueProperty, (double)(isHour12Format ? 12 : 23));
            _hourToggleButton.SetCurrentValue(TagProperty, isHour12Format ? DateTimePart.Hour12 : DateTimePart.Hour);

            IsAmPmShortFormat = _formatInfo.IsAmPmShortFormat ?? true;

            EnableOrDisableHourConverterDependingOnFormat();
            EnableOrDisableAmPmConverterDependingOnFormat();

            _amPmListTextBox.SetCurrentValue(ListTextBox.ListOfValuesProperty, new List<string>
        {
            Meridiems.GetAmForFormat(_formatInfo),
            Meridiems.GetPmForFormat(_formatInfo)
        });

            _hourNumericTextBox.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.HourFormat.Length));
            _minuteNumericTextBox.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.MinuteFormat.Length));
            _secondNumericTextBox.SetCurrentValue(NumericTextBox.FormatProperty, NumberFormatHelper.GetFormat(_formatInfo.SecondFormat?.Length ?? 0));

            UnsubscribeNumericTextBoxes();

            Grid.SetColumn(_hourNumericTextBox, GetPosition(hourPosition.Value));
            Grid.SetColumn(_minuteNumericTextBox, GetPosition(minutePosition.Value));
            Grid.SetColumn(_secondNumericTextBox, GetPosition(_formatInfo.SecondPosition ?? _defaultSecondFormatPosition));
            Grid.SetColumn(_amPmListTextBox, GetPosition(_formatInfo.AmPmPosition ?? _defaultAmPmFormatPosition));

            Grid.SetColumn(_hourToggleButton, GetPosition(hourPosition.Value) + 1);
            Grid.SetColumn(_minuteToggleButton, GetPosition(minutePosition.Value) + 1);
            Grid.SetColumn(_secondToggleButton, GetPosition(_formatInfo.SecondPosition ?? _defaultSecondFormatPosition) + 1);

            Grid.SetColumn(_amPmToggleButton, GetPosition((_formatInfo.AmPmPosition ?? _defaultAmPmFormatPosition) + 1));

            // Fix positions which could be broken, because of AM/PM textBlock.
            // Fix for seconds in a same way
            int hourPos = hourPosition.Value, minutePos = minutePosition.Value, secondPos = _formatInfo.SecondPosition ?? 5,
                amPmPos = _formatInfo.AmPmPosition ?? 6;
            FixNumericTextBoxesPositions(ref hourPos, ref minutePos, ref secondPos, ref amPmPos);

            _textBoxes[hourPos] = _hourNumericTextBox;
            _textBoxes[minutePos] = _minuteNumericTextBox;
            _textBoxes[secondPos] = _secondNumericTextBox;
            _textBoxes[amPmPos] = _amPmListTextBox;

            // Fix tab order inside control.
            _hourNumericTextBox.SetCurrentValue(TabIndexProperty, hourPos);
            _minuteNumericTextBox.SetCurrentValue(TabIndexProperty, minutePos);
            _secondNumericTextBox.SetCurrentValue(TabIndexProperty, secondPos);
            _amPmListTextBox.SetCurrentValue(TabIndexProperty, amPmPos);
            _clockFaceIconDropDownButton.SetCurrentValue(TabIndexProperty, amPmPos + 1);

            SubscribeNumericTextBoxes();

            _hourMinuteSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator4);
            _minuteSecondSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator5);
            _secondAmPmSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator6);
            _amPmSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, Value == null ? string.Empty : _formatInfo.Separator7);
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            UpdateUiPartVisibility();
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            UpdateUiPartVisibility();
        }

        private void OnIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateUiPartVisibility();
        }

        private void UpdateUiPartVisibility()
        {
            if (_formatInfo is null)
            {
                return;
            }

            //hide parts according to Hide options and format
            var isHiglighted = _mainGrid.IsMouseOver || _mainGrid.IsKeyboardFocusWithin;

            /**** time parts ****/
            var isAmPmVisible = _formatInfo.AmPmFormat != null;
            var isSecondsVisible = _formatInfo.SecondFormat != null && !HideSeconds;
            var isMinutesVisible = _formatInfo.MinuteFormat != null;
            var isHoursVisible = _formatInfo.HourFormat != null;

            var secondsHiddenVisibility = isSecondsVisible ? Visibility.Hidden : Visibility.Collapsed;
            var minutesHiddenVisibility = isMinutesVisible ? Visibility.Hidden : Visibility.Collapsed;
            var hoursHiddenVisibility = isHoursVisible ? Visibility.Hidden : Visibility.Collapsed;
            var ampmHiddenVisibility = isAmPmVisible ? Visibility.Hidden : Visibility.Collapsed;

            /* second parts */
            _secondNumericTextBox.SetCurrentValue(VisibilityProperty, isSecondsVisible.ToVisibility());
            _minuteSecondSeparatorTextBlock.SetCurrentValue(VisibilityProperty, (isSecondsVisible && !isHiglighted).ToVisibility(secondsHiddenVisibility));
            _secondToggleButton.SetCurrentValue(VisibilityProperty, (isSecondsVisible && isHiglighted).ToVisibility(secondsHiddenVisibility));

            /* minute parts */
            _minuteNumericTextBox.SetCurrentValue(VisibilityProperty, isMinutesVisible.ToVisibility());
            _hourMinuteSeparatorTextBlock.SetCurrentValue(VisibilityProperty, (isMinutesVisible && !isHiglighted).ToVisibility(minutesHiddenVisibility));
            _minuteToggleButton.SetCurrentValue(VisibilityProperty, (isMinutesVisible && isHiglighted).ToVisibility(minutesHiddenVisibility));

            /* hour parts */
            _hourNumericTextBox.SetCurrentValue(VisibilityProperty, isHoursVisible.ToVisibility());
            _hourToggleButton.SetCurrentValue(VisibilityProperty, (isHoursVisible && isHiglighted).ToVisibility(hoursHiddenVisibility));

            /* am pm parts */
            _amPmListTextBox.SetCurrentValue(VisibilityProperty, isAmPmVisible.ToVisibility());
            _secondAmPmSeparatorTextBlock.SetCurrentValue(VisibilityProperty, (isAmPmVisible && !isHiglighted).ToVisibility(ampmHiddenVisibility));
            _amPmSeparatorTextBlock.SetCurrentValue(VisibilityProperty, (isAmPmVisible && !isHiglighted).ToVisibility(ampmHiddenVisibility));
            _amPmToggleButton.SetCurrentValue(VisibilityProperty, (isAmPmVisible && isHiglighted).ToVisibility(ampmHiddenVisibility));
        }

        private void OnToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;
            var activeTimePart = (DateTimePart)toggleButton.Tag;

            TextBox activeTextBox;
            switch (activeTimePart)
            {
                case DateTimePart.Hour:
                    activeTextBox = _hourNumericTextBox;
                    break;

                case DateTimePart.Hour12:
                    activeTextBox = _hourNumericTextBox;
                    break;

                case DateTimePart.Minute:
                    activeTextBox = _minuteNumericTextBox;
                    break;

                case DateTimePart.Second:
                    activeTextBox = _secondNumericTextBox;
                    break;

                case DateTimePart.AmPmDesignator:
                    activeTextBox = _amPmListTextBox;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            // TODO: Переделать
            var dateTime = Value ?? _nowValue;
            var dateTimePartHelper = new DateTimePartHelper(new DateTime(0001,01,01, dateTime.Hours, dateTime.Minutes, dateTime.Seconds), activeTimePart, _formatInfo, activeTextBox, toggleButton);
            dateTimePartHelper.CreatePopup();
        }

        private void OnValueChanged(TimeSpan? oldValue, TimeSpan? newValue)
        {
            var ov = oldValue;
            var nv = newValue;

            if (!AllowNull && newValue == null)
            {
                nv = _nowValue;
            }

            if (ov == null && nv != null || ov != null && nv == null)
            {
                ApplyFormat();
            }

            if (newValue == null && nv != null)
            {
                var dispatcherService = this.GetServiceLocator().ResolveType<IDispatcherService>();
                dispatcherService.Invoke(() => SetCurrentValue(ValueProperty, nv));
            }

            UpdateUi();
        }

        private void UpdateTime(TimeSpan? time)
        {
            if (!AllowNull && !time.HasValue)
            {
                time = _nowValue;
            }

            SetCurrentValue(ValueProperty, time);
        }
        
        private void UpdateUi()
        {
            if (!_isTemplateApplied)
            {
                return;
            }

            var value = Value;

            var hour = value?.Hours;
            if (hour != null && IsHour12Format && hour > 12)
            {
                hour -= 12;
            }

            Hour = hour;
            Minute = value?.Minutes;
            Second = value?.Seconds;
        }

        private void OnHourValueChanged(object sender, EventArgs e)
        {
            var value = Value;
            var hour = Hour;
            if (hour is null)
            {
                return;
            }

            if (value?.Hours == hour)
            {
                return;
            }

            var currentValue = value ?? _nowValue;
            SetCurrentValue(ValueProperty, new TimeSpan(hour.Value, currentValue.Minutes, currentValue.Seconds));
        }

        private void OnMinuteValueChanged(object sender, EventArgs e)
        {
            var value = Value;
            var minute = Minute;
            if (minute is null)
            {
                return;
            }

            if (value?.Minutes == minute)
            {
                return;
            }

            var currentValue = value ?? _nowValue;
            SetCurrentValue(ValueProperty, new TimeSpan(currentValue.Hours, minute.Value, currentValue.Seconds));
        }

        private void OnSecondValueChanged(object sender, EventArgs e)
        {
            var value = Value;
            var second = Second;
            if (second is null)
            {
                return;
            }

            if (value?.Seconds == second)
            {
                return;
            }

            var currentValue = value ?? _nowValue;
            SetCurrentValue(ValueProperty, new TimeSpan(currentValue.Hours, currentValue.Minutes, second.Value));
        }

        private void OnAmPmListTextBoxValueChanged(object sender, EventArgs e)
        {
            var amPm = AmPm;
            var value = Value;

            if (!Meridiems.LongAM.Equals(amPm) && !Meridiems.LongPM.Equals(amPm))
            {
                return;
            }

            if (value == null)
            {
                SetCurrentValue(ValueProperty, _nowValue);
            }
            else
            {
                var currentValue = value.Value;
                if (currentValue.Hours >= 12 && Meridiems.LongPM.Equals(amPm)
                    || currentValue.Hours < 12 && Meridiems.LongAM.Equals(amPm))
                {
                    return;
                }

                var newValue = new TimeSpan(currentValue.Hours, currentValue.Minutes, currentValue.Seconds);
                newValue = Meridiems.LongAM.Equals(amPm) ? new TimeSpan(newValue.Hours - 12, newValue.Minutes, newValue.Seconds) : new TimeSpan(newValue.Hours + 12, newValue.Minutes, newValue.Seconds);
                
                SetCurrentValue(ValueProperty, newValue);
            }
        }
        
        private void OnShowOptionsButtonChanged()
        {
            _clockFaceIconDropDownButton.SetCurrentValue(VisibilityProperty, ShowOptionsButton ? Visibility.Visible : Visibility.Collapsed);
        }

        private void OnHideSecondsChanged()
        {
            ApplyFormat();
        }

        private async void OnTextBoxLeftBoundReached(object sender, EventArgs e)
        {
            var currentTextBoxIndex = _textBoxes.IndexOf((TextBox)sender);
            var prevTextBox = _textBoxes[currentTextBoxIndex - 1];

            prevTextBox.CaretIndex = prevTextBox.Text.Length;

            await TaskShim.Delay(1); // Note: this is a hack. without this delay it is not possible to set focus

            prevTextBox.Focus();
        }

        private async void OnTextBoxRightBoundReached(object sender, EventArgs eventArgs)
        {
            var currentTextBoxIndex = _textBoxes.IndexOf((TextBox)sender);
            var nextTextBox = _textBoxes[currentTextBoxIndex + 1];

            nextTextBox.CaretIndex = 0;

            await TaskShim.Delay(1); // Note: this is a hack. without this delay it is not possible to set focus

            nextTextBox.Focus();
        }

        private void SubscribeNumericTextBoxes()
        {
            // Enable support for switching between textboxes,
            // 0-5 because we can't switch to right on last textBox.
            for (var i = 0; i <= 2; i++)
            {
                _textBoxes[i].SubscribeToOnRightBoundReachedEvent(OnTextBoxRightBoundReached);
            }

            // Enable support for switching between textboxes,
            // 5-1 because we can't switch to left on first textBox.
            for (var i = 3; i >= 1; i--)
            {
                _textBoxes[i].SubscribeToOnLeftBoundReachedEvent(OnTextBoxLeftBoundReached);
            }
        }

        private void UnsubscribeNumericTextBoxes()
        {
            // Disable support for switching between textboxes,
            // 0-4 because we can't switch to right on last textBox.
            for (var i = 0; i <= 2; i++)
            {
                _textBoxes[i].UnsubscribeFromOnRightBoundReachedEvent(OnTextBoxRightBoundReached);
            }

            // Disable support for switching between textboxes,
            // 5-1 because we can't switch to left on first textBox.
            for (var i = 3; i >= 1; i--)
            {
                _textBoxes[i].UnsubscribeFromOnLeftBoundReachedEvent(OnTextBoxLeftBoundReached);
            }
        }

        private void EnableOrDisableHourConverterDependingOnFormat()
        {
            if (!(TryFindResource(nameof(Hour24ToHour12Converter)) is Hour24ToHour12Converter converter))
            {
                return;
            }

            converter.IsEnabled = IsHour12Format;
            BindingOperations.GetBindingExpression(_hourNumericTextBox, NumericTextBox.ValueProperty)?.UpdateTarget();
        }

        private void EnableOrDisableAmPmConverterDependingOnFormat()
        {
            if (!(TryFindResource(nameof(AmPmLongToAmPmShortConverter)) is AmPmLongToAmPmShortConverter converter))
            {
                return;
            }

            converter.IsEnabled = IsAmPmShortFormat;
            BindingOperations.GetBindingExpression(_amPmListTextBox, ListTextBox.ValueProperty)?.UpdateTarget();
        }

       private static void FixNumericTextBoxesPositions(ref int hourPosition, ref int minutePosition, ref int secondPosition, ref int amPmPosition)
        {
            var dict = new Dictionary<string, int>
            {
                { "hourPosition", hourPosition },
                { "minutePosition", minutePosition },
                { "secondPosition", secondPosition },
                { "amPmPosition", amPmPosition }
            };

            var current = 0;
            foreach (var entry in dict.OrderBy(x => x.Value))
            {
                dict[entry.Key] = current++;
            }

            hourPosition = dict["hourPosition"];
            minutePosition = dict["minutePosition"];
            secondPosition = dict["secondPosition"];
            amPmPosition = dict["amPmPosition"];
        }

        private static int GetPosition(int index)
        {
            return index * 2;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.OemSemicolon)
            {
                if (KeyboardHelper.AreKeyboardModifiersPressed(ModifierKeys.Control))
                {
                    UpdateTime(_nowValue);
                    e.Handled = true;
                }

                if (KeyboardHelper.AreKeyboardModifiersPressed(ModifierKeys.Control | ModifierKeys.Shift))
                {
                    UpdateTime(_nowValue);
                    e.Handled = true;
                }
            }
        }

        private void ClockFaceOnTimeChanged(object sender, SelectionChangedEventArgs args)
        {
            if (!(sender is Controls.ClockFace clockFace))
            {
                return;
            }

            if (_clockFaceSelectionChangedByKey)
            {
                _clockFaceSelectionChangedByKey = false;

                return;
            }

            if (AllowNull || clockFace.TimeValue != null)
            {
                UpdateTime(clockFace.TimeValue);
            }

            ((Popup)clockFace.Parent).SetCurrentValue(Popup.IsOpenProperty, _clockFaceSelectionChangedByKey);

            _clockFaceSelectionChangedByKey = false;
        }

        private bool _clockFaceSelectionChangedByKey = false;

        private void ClockFaceOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var clockFace = ((Controls.ClockFace)sender);
            if (e.Key == Key.Escape)
            {
                ((Popup)clockFace.Parent).SetCurrentValue(Popup.IsOpenProperty, false);
                _hourNumericTextBox.Focus();
                e.Handled = true;
            }

            if (e.Key != Key.Enter)
            {
                _clockFaceSelectionChangedByKey = true;

                return;
            }

            if (clockFace.TimeValue != null)
            {
                UpdateTime(clockFace.TimeValue);
            }

            ((Popup)clockFace.Parent).SetCurrentValue(Popup.IsOpenProperty, false);

            e.Handled = true;
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            IsInEditMode = true;

            EditStarted?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);

            InvalidateEditMode();
        }

        private void InvalidateEditMode()
        {
            if (!IsInEditMode)
            {
                return;
            }

            if (IsKeyboardFocusWithin || IsKeyboardFocused || IsFocused)
            {
                return;
            }

            var focusedControl = FocusManager.GetFocusedElement(this) as FrameworkElement;
            var keyboardFocusedControl = focusedControl ?? Keyboard.FocusedElement as FrameworkElement;
            var root = keyboardFocusedControl?.FindLogicalOrVisualAncestor(x => Equals(this, x));
            if (root != null)
            {
                return;
            }

            if (_clockFaceIconDropDownButton.IsChecked == true)
            {
                return;
            }

            if (_clockFacePopup != null && _clockFacePopup.IsOpen)
            {
                //_clockFacePopup.Closed += OnClockFacePopupClosed;
                return;
            }

            if (_hourToggleButton.IsChecked == true
                || _minuteToggleButton.IsChecked == true
                || _amPmToggleButton.IsChecked == true
                || _secondToggleButton.IsChecked == true)
            {
                return;
            }

            RaiseStopEdit();
        }

        private void RaiseStopEdit()
        {
            IsInEditMode = false;

            EditEnded?.Invoke(this, EventArgs.Empty);
        }

        private CultureInfo GetCulture()
        {
            return Culture ?? CultureInfo.CurrentUICulture;
        }
        #endregion

        public event EventHandler<EventArgs> EditStarted;
        public event EventHandler<EventArgs> EditEnded;
    }
}
