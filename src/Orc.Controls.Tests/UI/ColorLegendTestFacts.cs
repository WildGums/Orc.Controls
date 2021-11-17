namespace Orc.Controls.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Windows.Automation;
    using System.Windows.Media;
    using Automation;
    using Automation.Tests;
    using Catel.IoC;
    using Catel.Runtime.Serialization;
    using NUnit.Framework;

    [Explicit, TestFixture]
    public class ColorLegendTestFacts : ControlUiTestFactsBase<ColorLegend>
    {
        private CommandAutomationElement Target { get; set; }

        private AutomationElement SearchBoxPart => Target.Element.Find(controlType: ControlType.Text, numberOfWaits: 3);
        private AutomationElement SettingsButton => Target.Element.Find(id: nameof(SettingsButton), numberOfWaits: 3);
        private AutomationElement AllVisibleCheckBox => Target.Element.Find(id: nameof(AllVisibleCheckBox), numberOfWaits: 3);
        private AutomationElement UnselectAllButton => Target.Element.Find(id: nameof(UnselectAllButton), numberOfWaits: 3);
        
        [SetUp]
        public override void SetUpTest()
        {
            base.SetUpTest();

            var targetControl = TestModel.TargetControl;

            Target = new CommandAutomationElement(targetControl);

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

            Target.SetValue(nameof(ColorLegend.ItemsSource), colorLegendItems);
        }

        [TestCase(true, true, true, true)]
        public void CheckInitialState(bool expectedShowSearchBox, bool expectedTShowToolBox, bool expectedShowBottomToolBox,
            bool expectedShowColorVisibilityControls)
        {
            var colorLegend = Target;

            //Check Search Box
            Assert.AreEqual(colorLegend.GetValue(nameof(ColorLegend.ShowSearchBox)), expectedShowSearchBox);
            Assert.AreEqual(SearchBoxPart?.IsVisible(), expectedShowSearchBox && expectedTShowToolBox);

            //Check ToolBox
            Assert.AreEqual(colorLegend.GetValue(nameof(ColorLegend.ShowToolBox)), expectedTShowToolBox);
            Assert.AreEqual(SettingsButton?.IsVisible(), expectedTShowToolBox);

            //Check Bottom ToolBox
            Assert.AreEqual(colorLegend.GetValue(nameof(ColorLegend.ShowBottomToolBox)), expectedShowBottomToolBox);
            Assert.AreEqual(AllVisibleCheckBox?.IsVisible(), expectedShowBottomToolBox && expectedShowColorVisibilityControls);
            Assert.AreEqual(UnselectAllButton?.IsVisible(), expectedShowBottomToolBox);
        }
    }
}
