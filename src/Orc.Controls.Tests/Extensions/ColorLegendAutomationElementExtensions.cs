namespace Orc.Controls.Tests
{
    using System.Linq;
    using Automation;

    public static class ColorLegendAutomationElementExtensions
    {
        public static IColorLegendItem GetItemSourceItem(this ColorLegendAutomationControl target, int index)
            => target.ItemsSource.ToList()[index];
    }
}
