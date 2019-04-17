// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccentColorBrush.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;

    [ObsoleteEx(ReplacementTypeOrMember = "ThemeColorBrush", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
    public class AccentColorBrush : ThemeColorBrush
    {
        #region Constructors
        public AccentColorBrush()
            : base()
        {
        }

        public AccentColorBrush(AccentColorStyle accentColorStyle)
            : base()
        {
            AccentColorStyle = accentColorStyle;
        }
        #endregion

        #region Properties
        public AccentColorStyle AccentColorStyle { get; set; }
        #endregion

        #region Methods
        protected override object ProvideDynamicValue(IServiceProvider serviceProvider)
        {
            return ThemeHelper.GetAccentColorBrush(AccentColorStyle);
        }
        #endregion
    }
}
