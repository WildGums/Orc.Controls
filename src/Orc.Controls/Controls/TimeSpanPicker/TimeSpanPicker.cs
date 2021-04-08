// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanPicker.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Catel.Logging;

    [TemplatePart(Name = "PART_DaysNumericTextBox", Type = typeof(NumericTextBox))]
    [TemplatePart(Name = "PART_HoursNumericTextBox", Type = typeof(NumericTextBox))]
    [TemplatePart(Name = "PART_MinutesNumericTextBox", Type = typeof(NumericTextBox))]
    [TemplatePart(Name = "PART_SecondsNumericTextBox", Type = typeof(NumericTextBox))]
    [TemplatePart(Name = "PART_EditorNumericTextBox", Type = typeof(NumericTextBox))]

    [TemplatePart(Name = "PART_DaysAbbreviationTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_HoursAbbreviationTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_MinutesAbbreviationTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_SecondsAbbreviationTextBlock", Type = typeof(TextBlock))]

    [TemplatePart(Name = "PART_DaysHoursSeparatorTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_HoursMinutesSeparatorTextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_MinutesSecondsSeparatorTextBlock", Type = typeof(TextBlock))]

    [TemplatePart(Name = "PART_EditedUnitTextBlock", Type = typeof(TextBlock))]

    [TemplatePart(Name = "PART_NumericTBEditorContainerBorder", Type = typeof(Border))]
    public class TimeSpanPicker : Control
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private List<NumericTextBox> _numericTextBoxes;
        private TimeSpanPart _activeTextBoxPart;
        private bool _isInEditMode;

        private NumericTextBox _daysNumericTextBox;
        private NumericTextBox _hoursNumericTextBox;
        private NumericTextBox _minutesNumericTextBox;
        private NumericTextBox _secondsNumericTextBox;
        private NumericTextBox _editorNumericTextBox;

        private TextBlock _daysAbbreviationTextBlock;
        private TextBlock _hoursAbbreviationTextBlock;
        private TextBlock _minutesAbbreviationTextBlock;
        private TextBlock _secondsAbbreviationTextBlock;

        private TextBlock _daysHoursSeparatorTextBlock;
        private TextBlock _hoursMinutesSeparatorTextBlock;
        private TextBlock _minutesSecondsSeparatorTextBlock;

        private TextBlock _editedUnitTextBlock;

        private Border _numericTbEditorContainerBorder;

        private bool _isTemplateApplied;
        #endregion

        #region Properties
        private int Days
        {
            get => (int)(_daysNumericTextBox.Value ?? 0);
        }

        private int Hours
        {
            get => (int)(_hoursNumericTextBox.Value ?? 0);
        }

        private int Minutes
        {
            get => (int)(_minutesNumericTextBox.Value ?? 0);
        }

        private int Seconds
        {
            get => (int)(_secondsNumericTextBox.Value ?? 0);
        }
        #endregion

        #region Dependency properties
        public TimeSpan? Value
        {
            get { return (TimeSpan?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(TimeSpan?), typeof(TimeSpanPicker),
            new FrameworkPropertyMetadata(TimeSpan.Zero, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                (sender, e) => ((TimeSpanPicker)sender).OnValueChanged(e.NewValue as TimeSpan?)));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool),
            typeof(TimeSpanPicker), new PropertyMetadata(false));
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _numericTextBoxes = new List<NumericTextBox>();

            /*Days numeric text box*/
            _daysNumericTextBox = GetTemplateChild("PART_DaysNumericTextBox") as NumericTextBox;
            if (_daysNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_DaysNumericTextBox'");
            }
            _daysNumericTextBox.ValueChanged += OnDaysValueChanged;
            _daysNumericTextBox.RightBoundReached += OnNumericTextBoxRightBoundReached;
            _numericTextBoxes.Add(_daysNumericTextBox);

            /*Hours numeric text box*/
            _hoursNumericTextBox = GetTemplateChild("PART_HoursNumericTextBox") as NumericTextBox;
            if (_hoursNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_HoursNumericTextBox'");
            }
            _hoursNumericTextBox.ValueChanged += OnHoursValueChanged;
            _hoursNumericTextBox.RightBoundReached += OnNumericTextBoxRightBoundReached;
            _hoursNumericTextBox.LeftBoundReached += OnNumericTextBoxLeftBoundReached;
            _numericTextBoxes.Add(_hoursNumericTextBox);

            /*Minutes numeric text box*/
            _minutesNumericTextBox = GetTemplateChild("PART_MinutesNumericTextBox") as NumericTextBox;
            if (_minutesNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_MinutesNumericTextBox'");
            }
            _minutesNumericTextBox.ValueChanged += OnMinutesValueChanged;
            _minutesNumericTextBox.RightBoundReached += OnNumericTextBoxRightBoundReached;
            _minutesNumericTextBox.LeftBoundReached += OnNumericTextBoxLeftBoundReached;
            _numericTextBoxes.Add(_minutesNumericTextBox);

            /*Seconds numeric text box*/
            _secondsNumericTextBox = GetTemplateChild("PART_SecondsNumericTextBox") as NumericTextBox;
            if (_secondsNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_SecondsNumericTextBox'");
            }
            _secondsNumericTextBox.ValueChanged += OnSecondsValueChanged;
            _secondsNumericTextBox.LeftBoundReached += OnNumericTextBoxLeftBoundReached;
            _numericTextBoxes.Add(_secondsNumericTextBox);

            /*Editor numeric text box*/
            _editorNumericTextBox = GetTemplateChild("PART_EditorNumericTextBox") as NumericTextBox;
            if (_editorNumericTextBox is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_EditorNumericTextBox'");
            }
            _editorNumericTextBox.ValueChanged += OnEditorValueChanged;
            _editorNumericTextBox.IsKeyboardFocusWithinChanged += OnEditorNumericTextBoxIsKeyboardFocusWithinChanged;
            _editorNumericTextBox.IsVisibleChanged += OnEditorNumericTextBoxIsVisibleChanged;

            /*Editor container border*/
            _numericTbEditorContainerBorder = GetTemplateChild("PART_NumericTBEditorContainerBorder") as Border;
            if (_numericTbEditorContainerBorder is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_NumericTBEditorContainerBorder'");
            }

            /*Currently editing unit text block*/
            _editedUnitTextBlock = GetTemplateChild("PART_EditedUnitTextBlock") as TextBlock;
            if (_editedUnitTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_EditedUnitTextBlock'");
            }

            /*Days abbreviation text block*/
            _daysAbbreviationTextBlock = GetTemplateChild("PART_DaysAbbreviationTextBlock") as TextBlock;
            if (_daysAbbreviationTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_DaysAbbreviationTextBlock'");
            }
            _daysAbbreviationTextBlock.MouseDown += OnAbbreviationTextBlockOnMouseDown;

            /*Hours abbreviation text block*/
            _hoursAbbreviationTextBlock = GetTemplateChild("PART_HoursAbbreviationTextBlock") as TextBlock;
            if (_hoursAbbreviationTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_HoursAbbreviationTextBlock'");
            }
            _hoursAbbreviationTextBlock.MouseDown += OnAbbreviationTextBlockOnMouseDown;

            /*Minutes abbreviation text block*/
            _minutesAbbreviationTextBlock = GetTemplateChild("PART_MinutesAbbreviationTextBlock") as TextBlock;
            if (_minutesAbbreviationTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_MinutesAbbreviationTextBlock'");
            }
            _minutesAbbreviationTextBlock.MouseDown += OnAbbreviationTextBlockOnMouseDown;

            /*Seconds abbreviation text block*/
            _secondsAbbreviationTextBlock = GetTemplateChild("PART_SecondsAbbreviationTextBlock") as TextBlock;
            if (_secondsAbbreviationTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_SecondsAbbreviationTextBlock'");
            }
            _secondsAbbreviationTextBlock.MouseDown += OnAbbreviationTextBlockOnMouseDown;

            /*Days-Hours separator text block*/
            _daysHoursSeparatorTextBlock = GetTemplateChild("PART_DaysHoursSeparatorTextBlock") as TextBlock;
            if (_daysHoursSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_DaysHoursSeparatorTextBlock'");
            }

            /*Hours-Minutes separator text block*/
            _hoursMinutesSeparatorTextBlock = GetTemplateChild("PART_HoursMinutesSeparatorTextBlock") as TextBlock;
            if (_hoursMinutesSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_HoursMinutesSeparatorTextBlock'");
            }

            /*Minutes-Seconds separator text block*/
            _minutesSecondsSeparatorTextBlock = GetTemplateChild("PART_MinutesSecondsSeparatorTextBlock") as TextBlock;
            if (_minutesSecondsSeparatorTextBlock is null)
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>($"Can't find template part 'PART_MinutesSecondsSeparatorTextBlock'");
            }

            _isTemplateApplied = true;

            UpdateUi();
        }

        protected override void OnIsKeyboardFocusedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusedChanged(e);

            _daysNumericTextBox.SetCurrentValue(FocusableProperty, true);
            Keyboard.Focus(_daysNumericTextBox);
        }

        private void OnEditorValueChanged(object sender, EventArgs e)
        {
            if (!IsReadOnly)
            {
                var value = _editorNumericTextBox.Value ?? _editorNumericTextBox.MinValue;
                var timeSpan = _activeTextBoxPart.CreateTimeSpan(value).RoundTimeSpan();
                
                SetCurrentValue(ValueProperty, timeSpan);
            }
        }

        private void OnEditorNumericTextBoxIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        { 
            _editedUnitTextBlock.SetCurrentValue(TextBlock.TextProperty, _activeTextBoxPart.GetTimeSpanPartName());
            _editorNumericTextBox.Focus();
        }


        private void OnEditorNumericTextBoxIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var timeSpan = Value ?? TimeSpan.Zero;
            if (IsKeyboardFocusWithin)
            {
                var textBoxValue = timeSpan.GetTimeSpanPartValue(_activeTextBoxPart);
                _editorNumericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, textBoxValue);

                _editorNumericTextBox.UpdateText();

                return;
            }

            _numericTbEditorContainerBorder.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
        }

        private void OnDaysValueChanged(object sender, EventArgs args)
        {
            var days = Days;
            var value = Value;
            if (value == null)
            {
                SetCurrentValue(ValueProperty, new TimeSpan(days, 0, 0, 0));
            }
            else
            {
                if (value.Value.Days == days)
                {
                    return;
                }

                SetCurrentValue(ValueProperty, new TimeSpan(days, Hours, Minutes, Seconds));
            }
        }

        private void OnHoursValueChanged(object sender, EventArgs args)
        {
            var hours = Hours;
            var value = Value;
            if (value == null)
            {
                SetCurrentValue(ValueProperty, new TimeSpan(0, hours, 0, 0));
            }
            else
            {
                if (value.Value.Hours == hours)
                {
                    return;
                }

                SetCurrentValue(ValueProperty, new TimeSpan(Days, hours, Minutes, Seconds));
            }
        }

        private void OnMinutesValueChanged(object sender, EventArgs args)
        {
            var minutes = Minutes;
            var value = Value;
            if (value == null)
            {
                SetCurrentValue(ValueProperty, new TimeSpan(0, 0, minutes, 0));
            }
            else
            {
                if (value.Value.Minutes == minutes)
                {
                    return;
                }

                SetCurrentValue(ValueProperty, new TimeSpan(Days, Hours, minutes, Seconds));
            }
        }

        private void OnSecondsValueChanged(object sender, EventArgs args)
        {
            var seconds = Seconds;
            var value = Value;
            if (value == null)
            {
                SetCurrentValue(ValueProperty, new TimeSpan(0, 0, 0, seconds));
            }
            else
            {
                if (value.Value.Seconds == seconds)
                {
                    return;
                }

                SetCurrentValue(ValueProperty, new TimeSpan(Days, Hours, Minutes, seconds));
            }
        }

        private void OnValueChanged(TimeSpan? value)
        {
            if (!_isTemplateApplied)
            {
                return;
            }

            UpdateUi();
        }

        private void UpdateUi()
        {
            var value = Value;

            _daysNumericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, (double?)(value?.Days ?? 0));
            _hoursNumericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, (double?)(value?.Hours ?? 0));
            _minutesNumericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, (double?)(value?.Minutes ?? 0));
            _secondsNumericTextBox.SetCurrentValue(NumericTextBox.ValueProperty, (double?)(value?.Seconds ?? 0));

            _daysHoursSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, value == null ? string.Empty : ".");
            _hoursMinutesSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, value == null ? string.Empty : ":");
            _minutesSecondsSeparatorTextBlock.SetCurrentValue(TextBlock.TextProperty, value == null ? string.Empty : ":");
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Escape && _isInEditMode)
            {
                _numericTbEditorContainerBorder.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                _isInEditMode = false;
                e.Handled = true;
            }

            var isTabPressed = e.Key == Key.Tab;
            if (!isTabPressed && e.Key != Key.Enter || !_isInEditMode)
            {
                return;
            }

            _numericTbEditorContainerBorder.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            _isInEditMode = false;

            if (!IsReadOnly)
            {
                _editorNumericTextBox.UpdateValue();

                var value = _editorNumericTextBox.Value ?? _editorNumericTextBox.MinValue;
                var timeSpan = _activeTextBoxPart.CreateTimeSpan(value).RoundTimeSpan();

                SetCurrentValue(ValueProperty, timeSpan);
            }

            e.Handled = !isTabPressed;
        }

        private void OnAbbreviationTextBlockOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
            {
                return;
            }

            _activeTextBoxPart = (TimeSpanPart)((TextBlock)sender).Tag;
            _numericTbEditorContainerBorder.SetCurrentValue(VisibilityProperty, Visibility.Visible);

            _isInEditMode = true;
        }

        private void OnNumericTextBoxRightBoundReached(object sender, EventArgs args)
        {
            var currentTextBoxIndex = _numericTextBoxes.IndexOf(sender as NumericTextBox);
            var nextTextBox = _numericTextBoxes[currentTextBoxIndex + 1];
            nextTextBox.CaretIndex = 0;
            nextTextBox.Focus();
        }

        private void OnNumericTextBoxLeftBoundReached(object sender, EventArgs args)
        {
            var currentTextBoxIndex = _numericTextBoxes.IndexOf(sender as NumericTextBox);
            var prevTextBox = _numericTextBoxes[currentTextBoxIndex - 1];
            prevTextBox.CaretIndex = prevTextBox.Text.Length;
            prevTextBox.Focus();
        }
    }
}
