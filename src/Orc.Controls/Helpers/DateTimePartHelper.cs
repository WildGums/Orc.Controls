// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePartHelper.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using Catel.IoC;
    using Services.Interfaces;

    public class DateTimePartHelper
    {
        private readonly DateTime _dateTime;
        private readonly NumericTextBox _textBox;
        private readonly ToggleButton _toggleButton;
        private readonly DateTimePart _dateTimePart;

        public DateTimePartHelper(DateTime dateTime, DateTimePart dateTimePart, NumericTextBox textBox, ToggleButton activeToggleButton)
        {
            _dateTime = dateTime;
            _textBox = textBox;
            _toggleButton = activeToggleButton;
            _dateTimePart = dateTimePart;
        }

        #region Methods
        public Popup CreatePopup()
        {
            var popup = new Popup
            {
                MinWidth = _textBox.ActualWidth + 25,
                MaxHeight = 100,
                PlacementTarget = _textBox,
                Placement = PlacementMode.Bottom,
                VerticalOffset = 2,
                IsOpen = true,
                StaysOpen = false
            };

            popup.Closed += PopupOnClosed;

            var popupSource = CreatePopupSource();
            popupSource.PreviewKeyDown += popupSource_PreviewKeyDown;
            popupSource.MouseDoubleClick += PopupSourceOnMouseDoubleClick;

            popup.Child = popupSource;
            SelectItem(popupSource);
            popupSource.Focus();
            
            return popup;
        }

        private void PopupSourceOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var listbox = ((ListBox)sender);
            UpdateTextBox(listbox.SelectedItems[0].ToString());
            ((Popup)listbox.Parent).IsOpen = false;
            _textBox.Focus();
        }

        void popupSource_PreviewKeyDown(object sender, KeyEventArgs e)
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
                ((Popup)listbox.Parent).IsOpen = false;
                _textBox.Focus();
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                UpdateTextBox(listbox.SelectedItems[0].ToString());
                ((Popup)listbox.Parent).IsOpen = false;
                _textBox.Focus();
                e.Handled = true;
            }
        }

        private void SelectItem(ListBox listBox)
        {
            foreach (var item in listBox.Items)
            {
                if (item.ToString().StartsWith(_textBox.Text))
                {
                    listBox.SelectedItem = item;
                    listBox.ScrollIntoView(listBox.SelectedItem);
                    listBox.Focus();

                    break;
                }
            }
        }

        private void PopupOnClosed(object sender, EventArgs eventArgs)
        {
            _toggleButton.IsChecked = false;
        }

        private void UpdateTextBox(string selectedItem)
        {
            _textBox.Text = selectedItem;
        }

        private ListBox CreatePopupSource()
        {
            var serviceLocator = ServiceLocator.Default;
            var suggestionListService = serviceLocator.ResolveType<ISuggestionListService>();
            var source = suggestionListService.GetSuggestionList(_dateTime, _dateTimePart);

            var listbox = new ListBox() { ItemsSource = source, IsSynchronizedWithCurrentItem = false};
            return listbox;
        }
        #endregion
    }
}