namespace Orc.Controls.Tests.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Windows.Media;
    using Automation;
    using NUnit.Framework;
    using Orc.Automation;
    using Orc.Automation.Tests;

    [Explicit]
    [TestFixture(TestOf = typeof(ColorLegend))]
    [Category("UI Tests")]
    public partial class ColorLegendTestFacts : StyledControlTestFacts<Orc.Controls.ColorLegend>
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

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var model = Target.Current;
            model.ItemsSource = OriginalItemSource;
        }

        [Test]
        public void VerifyConnectedProperties()
        {
            var target = Target;
            var model = target.Current;


            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(model, nameof(model.ShowSearchBox), target, nameof(target.IsSearchBoxVisible), true, false);

            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(model, nameof(model.AllowColorEditing), target, nameof(target.AllowColorEditing), true, false);
            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(model, nameof(model.ShowColorPicker), target, nameof(target.ShowColorPicker), true, false);
            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(model, nameof(model.ShowColorVisibilityControls), target, nameof(target.ShowColorVisibilityControls), true, false);
            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(model, nameof(model.ShowToolBox), target, nameof(target.IsToolBoxVisible), true, false);
            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(model, nameof(model.ShowBottomToolBox), target, nameof(target.IsBottomToolBoxVisible), true, false);
            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(model, nameof(model.ShowSettingsBox), target, nameof(target.IsSettingsBoxVisible), true, false);
            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(model, nameof(model.IsAllVisible), target, nameof(target.IsAllVisible), true, false);
           // ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(target, nameof(target.IsColorSelecting), view, nameof(view.IsColorSelecting), true, false);

            ConnectedPropertiesAssert.VerifyConnectedProperties(target, nameof(target.Filter), model, nameof(model.Filter),
                true,
                new ValueTuple<object, object>("test string", "test string"),
              //  new ValueTuple<object, object>(null, null),
              //  new ValueTuple<object, object>(string.Empty, string.Empty),
                new ValueTuple<object, object>("\\Items+ddd", "\\Items+ddd"));

            ConnectedPropertiesAssert.VerifyIdenticalConnectedProperties(model, nameof(model.FilterWatermark), target, nameof(target.FilterWaterMark), true,
                "Find...",
                "Go and find me...",
                string.Empty,
                null);
        }

        [Test]
        public void CorrectlyInitializeColorLegend()
        {
            var target = Target;
            var model = target.Current;

            Thread.Sleep(1000);

            target.ShowColorPicker = false;

            Thread.Sleep(1000);

            ColorLegendAssert.AllCheckedState(target);
            ColorLegendAssert.ItemsCheckStateMatch(target);
            ColorLegendAssert.ItemsSelectedStateMatch(target);
            ColorLegendAssert.ClearSelectionState(target);
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
