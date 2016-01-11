// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CulturePickerViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2016 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Threading;

    internal class CulturePickerViewModel : ViewModelBase
    {
        private bool _changingSelectedIndex;

        public CulturePickerViewModel()
        {
            AvailableCultures = new List<CultureInfo>();
        }

        public CultureInfo SelectedCulture { get; set; }
        public List<CultureInfo> AvailableCultures { get; set; }
        public int SelectedIndex { get; set; }

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

        protected override Task InitializeAsync()
        {
            AvailableCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(culture => !string.IsNullOrEmpty(culture.Name) && !string.IsNullOrEmpty(culture.Parent.Name))
                .OrderBy(culture => culture.DisplayName).ToList();

            return TaskHelper.Completed;
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
    }
}