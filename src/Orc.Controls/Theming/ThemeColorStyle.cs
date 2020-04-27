// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    public enum ThemeColorStyle
    {
        AccentColor = 0,
        AccentColor80 = 1,
        AccentColor60 = 2,
        AccentColor40 = 3,
        AccentColor20 = 4,

        // Backwards compatibility
        AccentColor1 = AccentColor80,
        AccentColor2 = AccentColor60,
        AccentColor3 = AccentColor40,
        AccentColor4 = AccentColor20,
        AccentColor5 = 5, // Kept for backwards compatibility, should not be used

        BorderColor = 6,
        BorderColor80 = 7,
        BorderColor60 = 8,
        BorderColor40 = 9,
        BorderColor20 = 10,

        // Backwards compatibility
        BorderColor1 = BorderColor80,
        BorderColor2 = BorderColor60,
        BorderColor3 = BorderColor40,
        BorderColor4 = BorderColor20,
        BorderColor5 = 11, // Kept for backwards compatibility, should not be used

        BackgroundColor = 12,

        ForegroundColor = 13,
        ForegroundAlternativeColor = 14,

        // Aliases - highlights
        DarkHighlight = AccentColor3,
        Highlight = AccentColor4,

        // Aliases - borders
        BorderLight = BorderColor3,
        BorderMedium = BorderColor2,
        BorderDark = BorderColor1,
        BorderMouseOver = AccentColor1,
        BorderPressed = AccentColor,
        BorderChecked = AccentColor,
        BorderSelected = AccentColor,
        BorderSelectedInactive = AccentColor2,
        BorderDisabled = BorderColor5,

        // Aliases - backgrounds
        BackgroundMouseOver = AccentColor4,
        BackgroundPressed = AccentColor3,
        BackgroundChecked = AccentColor,
        BackgroundSelected = AccentColor3,
        BackgroundSelectedInactive = AccentColor4,
        BackgroundDisabled = AccentColor5,

        // Aliases - foregrounds
        ForegroundMouseOver = ForegroundColor,
        ForegroundPressed = ForegroundAlternativeColor,
        ForegroundChecked = ForegroundAlternativeColor,
        ForegroundSelected = ForegroundAlternativeColor,
        ForegroundSelectedInactive = ForegroundAlternativeColor,
        ForegroundDisabled = ForegroundColor
    }
}
