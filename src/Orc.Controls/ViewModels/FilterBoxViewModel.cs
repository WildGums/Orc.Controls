// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBoxViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.ViewModels
{
    using System.Collections;
    using System.Windows.Media;
    using Catel.MVVM;

    public class FilterBoxViewModel : ViewModelBase
    {
        private IEnumerable _filterSource;
        private string _propertyName;
        private string _text;
        private Brush _accentColorBrushProperty;
        private Brush _highlightColorBrush;

        public FilterBoxViewModel()
        {
            ClearFilter = new Command(OnClearFilterExecute, OnClearFilterCanExecute);
        }

        public Brush AccentColorBrush
        {
            get { return _accentColorBrushProperty; }
            set
            {
                if (_accentColorBrushProperty == value)
                {
                    return;
                }

                _accentColorBrushProperty = value;
                HighlightColorBrush = GetHighlightBrush(_accentColorBrushProperty);

                RaisePropertyChanged("AccentColorBrush");
            }
        }

        public Brush HighlightColorBrush
        {
            get { return _highlightColorBrush; }
            set
            {
                if (_highlightColorBrush == value)
                {
                    return;
                }

                _highlightColorBrush = value;

                RaisePropertyChanged("HighlightColorBrush");
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value)
                {
                    return;
                }

                _text = value;

                // Required for mappings
                RaisePropertyChanged("Text");
            }
        }

        public IEnumerable FilterSource
        {
            get { return _filterSource; }
            set
            {
                if (_filterSource == value)
                {
                    return;
                }

                _filterSource = value;

                // Required for mappings
                RaisePropertyChanged("FilterSource");
            }
        }

        public string PropertyName
        {
            get { return _propertyName; }
            set
            {
                if (_propertyName == value)
                {
                    return;
                }

                _propertyName = value;

                // Required for mappings
                RaisePropertyChanged("PropertyName");
            }
        }

        public Command ClearFilter { get; private set; }

        private bool OnClearFilterCanExecute()
        {
            return !string.IsNullOrWhiteSpace(Text);
        }

        private void OnClearFilterExecute()
        {
            Text = null;
        }

        private Brush GetHighlightBrush(Brush brush)
        {
            var color = ((SolidColorBrush)brush).Color;
            return new SolidColorBrush(Color.FromArgb(51, color.R, color.G, color.B));
        }
    }
}