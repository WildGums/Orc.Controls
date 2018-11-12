// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccentColorBrush.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Windows.Markup;
    using Services;

    public class AccentColorBrush : UpdatableMarkupExtension
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IAccentColorService _accentColorService;
        #endregion

        #region Constructors
        static AccentColorBrush()
        {
            // Note: use ThemeHelper so it subscribes to events first
            var dummyCall = ThemeHelper.GetAccentColor();
        }

        public AccentColorBrush()
        {
            AllowUpdatableStyleSetters = true;

            _accentColorService = ServiceLocator.Default.ResolveType<IAccentColorService>();
        }

        public AccentColorBrush(AccentColorStyle accentColorStyle)
            : this()
        {
            AccentColorStyle = accentColorStyle;
        }
        #endregion

        #region Properties
        public AccentColorStyle AccentColorStyle { get; set; }
        #endregion

        #region Methods
        protected override void OnTargetObjectLoaded()
        {
            base.OnTargetObjectLoaded();

            _accentColorService.AccentColorChanged += OnAccentColorChanged;
        }

        protected override void OnTargetObjectUnloaded()
        {
            _accentColorService.AccentColorChanged -= OnAccentColorChanged;

            base.OnTargetObjectUnloaded();
        }

        private void OnAccentColorChanged(object sender, EventArgs e)
        {
            UpdateValue();
        }

        protected override object ProvideDynamicValue(IServiceProvider serviceProvider)
        {
            return ThemeHelper.GetAccentColorBrush(AccentColorStyle);
        }
        #endregion
    }
}
