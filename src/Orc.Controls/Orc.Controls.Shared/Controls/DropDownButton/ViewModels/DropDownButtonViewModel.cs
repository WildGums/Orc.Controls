// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropDownButtonViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Media;
    using Catel.MVVM;

    internal class DropDownButtonViewModel : ViewModelBase
    {
        private Brush _accentColorBrushProperty;

        public DropDownButtonViewModel()
        {
            Items = new List<string>();
        }

        public IList<string> Items { get; set; }
        public bool IsDropDownOpen { get; set; }
        public object Header { get; set; }
        public bool ShowDefaultButton { get; set; }

        public Brush AccentColorBrush
        {
            get { return _accentColorBrushProperty; }
            set
            {
                if (Equals(_accentColorBrushProperty, value))
                {
                    return;
                }

                _accentColorBrushProperty = value;
                var accentColor = ((SolidColorBrush) AccentColorBrush).Color;
                accentColor.CreateAccentColorResourceDictionary();
                RaisePropertyChanged("AccentColorBrush");
            }
        }
    }
}