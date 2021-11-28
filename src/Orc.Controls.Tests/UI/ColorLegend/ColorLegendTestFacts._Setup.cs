namespace Orc.Controls.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;
    using Automation;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [TestFixture(TestOf = typeof(ColorLegend))]
    [Category("UI Tests")]
    public partial class ColorLegendTestFacts : ControlUiTestFactsBase<ColorLegend>
    {
        private static readonly List<IColorLegendItem> OriginalItemSource = new List<Color>
        {
            Colors.Blue,
            Colors.Red,
            Colors.Cyan,
            Colors.DarkGreen,
            Colors.Indigo,
            Colors.Orange,
            Colors.Aqua,
            Colors.DarkOliveGreen,
            Colors.Yellow,
            Colors.SeaGreen,
        }
        .Select((x, i) => (IColorLegendItem)new ColorLegendItem
        {
            Id = (i + 1).ToString(),
            IsChecked = true,
            Description = $"Item {i + 1}",
            Color = x,
            AdditionalData = "1"
        })
        .ToList();

        [Target]
        public ColorLegendAutomationControl Target { get; set; }

        public ColorLegendAutomationElementMap TargetMap => Target.CreateMap<ColorLegendAutomationElementMap>();

        [SetUp]
        public override void SetUpTest()
        {
            base.SetUpTest();

            Target.ItemsSource = OriginalItemSource;
        }

        [TestCase(true, true, true, true)]
        public void CorrectlyInitializeColorLegend(bool expectedShowSearchBox, bool expectedTShowToolBox, bool expectedShowBottomToolBox,
            bool expectedShowColorVisibilityControls)
        {
            //var target = Target;
            //var map = TargetMap;

            ////Check Search Box
            //Assert.AreEqual(target.ShowSearchBox, expectedShowSearchBox);
            //Assert.AreEqual(map.SearchBoxPart?.IsVisible(), expectedShowSearchBox && expectedTShowToolBox);

            ////Check ToolBox
            //Assert.AreEqual(target.ShowToolBox, expectedTShowToolBox);
            //Assert.AreEqual(map.SettingsButtonPart?.IsVisible(), expectedTShowToolBox);

            ////Check Bottom ToolBox
            //Assert.AreEqual(target.ShowBottomToolBox, expectedShowBottomToolBox);
            //Assert.AreEqual(map.AllVisibleCheckBoxPart?.IsVisible(), expectedShowBottomToolBox && expectedShowColorVisibilityControls);
            //Assert.AreEqual(map.UnselectAllButtonPart?.IsVisible(), expectedShowBottomToolBox);
        }

        private class TestColorItemComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return string.CompareOrdinal((x as IColorLegendItem)?.Id, (y as IColorLegendItem)?.Id);
            }
        }
    }
}
