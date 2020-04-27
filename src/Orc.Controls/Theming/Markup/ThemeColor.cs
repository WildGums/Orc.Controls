// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccentColor.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel.Logging;
    using Catel.Windows.Markup;
    using Orc.Controls.Theming;

    public class ThemeColor : UpdatableMarkupExtension
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ControlzEx.Theming.ThemeManager _controlzThemeManager;
        private readonly ThemeManager _themeManager;
        #endregion

        #region Constructors
        public ThemeColor()
        {
            AllowUpdatableStyleSetters = true;

            _controlzThemeManager = ControlzEx.Theming.ThemeManager.Current;
            _themeManager = ThemeManager.Current;
        }

        public ThemeColor(ThemeColorStyle themeColorStyle)
            : this()
        {
            ThemeColorStyle = themeColorStyle;
        }
        #endregion

        #region Properties
        public ThemeColorStyle ThemeColorStyle { get; set; }
        #endregion

        #region Methods
        protected override void OnTargetObjectLoaded()
        {
            base.OnTargetObjectLoaded();

            _controlzThemeManager.ThemeChanged += OnThemeChanged;
        }

        protected override void OnTargetObjectUnloaded()
        {
            _controlzThemeManager.ThemeChanged -= OnThemeChanged;

            base.OnTargetObjectUnloaded();
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            UpdateValue();
        }

        protected override object ProvideDynamicValue(IServiceProvider serviceProvider)
        {
            return _themeManager.GetThemeColor(ThemeColorStyle);
        }
        #endregion
    }
}
