// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;
    using Catel;
    using Catel.MVVM;
    using Catel.Reflection;
    using Orc.Controls.Services;

    public class MainViewModel : ViewModelBase
    {
        private readonly IAccentColorService _accentColorService;

        #region Constructors
        public MainViewModel(IAccentColorService accentColorService)
        {
            Argument.IsNotNull(() => accentColorService);

            _accentColorService = accentColorService;

            AccentColors = typeof(Colors).GetPropertiesEx(true, true).Where(x => x.PropertyType.IsAssignableFromEx(typeof(Color))).Select(x => (Color)x.GetValue(null)).ToList();
            SelectedAccentColor = Colors.Orange;

            DeferValidationUntilFirstSaveCall = false;
        }
        #endregion

        #region Properties
        public override string Title => "Orc.Controls example";

        public List<Color> AccentColors { get; private set; }

        public Color SelectedAccentColor { get; set; }
        #endregion

        #region Methods
        private void OnSelectedAccentColorChanged()
        {
            _accentColorService.SetAccentColor(SelectedAccentColor);
        }
        #endregion
    }
}
