// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanControl.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using Catel.MVVM.Views;

    /// <summary>
    /// Interaction logic for TimeSpanControl.xaml
    /// </summary>
    [ObsoleteEx(TreatAsErrorFromVersion = "1.4", RemoveInVersion = "2.0")]
    public partial class TimeSpanControl
    {
        #region Fields
        private readonly List<NumericTextBox> _numericTextBoxes;
        private TimeSpanPart _activeTextBoxPart;
        private bool _isInEditMode;
        #endregion

        #region Constructors
        static TimeSpanControl()
        {
            typeof(TimeSpanControl).AutoDetectViewPropertiesToSubscribe();
        }

        public TimeSpanControl()
        {
            InitializeComponent();

            _numericTextBoxes = new List<NumericTextBox>()
            {
                NumericTBDays,
                NumericTBHours,
                NumericTBMinutes,
                NumericTBSeconds,
            };

            NumericTBDays.RightBoundReached += NumericTextBoxOnRightBoundReached;
            NumericTBHours.RightBoundReached += NumericTextBoxOnRightBoundReached;
            NumericTBMinutes.RightBoundReached += NumericTextBoxOnRightBoundReached;

            NumericTBHours.LeftBoundReached += NumericTextBoxOnLeftBoundReached;
            NumericTBMinutes.LeftBoundReached += NumericTextBoxOnLeftBoundReached;
            NumericTBSeconds.LeftBoundReached += NumericTextBoxOnLeftBoundReached;

            TextBlockD.MouseDown += TextBlock_MouseDown;
            TextBlockH.MouseDown += TextBlock_MouseDown;
            TextBlockM.MouseDown += TextBlock_MouseDown;
            TextBlockS.MouseDown += TextBlock_MouseDown;
        }
        #endregion

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public TimeSpan? Value
        {
            get { return (TimeSpan?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(TimeSpan?), typeof(TimeSpanControl),
            new FrameworkPropertyMetadata(TimeSpan.Zero, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((TimeSpanControl)sender).OnValueChanged(e.OldValue, e.NewValue)));

        public Brush AccentColorBrush
        {
            get { return (Brush)GetValue(AccentColorBrushProperty); }
            set { SetValue(AccentColorBrushProperty, value); }
        }

        public static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.Register("AccentColorBrush", typeof(Brush),
            typeof(TimeSpanControl), new FrameworkPropertyMetadata(Brushes.LightGray, (sender, e) => ((TimeSpanControl)sender).OnAccentColorBrushChanged()));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool),
            typeof(TimeSpanControl), new PropertyMetadata(false));

        #endregion

        #region Methods
        private void NumericTextBoxOnLeftBoundReached(object sender, EventArgs e)
        {
            var currentTextBoxIndex = _numericTextBoxes.IndexOf(sender as NumericTextBox);
            var prevTextBox = _numericTextBoxes[currentTextBoxIndex - 1];
            prevTextBox.CaretIndex = prevTextBox.Text.Length;
            prevTextBox.Focus();
        }

        private void NumericTextBoxOnRightBoundReached(object sender, EventArgs eventArgs)
        {
            var currentTextBoxIndex = _numericTextBoxes.IndexOf(sender as NumericTextBox);
            var nextTextBox = _numericTextBoxes[currentTextBoxIndex + 1];
            nextTextBox.CaretIndex = 0;
            nextTextBox.Focus();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                _activeTextBoxPart = (TimeSpanPart)((TextBlock)sender).Tag;
                NumericTBEditorContainer.SetCurrentValue(VisibilityProperty, Visibility.Visible);
                _isInEditMode = true;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Escape && _isInEditMode)
            {
                NumericTBEditorContainer.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                _isInEditMode = false;
                e.Handled = true;
            }

            if (e.Key == Key.Enter && _isInEditMode)
            {
                NumericTBEditorContainer.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
                _isInEditMode = false;

                if (!IsReadOnly)
                {
                    var value = NumericTBEditor.Value == null ? NumericTBEditor.MinValue : NumericTBEditor.Value.Value;
                    SetCurrentValue(ValueProperty, RoundTimeSpan(_activeTextBoxPart.CreateTimeSpan(value)));
                }

                e.Handled = true;
            }
        }

        private TimeSpan RoundTimeSpan(TimeSpan timeSpan)
        {
            var totalSeconds = Math.Round(timeSpan.TotalSeconds);
            return TimeSpan.FromSeconds(totalSeconds);
        }

        private void NumericTBEditor_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            NumericTBEditorUnit.SetCurrentValue(TextBlock.TextProperty, _activeTextBoxPart.GetTimeSpanPartName());
            NumericTBEditor.Focus();
        }

        private void NumericTBEditor_OnIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var timeSpan = Value == null ? TimeSpan.Zero : Value.Value;
            if (IsKeyboardFocusWithin)
            {
                NumericTBEditor.SetCurrentValue(NumericTextBox.ValueProperty, timeSpan.GetTimeSpanPartValue(_activeTextBoxPart));
                return;
            }

            NumericTBEditorContainer.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
        }

        private void OnAccentColorBrushChanged()
        {
            var solidColorBrush = AccentColorBrush as SolidColorBrush;
            if (solidColorBrush != null)
            {
                var accentColor = ((SolidColorBrush)AccentColorBrush).Color;
                accentColor.CreateAccentColorResourceDictionary("TimeSpan");
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetCurrentValue(AccentColorBrushProperty, TryFindResource("AccentColorBrush") as SolidColorBrush);
        }

        private void OnValueChanged(object oldValue, object newValue)
        {
            Separator1.SetCurrentValue(TextBlock.TextProperty, newValue == null ? string.Empty : ".");
            Separator2.SetCurrentValue(TextBlock.TextProperty, newValue == null ? string.Empty : ":");
            Separator3.SetCurrentValue(TextBlock.TextProperty, newValue == null ? string.Empty : ":");
        }

        #endregion
    }
}