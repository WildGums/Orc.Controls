namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Catel.IoC;
using Catel.Windows;

public class DateTimePartHelper
{
    private readonly DateTime _dateTime;
    private readonly DateTimeFormatInfo _dateTimeFormatInfo;
    private readonly DateTimePart _dateTimePart;

    private readonly TextBox _textBox;
    private readonly ToggleButton _toggleButton;

    public DateTimePartHelper(DateTime dateTime, DateTimePart dateTimePart, DateTimeFormatInfo dateTimeFormatInfo, TextBox textBox,
        ToggleButton activeToggleButton)
    {
        ArgumentNullException.ThrowIfNull(dateTimeFormatInfo);
        ArgumentNullException.ThrowIfNull(textBox);
        ArgumentNullException.ThrowIfNull(activeToggleButton);

        _dateTime = dateTime;
        _textBox = textBox;
        _toggleButton = activeToggleButton;
        _dateTimePart = dateTimePart;
        _dateTimeFormatInfo = dateTimeFormatInfo;
    }

    public Popup CreatePopup()
    {
        var popup = new Popup
        {
            Name = "DatePartPopupId",
            MinWidth = _textBox.ActualWidth + 25,
            MaxHeight = 100,
            PlacementTarget = _textBox,
            Placement = PlacementMode.Bottom,
            VerticalOffset = 2,
            IsOpen = true,
            StaysOpen = false,
        };

        popup.SetCurrentValue(AutomationProperties.AutomationIdProperty, "DatePartPopupId");

        popup.Closed += PopupOnClosed;

        var popupSource = CreatePopupSource();
        popupSource.PreviewKeyDown += OnPopupSourcePreviewKeyDown;
        popupSource.MouseUp += OnPopupSourceMouseUp;

        var automationInformer = new Orc.Automation.Controls.AutomationInformer
        {
            Content = popupSource
        };

        popup.Child = automationInformer;
        SelectItem(popupSource);
        popupSource.Focus();

        return popup;
    }

    private void OnPopupSourceMouseUp(object? sender, MouseButtonEventArgs mouseButtonEventArgs)
    {
        if (sender is not ListBox listbox)
        {
            return;
        }

        var selectedItems = listbox.SelectedItems
            .OfType<KeyValuePair<string, string>>()
            .ToArray();
        if(selectedItems.Length > 0)
        {
            UpdateTextBox(selectedItems[0]);
        }

        listbox.FindLogicalOrVisualAncestorByType<Popup>()
            ?.SetCurrentValue(Popup.IsOpenProperty, false);

        _textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
    }

    private void OnPopupSourcePreviewKeyDown(object sender, KeyEventArgs e)
    {
        var listbox = ((ListBox)sender);
        if (e.Key == Key.Down)
        {
            if (listbox.SelectedIndex < listbox.Items.Count)
            {
                listbox.SelectedIndex++;
                listbox.ScrollIntoView(listbox.SelectedItem);
                e.Handled = true;
            }
        }

        if (e.Key == Key.Up)
        {
            if (listbox.SelectedIndex > 0)
            {
                listbox.SelectedIndex--;
                listbox.ScrollIntoView(listbox.SelectedItem);
                e.Handled = true;
            }
        }

        if (e.Key == Key.Escape)
        {
            ((Popup)listbox.Parent).SetCurrentValue(Popup.IsOpenProperty, false);
            _textBox.Focus();
            e.Handled = true;
        }

        if (e.Key == Key.Enter)
        {
            var selectedItems = listbox.SelectedItems
                .OfType<KeyValuePair<string, string>>()
                .ToArray();

            if (selectedItems.Length > 0)
            {
                UpdateTextBox(selectedItems[0]);
            }
            
            ((Popup)listbox.Parent).SetCurrentValue(Popup.IsOpenProperty, false);
            _textBox.Focus();
            e.Handled = true;
        }
    }

    private void SelectItem(ListBox listBox)
    {
        foreach (var item in listBox.Items)
        {
            if (((KeyValuePair<string, string>)item).Key != _textBox.Text)
            {
                continue;
            }

            listBox.SetCurrentValue(Selector.SelectedItemProperty, item);
            listBox.ScrollIntoView(listBox.SelectedItem);
            listBox.Focus();

            break;
        }
    }

    private void PopupOnClosed(object? sender, EventArgs eventArgs)
    {
        _toggleButton.SetCurrentValue(ToggleButton.IsCheckedProperty, false);
    }

    private void UpdateTextBox(KeyValuePair<string, string> selectedItem)
    {
        _textBox.UpdateValue(selectedItem.Key);
    }

    private ListBox CreatePopupSource()
    {
        var serviceLocator = ServiceLocator.Default;
        var suggestionListService = serviceLocator.ResolveRequiredType<ISuggestionListService>();
        var source = suggestionListService.GetSuggestionList(_dateTime, _dateTimePart, _dateTimeFormatInfo);

        var listbox = new ListBox
        {
            Name="DatePartListBoxId",
            ItemsSource = source,
            IsSynchronizedWithCurrentItem = false,
            DisplayMemberPath = nameof(KeyValuePair<string, string>.Value),
            Margin = new Thickness(0, 0, 0, 0),
        };

        return listbox;
    }
}
