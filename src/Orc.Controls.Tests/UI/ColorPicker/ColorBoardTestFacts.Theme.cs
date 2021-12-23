namespace Orc.Controls.Tests
{
    using Automation;
    using NUnit.Framework;
    using Orc.Automation.Tests.StyleAsserters;

    public partial class ColorBoardTestFacts
    {
        [Test]
        public void VerifyMapStyles()
        {
            var target = Target;
            var map = target.Map<ColorBoardMap>();

            var selectButton = map.SelectButton;

            FrameworkElementAssert.BorderColor(selectButton);
            FrameworkElementAssert.Background(selectButton);
            FrameworkElementAssert.MouseOverBackground(selectButton);
        }
    }
}
