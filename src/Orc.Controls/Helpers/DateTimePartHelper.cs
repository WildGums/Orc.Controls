// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePartHelper.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
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
            popupSource.SelectionChanged += PopupSourceOnSelectionChanged;

            popup.Child = popupSource;

            return popup;
        }

        private void PopupOnClosed(object sender, EventArgs eventArgs)
        {
            _toggleButton.IsChecked = false;
        }

        private void PopupSourceOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var selectedItem = ((ListBox) sender).SelectedItems[0];
            UpdateTextBox(selectedItem.ToString());
        }

        private void UpdateTextBox(string selectedItem)
        {
            switch (_dateTimePart)
            {
                case DateTimePart.Day:
                    var day = selectedItem.Split(new char[0]);
                    _textBox.Text = day[0];
                    break;
            }
            _textBox.Text = selectedItem;
        }

        private ListBox CreatePopupSource()
        {
            var serviceLocator = ServiceLocator.Default;
            var suggestionListService = serviceLocator.ResolveType<ISuggestionListService>();
            var source = suggestionListService.GetSuggestionList(_dateTime, _dateTimePart);

            return new ListBox(){ItemsSource = source};
        }
        #endregion
    }
}