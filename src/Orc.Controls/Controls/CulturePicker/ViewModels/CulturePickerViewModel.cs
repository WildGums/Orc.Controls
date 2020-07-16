// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CulturePickerViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Catel.MVVM;

    internal class CulturePickerViewModel : ViewModelBase
    {
        #region Fields
        private bool _changingSelectedIndex;
        #endregion

        #region Constructors
        public CulturePickerViewModel()
        {
            AvailableCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(culture => !string.IsNullOrEmpty(culture.Name) && !string.IsNullOrEmpty(culture.Parent.Name))
                .OrderBy(culture => culture.DisplayName).ToList();
        }
        #endregion

        #region Properties
        public CultureInfo SelectedCulture { get; set; }
        public List<CultureInfo> AvailableCultures { get; private set; }
        public int SelectedIndex { get; set; }
        #endregion

        #region Methods
        private void OnSelectedCultureChanged()
        {
            if (_changingSelectedIndex)
            {
                return;
            }

            var index = AvailableCultures.IndexOf(SelectedCulture);
            SetSelectedIndex(index);
        }

        private void OnSelectedIndexChanged()
        {
            var selectedIndex = SelectedIndex;
            if (_changingSelectedIndex || selectedIndex < 0)
            {
                return;
            }

            SelectedCulture = AvailableCultures[selectedIndex];
        }

        private void SetSelectedIndex(int index)
        {
            _changingSelectedIndex = true;

            try
            {
                SelectedIndex = index;
            }
            finally
            {
                _changingSelectedIndex = false;
            }
        }
        #endregion
    }
}
