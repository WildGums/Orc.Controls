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
    public class ColorLegendTestFacts : ControlUiTestFactsBase<ColorLegend>
    {
        [TestTarget]
        public ColorLegendAutomationElement Target { get; set; }

        [ControlPart(ControlType = nameof(ControlType.Text))]
        public AutomationElement SearchBoxPart { get; set; }

        [ControlPart(AutomationId = "SettingsButton")]
        public AutomationElement SettingsButtonPart { get; set; }

        [ControlPart(AutomationId = "AllVisibleCheckBox")]
        public AutomationElement AllVisibleCheckBoxPart { get; set; }

        [ControlPart(AutomationId = "UnselectAllButton")]
        public AutomationElement UnselectAllButtonPart { get; set; }

        [SetUp]
        public override void SetUpTest()
        {
            base.SetUpTest();

            Target.ItemsSource = new List<Color>
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
        }

        [TestCase(true, true, true, true)]
        public void CheckInitialState(bool expectedShowSearchBox, bool expectedTShowToolBox, bool expectedShowBottomToolBox,
            bool expectedShowColorVisibilityControls)
        {
            var colorLegend = Target;

            //Check Search Box
            Assert.AreEqual(colorLegend.ShowSearchBox, expectedShowSearchBox);
            Assert.AreEqual(SearchBoxPart?.IsVisible(), expectedShowSearchBox && expectedTShowToolBox);

            //Check ToolBox
            Assert.AreEqual(colorLegend.ShowToolBox, expectedTShowToolBox);
            Assert.AreEqual(SettingsButtonPart?.IsVisible(), expectedTShowToolBox);

            //Check Bottom ToolBox
            Assert.AreEqual(colorLegend.ShowBottomToolBox, expectedShowBottomToolBox);
            Assert.AreEqual(AllVisibleCheckBoxPart?.IsVisible(), expectedShowBottomToolBox && expectedShowColorVisibilityControls);
            Assert.AreEqual(UnselectAllButtonPart?.IsVisible(), expectedShowBottomToolBox);
        }
    }
}
