#nullable disable
namespace Orc.Controls.Automation;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Orc.Automation;

public class ColorLegendModel : FrameworkElementModel
{
    public ColorLegendModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    [ActiveAutomationProperty]
    public bool AllowColorEditing { get; set; }

    [ActiveAutomationProperty]
    public bool ShowColorVisibilityControls { get; set; }

    [ActiveAutomationProperty]
    public bool ShowColorPicker { get; set; }

    [ActiveAutomationProperty]
    public bool ShowSearchBox { get; set; }

    [ActiveAutomationProperty]
    public bool ShowToolBox { get; set; }

    [ActiveAutomationProperty]
    public bool ShowBottomToolBox { get; set; }

    [ActiveAutomationProperty]
    public bool ShowSettingsBox { get; set; }

    [ActiveAutomationProperty]
    public bool IsColorSelecting { get; set; }

    [ActiveAutomationProperty]
    public Color EditingColor { get; set; }

    [ActiveAutomationProperty]
    public string Filter { get; set; }

    [ActiveAutomationProperty]
    public IEnumerable<IColorLegendItem> ItemsSource { get; set; }

    [ActiveAutomationProperty]
    public bool? IsAllVisible { get; set; }

    [ActiveAutomationProperty]
    public IEnumerable<IColorLegendItem> FilteredItemsSource { get; set; }

    public IEnumerable<string> FilteredItemsIds
    {
        get => _accessor.GetValue<IEnumerable<string>>();
    }

    [ActiveAutomationProperty]
    public string FilterWatermark { get; set; }

    [ActiveAutomationProperty]
    public IEnumerable<IColorLegendItem> SelectedColorItems { get; set; }

    public IColorLegendItem this[int index]
    {
        get => ItemsSource.ToList()[index];
    }
}
#nullable enable
