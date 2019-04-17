// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccentColor.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;

    [ObsoleteEx(ReplacementTypeOrMember = "ThemeColor", TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0")]
    public class AccentColor : ThemeColor
    {
        public AccentColor()
            : base()
        {
        }

        public AccentColor(AccentColorStyle accentColorStyle)
            : this()
        {
        }

        #region Properties
        public AccentColorStyle AccentColorStyle { get; set; }
        #endregion

        #region Methods
        protected override object ProvideDynamicValue(IServiceProvider serviceProvider)
        {
            return ThemeHelper.GetAccentColor(AccentColorStyle);
        }
        #endregion
    }
}
