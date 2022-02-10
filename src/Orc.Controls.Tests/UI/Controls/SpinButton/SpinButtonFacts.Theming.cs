namespace Orc.Controls.Tests
{
    using Automation;
    using NUnit.Framework;
    using Orc.Automation.Tests;

    public partial class SpinButtonFacts
    {
        [Test]
        public void VerifyTheme()
        {
            var map = Target.Map<SpinButtonMap>();

            ButtonThemeAssert.VerifyTheme(map.IncreaseButton);
            ButtonThemeAssert.VerifyTheme(map.DecreaseButton);
        }
    }
}
