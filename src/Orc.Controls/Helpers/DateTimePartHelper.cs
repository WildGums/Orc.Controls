namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using Catel.IoC;
    using Catel.Windows;
    using Orc.Automation;

    public class DateTimePartHelper
    {
        #region Fields
        private readonly DateTime _dateTime;
        private readonly DateTimeFormatInfo _dateTimeFormatInfo;
        private readonly DateTimePart _dateTimePart;
        private readonly TextBox _textBox;
        private readonly ToggleButton _toggleButton;
        #endregion

        #region Constructors
        public DateTimePartHelper(DateTime dateTime, DateTimePart dateTimePart, DateTimeFormatInfo dateTimeFormatInfo, TextBox textBox, ToggleButton activeToggleButton)
        {
            _dateTime = dateTime;
            _textBox = textBox;
            _toggleButton = activeToggleButton;
            _dateTimePart = dateTimePart;
            _dateTimeFormatInfo = dateTimeFormatInfo;
        }
        #endregion

        #region Methods
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
            popupSource.PreviewKeyDown += popupSource_PreviewKeyDown;
            popupSource.MouseUp += PopupSourceOnMouseUp;

            var automationInformer = new Orc.Automation.Controls.AutomationInformer
            {
                Content = popupSource
            };

            popup.Child = automationInformer;
            SelectItem(popupSource);
            popupSource.Focus();

            return popup;
        }

        private void PopupSourceOnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var listbox = ((ListBox)sender);
            
            if(listbox.SelectedItems.Count > 0)
            {
                UpdateTextBox((KeyValuePair<string, string>)listbox.SelectedItems[0]);
            }

            listbox.FindLogicalOrVisualAncestorByType<Popup>()
                .SetCurrentValue(Popup.IsOpenProperty, false);

            _textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void popupSource_PreviewKeyDown(object sender, KeyEventArgs e)
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
                UpdateTextBox((KeyValuePair<string, string>)listbox.SelectedItems[0]);
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

        private void PopupOnClosed(object sender, EventArgs eventArgs)
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
            var suggestionListService = serviceLocator.ResolveType<ISuggestionListService>();
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
        #endregion
    }
}
