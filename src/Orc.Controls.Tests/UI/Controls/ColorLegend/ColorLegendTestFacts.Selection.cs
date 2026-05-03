namespace Orc.Controls.Tests.UI;

using System;
using System.Linq;
using NUnit.Framework;
using Orc.Automation.Tests;

public partial class ColorLegendTestFacts
{
    [Test]
    public void CorrectlySelectItem()
    {
        var target = Target;

        const int itemIndex = 2;

        var selectedItem = target.Items[itemIndex];

        EventAssert.Raised(target, nameof(target.SelectionChanged), () => selectedItem.Select());

        var model = target.Current;

        var selectedSourceItems = (model.ItemsSource ?? Array.Empty<IColorLegendItem>())
            .Where(x => x.IsSelected)
            .ToArray();

        Assert.That(selectedSourceItems, Has.Length.EqualTo(1));

        var selectedItemData = selectedSourceItems.Single();
        Assert.That(selectedItem.Description, Is.EqualTo(selectedItemData.Description));
    }
}
