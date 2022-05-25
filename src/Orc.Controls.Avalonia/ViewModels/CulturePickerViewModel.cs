namespace Orc.Controls.Avalonia.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ReactiveUI;

    public class CulturePickerViewModel : ViewModelBase
    {
        #region Fields
        private bool _changingSelectedIndex;

        private List<CultureInfo> _cultures;
        private CultureInfo _selectedCulture;
        private int _index;
        #endregion

        #region Constructors
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public CulturePickerViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            AvailableCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(culture => !string.IsNullOrEmpty(culture.Name) && !string.IsNullOrEmpty(culture.Parent.Name))
                .OrderBy(culture => culture.DisplayName).ToList();
        }
        #endregion

        #region Properties
        public CultureInfo SelectedCulture { get => _selectedCulture; set => this.RaiseAndSetIfChanged(ref _selectedCulture, value); }
        public List<CultureInfo> AvailableCultures { get => _cultures; set => this.RaiseAndSetIfChanged(ref _cultures, value); }
        public int SelectedIndex { get => _index; set => this.RaiseAndSetIfChanged(ref _index, value); }
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
