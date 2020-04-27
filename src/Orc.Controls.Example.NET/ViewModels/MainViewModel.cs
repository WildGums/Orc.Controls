// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Catel;
    using Catel.MVVM;
    using Catel.Reflection;
    using ControlzEx.Theming;

    public class MainViewModel : ViewModelBase
    {
        #region Constructors
        public MainViewModel()
        {
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
            var application = Application.Current;
            var themeManager = ControlzEx.Theming.ThemeManager.Current;
            var themeGenerator = RuntimeThemeGenerator.Current;

            var generatedTheme = themeGenerator.GenerateRuntimeTheme("Light", SelectedAccentColor);
            themeManager.ChangeTheme(application, generatedTheme);
        }
        #endregion
    }
}
