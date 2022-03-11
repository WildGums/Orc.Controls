namespace Orc.Controls.Tests
{
    using NUnit.Framework;
    using Orc.Automation.Tests;

    public partial class ColorLegendTestFacts
    {
        [Test]
        public void CorrectlySelectItem()
        {
            var target = Target;

            const int itemIndex = 2;

            EventAssert.Raised(target, nameof(target.SelectionChanged), () => target.Items[itemIndex].Select());
        }
    }
}
