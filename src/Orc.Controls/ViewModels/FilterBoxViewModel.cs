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
    using Extensions;

    public class FilterBoxViewModel : ViewModelBase
    {
        private IEnumerable _filterSource;
        private string _propertyName;
        private string _text;
        private Brush _accentColorBrushProperty;

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
                var accentColor = ((SolidColorBrush) AccentColorBrush).Color;
                accentColor.CreateAccentColorResourceDictionary();
                RaisePropertyChanged("AccentColorBrush");
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
    }
}