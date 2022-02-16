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
            var view = View;

            const int itemIndex = 2;

            EventAssert.Raised(target, nameof(target.SelectionChanged), () => view.Items[itemIndex].Select());
        }
    }
}
