// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListTextBox.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class ListTextBox : TextBox
    {
        #region Fields
        private readonly bool _textChangingIsInProgress = false;
        private int _currentIndex = 0;
        #endregion

        #region Constructor
        public ListTextBox()
        {
            AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);
            AddHandler(GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);
            AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText), true);

            LostFocus += OnLostFocus;
        }
        #endregion

        #region Properties
        public IList<string> ListOfValues
        {
            get { return (IList<string>)GetValue(ListOfValuesProperty); }
            set { SetValue(ListOfValuesProperty, value); }
        }

        public static readonly DependencyProperty ListOfValuesProperty = DependencyProperty.Register("ListOfValues", typeof(IList<string>),
            typeof(ListTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string),
            typeof(ListTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, e) => ((ListTextBox)sender).OnValueChanged()));

        private bool AllTextSelected
        {
            get { return SelectedText == Text; }
        }

        private bool CaretAtStart
        {
            get { return CaretIndex == 0; }
        }

        private bool CaretAtEnd
        {
            get { return CaretIndex == Text.Length; }
        }
        #endregion

        #region Events
        public event EventHandler RightBoundReached;
        public event EventHandler LeftBoundReached;
        #endregion

        #region Methods
        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            SetCurrentValue(ValueProperty, Text);
            UpdateText();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            var allTextSelected = AllTextSelected;
            var caretAtEnd = CaretAtEnd;
            var caretAtStart = CaretAtStart;
            var isReadOnly = IsReadOnly;

            if (e.Key == Key.Right && (caretAtEnd || caretAtStart && allTextSelected))
            {
                RaiseRightBoundReachedEvent();
                e.Handled = true;
            }

            if (e.Key == Key.Left && caretAtStart)
            {
                RaiseLeftBoundReachedEvent();
                e.Handled = true;
            }

            if (e.Key == Key.Up && allTextSelected && !isReadOnly)
            {
                OnUpDown(1);
                e.Handled = true;
            }

            if (e.Key == Key.Down && allTextSelected && !isReadOnly)
            {
                OnUpDown(-1);
                e.Handled = true;
            }
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);

            var listOfValues = ListOfValues;
            var isReadOnly = IsReadOnly;

            if (listOfValues != null && listOfValues.Count > 0 && !isReadOnly)
            {
                var text = GetText(e.Text);
                if (text.Length > 0)
                {
                    var value = listOfValues.FirstOrDefault(x => x.StartsWith(text, StringComparison.CurrentCultureIgnoreCase));
                    if (value != null)
                    {
                        var index = listOfValues.IndexOf(value);
                        if (index >= 0)
                        {
                            _currentIndex = index >= 0 ? index : 0;

                            SetCurrentValue(ValueProperty, value);

                            UpdateText();
                            SelectAll();

                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private void OnUpDown(int increment)
        {
            var listOfValues = ListOfValues;

            if (listOfValues == null || listOfValues.Count == 0)
            {
                return;
            }

            _currentIndex = _currentIndex + increment;
            if (_currentIndex >= listOfValues.Count)
            {
                _currentIndex = 0;
            }
            else if (_currentIndex < 0)
            {
                _currentIndex = listOfValues.Count - 1;
            }

            SetCurrentValue(ValueProperty, listOfValues[_currentIndex]);

            SelectAll();
        }

        private void RaiseRightBoundReachedEvent()
        {
            RightBoundReached?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseLeftBoundReachedEvent()
        {
            LeftBoundReached?.Invoke(this, EventArgs.Empty);
        }

        private string GetText(string inputText)
        {
            var text = new StringBuilder(base.Text);
            if (!string.IsNullOrEmpty(SelectedText))
            {
                text.Remove(CaretIndex, SelectedText.Length);
            }
            text.Insert(CaretIndex, inputText);
            return (text.ToString());
        }

        private static void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent != null)
            {
                var textBox = (TextBox)parent;
                if (!textBox.IsKeyboardFocusWithin)
                {
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
            {
                textBox.SelectAll();
            }
        }

        private void OnValueChanged()
        {
            if (_textChangingIsInProgress)
            {
                return;
            }

            var listOfValues = ListOfValues;

            if (Value != null)
            {
                if (listOfValues != null && listOfValues.Count > 0)
                {
                    var item = listOfValues.FirstOrDefault(x => string.Equals(x, Value, StringComparison.CurrentCultureIgnoreCase));
                    if (item != null)
                    {
                        var index = listOfValues.IndexOf(item);
                        _currentIndex = index;

                        SetCurrentValue(ValueProperty, item);
                    }
                    else
                    {
                        _currentIndex = 0;
                    }
                }
                else
                {
                    SetCurrentValue(ValueProperty, null);
                }
            }

            UpdateText();
        }

        private void UpdateText()
        {
            SetCurrentValue(TextProperty, Value == null ? string.Empty : Value);
        }
        #endregion
    }
}
