namespace Orc.Controls.Theming
{
    using System.Collections.Generic;
    using ControlzEx.Theming;

    public class LibraryThemeProvider : ControlzEx.Theming.LibraryThemeProvider
    {
        public static readonly ControlzEx.Theming.LibraryThemeProvider DefaultInstance = new LibraryThemeProvider();

        public LibraryThemeProvider()
            : base(true)
        {
        }

        public override void FillColorSchemeValues(Dictionary<string, string> values, RuntimeThemeColorValues colorValues)
        {
            // Orc / Orchestra colors
            values.Add("Orc.Colors.AccentBaseColor", colorValues.AccentBaseColor.ToString());
            values.Add("Orc.Colors.AccentColor", colorValues.AccentBaseColor.ToString());
            values.Add("Orc.Colors.AccentColor1", colorValues.AccentColor80.ToString());
            values.Add("Orc.Colors.AccentColor2", colorValues.AccentColor60.ToString());
            values.Add("Orc.Colors.AccentColor3", colorValues.AccentColor40.ToString());
            values.Add("Orc.Colors.AccentColor4", colorValues.AccentColor20.ToString());
            values.Add("Orc.Colors.AccentColor5", colorValues.AccentColor20.ToString());

            values.Add("Orc.Colors.AccentColor80", colorValues.AccentColor80.ToString());
            values.Add("Orc.Colors.AccentColor60", colorValues.AccentColor60.ToString());
            values.Add("Orc.Colors.AccentColor40", colorValues.AccentColor40.ToString());
            values.Add("Orc.Colors.AccentColor20", colorValues.AccentColor20.ToString());

            values.Add("Orc.Colors.HighlightColor", colorValues.HighlightColor.ToString());
            values.Add("Orc.Colors.IdealForegroundColor", colorValues.IdealForegroundColor.ToString());

            // ControlzEx colors
            values.Add("ControlzEx.Colors.AccentBaseColor", colorValues.AccentBaseColor.ToString());
            values.Add("ControlzEx.Colors.AccentColor80", colorValues.AccentColor80.ToString());
            values.Add("ControlzEx.Colors.AccentColor60", colorValues.AccentColor60.ToString());
            values.Add("ControlzEx.Colors.AccentColor40", colorValues.AccentColor40.ToString());
            values.Add("ControlzEx.Colors.AccentColor20", colorValues.AccentColor20.ToString());

            values.Add("ControlzEx.Colors.HighlightColor", colorValues.HighlightColor.ToString());
            values.Add("ControlzEx.Colors.IdealForegroundColor", colorValues.IdealForegroundColor.ToString());
        }
    }
}
