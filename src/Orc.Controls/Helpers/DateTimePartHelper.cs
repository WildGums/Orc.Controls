// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePartHelper.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using Catel.IoC;
    using Services.Interfaces;

    public class DateTimePartHelper
    {
        private readonly DateTime _dateTime;
        private readonly NumericTextBox _textBox;
        private readonly DateTimePart _dateTimePart;

        public DateTimePartHelper(DateTime dateTime,DateTimePart dateTimePart, NumericTextBox textBox)
        {
            _dateTime = dateTime;
            _textBox = textBox;
            _dateTimePart = dateTimePart;
        }

        #region Methods
        public Popup CreatePopup()
        {
            var popup = new Popup();
            var popupSource = CreatePopupSource();
            popup.Child = popupSource;

            popup.Name = "comboBox";
            popup.MinWidth = _textBox.ActualWidth + 25;
            popup.MaxHeight = 100;
            popup.PlacementTarget = _textBox;
            popup.Placement = PlacementMode.Bottom;
            popup.VerticalOffset = 2;
            popup.IsOpen = true;
            popup.StaysOpen = false;

            return popup;
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