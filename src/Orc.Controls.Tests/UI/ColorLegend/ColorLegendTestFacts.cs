namespace Orc.Controls.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Windows.Media;
    using Automation;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [TestFixture(TestOf = typeof(ColorLegend))]
    [Category("UI Tests")]
    public partial class ColorLegendTestFacts : ControlUiTestFactsBase<Orc.Controls.ColorLegend>
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
        public ColorLegend Target { get; set; }

        public ColorLegendView View => Target.View;

        [SetUp]
        public override void SetUpTest()
        {
            base.SetUpTest();

            Target.ItemsSource = OriginalItemSource;
        }

        [Test]
        public void CorrectlyInitializeColorLegend()
        {
            var target = Target;
            var view = View;

            var items = view.Items;
            
            var isControlIsColorEditAllowed = view.IsColorEditAllowed;
            var isColorsVisible = view.IsColorsVisible;
            var isVisibilityColumnVisible = view.IsVisibilityColumnVisible;

            Thread.Sleep(1000);

            view.IsColorsVisible = false;

            Thread.Sleep(1000);


            var map = Target.Map<ColorLegendMap>();

            ColorLegendAssert.AllCheckedState(target);
            ColorLegendAssert.ItemsCheckStateMatch(target);
            ColorLegendAssert.ItemsSelectedStateMatch(target);
            ColorLegendAssert.ClearSelectionState(target);

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
