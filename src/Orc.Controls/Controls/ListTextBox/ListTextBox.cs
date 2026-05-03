namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Automation;

public partial class ListTextBox : TextBox
{
    private int _currentIndex;

    static ListTextBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ListTextBox), new FrameworkPropertyMetadata(typeof(ListTextBox)));
    }

    public ListTextBox()
    {
        AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);
        AddHandler(GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);
        AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText), true);

        LostFocus += OnLostFocus;
    }

    public IList<string>? ListOfValues
    {
        get { return (IList<string>?)GetValue(ListOfValuesProperty); }
        set { SetValue(ListOfValuesProperty, value); }
    }

    public static readonly DependencyProperty ListOfValuesProperty = DependencyProperty.Register(nameof(ListOfValues), typeof(IList<string>),
        typeof(ListTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string? Value
    {
        get { return (string?)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(string),
        typeof(ListTextBox), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
            (sender, _) => ((ListTextBox)sender).OnValueChanged()));

    private bool AllTextSelected => SelectedText == Text;

    private bool CaretAtStart => CaretIndex == 0;

    private bool CaretAtEnd => CaretIndex == Text.Length;

    public event EventHandler<EventArgs>? RightBoundReached;
    public event EventHandler<EventArgs>? LeftBoundReached;
    public event EventHandler<EventArgs>? ValueChanged;

    private void OnLostFocus(object? _, RoutedEventArgs e)
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

        if (listOfValues is null || listOfValues.Count <= 0 || isReadOnly)
        {
            return;
        }
            
        if (!listOfValues.Any(x => x.StartsWith(e.TextComposition.Text, StringComparison.OrdinalIgnoreCase)))
        {
            e.Handled = true;

            return;
        }

        var text = GetText(e.Text);
        if (text.Length <= 0)
        {
            return;
        }

        var value = listOfValues.FirstOrDefault(x => x.StartsWith(text, StringComparison.CurrentCultureIgnoreCase));
        if (value is null)
        {
            return;
        }

        var index = listOfValues.IndexOf(value);
        if (index < 0)
        {
            return;
        }

        _currentIndex = index;

        SetCurrentValue(ValueProperty, value);

        UpdateText();
        SelectAll();

        e.Handled = true;
    }

    private void OnUpDown(int increment)
    {
        var listOfValues = ListOfValues;

        if (listOfValues is null || listOfValues.Count == 0)
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

    private static void SelectivelyIgnoreMouseButton(object? _, MouseButtonEventArgs e)
    {
        DependencyObject? parent = e.OriginalSource as UIElement;
        while (parent is not null && parent is not TextBox)
        {
            parent = VisualTreeHelper.GetParent(parent);
        }

        if (parent is null)
        {
            return;
        }

        var textBox = (TextBox)parent;
        if (textBox.IsKeyboardFocusWithin)
        {
            return;
        }

        textBox.Focus();
        e.Handled = true;
    }

    private static void SelectAllText(object sender, RoutedEventArgs e)
    {
        var textBox = e.OriginalSource as TextBox;
        textBox?.SelectAll();
    }

    private void OnValueChanged()
    {
        var listOfValues = ListOfValues;

        if (Value is not null)
        {
            if (listOfValues is not null && listOfValues.Count > 0)
            {
                var item = listOfValues.FirstOrDefault(x => string.Equals(x, Value, StringComparison.CurrentCultureIgnoreCase));
                if (item is not null)
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

        ValueChanged?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateText()
    {
        SetCurrentValue(TextProperty, Value ?? string.Empty);
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new ListTextBoxAutomationPeer(this);
    }
}
