namespace Orc.Controls.Controls.StepBar.Models
{
    using System;

    public static class WizardConfiguration
    {
        public static TimeSpan AnimationDuration { get; set; } = TimeSpan.FromMilliseconds(300);

        public static readonly int CannotNavigate = -1;
    }

    public static class ThemingKeys
    {
        public const string AccentColorBrush = "Orc.Brushes.AccentColorBrush";
        public const string AccentColorBrush40 = "Orc.Brushes.AccentColorBrush40";
        public const string AccentColor = "Orc.Colors.AccentColor";
        public const string AccentColor40 = "Orc.Colors.AccentColor40";
    }
}
