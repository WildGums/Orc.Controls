#nullable disable
namespace Orc.Controls.Automation;

using System;
using System.Linq;
using Orc.Automation;

public class ColorLegendAutomationPeer : AutomationControlPeerBase<Orc.Controls.ColorLegend>
{
    public ColorLegendAutomationPeer(Orc.Controls.ColorLegend owner)
        : base(owner)
    {
        owner.SelectionChanged += OwnerOnSelectionChanged;
    }

    private void OwnerOnSelectionChanged(object sender, EventArgs e)
    {
        RaiseEvent(nameof(ColorLegend.SelectionChanged), 10);
    }

    [AutomationMethod]
    public void SetItemCheckState(int index, bool isChecked)
    {
        var item = Control.ItemsSource.ElementAt(index);

        item.IsChecked = isChecked;
    }
}
#nullable enable
