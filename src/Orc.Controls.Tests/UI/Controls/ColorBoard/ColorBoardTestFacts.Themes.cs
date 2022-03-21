namespace Orc.Controls.Tests.UI
{
    using Automation;
    using NUnit.Framework;
    using Orc.Automation.Tests;
    
    public partial class ColorBoardTestFacts
    {
        [Test]
        public void VerifyThemes()
        {
            var target = Target;
            var map = target.Map<ColorBoardMap>();

            ButtonThemeAssert.VerifyTheme(map.SelectButton);
            ButtonThemeAssert.VerifyTheme(map.CancelButton);

            ListBoxThemeAssert.VerifyTheme(map.ThemeColorsListBox);
            ListBoxThemeAssert.VerifyTheme(map.RecentColorsListBox);
        }
    }  
}
