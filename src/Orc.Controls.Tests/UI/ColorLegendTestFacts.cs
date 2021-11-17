namespace Orc.Controls.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using System.Windows.Media;
    using Automation;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [Explicit, TestFixture]
    public class ColorLegendTestFacts : ControlUiTestFactsBase<ColorLegend, ColorLegendAutomationElement>
    {


        [SetUp]
        public override void SetUpTest()
        {
            base.SetUpTest();

            var targetControl = TestModel.TargetControl;

            var colorLegendItems = new List<Color>
                {
                    Colors.Blue,
                    Colors.Red, 
                    Colors.Cyan, 
                    Colors.DarkGreen,
                    Colors.Indigo,
                    Colors.Orange
                }
                .Select((x, i) => new ColorLegendItem
                {
                    Description = $"Item {i}", 
                    Color = x,
                    Id = i.ToString(), 
                    IsChecked = true
                })
                .ToList();

            targetControl.SetValue(nameof(ColorLegend.ItemsSource), colorLegendItems);
        }

        [TestCase(true, true, true, true)]
        public void CheckInitialState(bool expectedShowSearchBox, bool expectedTShowToolBox, bool expectedShowBottomToolBox,
            bool expectedShowColorVisibilityControls)
        {
            var colorLegend = TestModel.TargetControl;

            //Check Search Box
            Assert.AreEqual(colorLegend.GetValue(nameof(ColorLegend.ShowSearchBox)), expectedShowSearchBox);
            Assert.AreEqual(colorLegend.SearchBox?.IsVisible(), expectedShowSearchBox && expectedTShowToolBox);

            //Check ToolBox
            Assert.AreEqual(colorLegend.GetValue(nameof(ColorLegend.ShowToolBox)), expectedTShowToolBox);
            Assert.AreEqual(colorLegend.SettingsButton?.IsVisible(), expectedTShowToolBox);

            //Check Bottom ToolBox
            Assert.AreEqual(colorLegend.GetValue(nameof(ColorLegend.ShowBottomToolBox)), expectedShowBottomToolBox);
            Assert.AreEqual(colorLegend.AllVisibleCheckBox?.IsVisible(), expectedShowBottomToolBox && expectedShowColorVisibilityControls);
            Assert.AreEqual(colorLegend.UnselectAllButton?.IsVisible(), expectedShowBottomToolBox);
        }
    }
}
