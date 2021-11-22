namespace Orc.Controls.Tests
{
    using System.Linq;
    using Automation;

    public static class ColorLegendAutomationElementExtensions
    {
        public static IColorLegendItem GetItemSourceItem(this ColorLegendAutomationElement target, int index)
            => target.ItemsSource.ToList()[index];
    }
}
