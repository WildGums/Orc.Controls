#nullable disable
namespace Orc.Controls.Automation;

using System;
using System.Collections.Generic;
using System.Windows.Automation;
using Orc.Automation;
using Orc.Automation.Controls;

[AutomatedControl(Class = typeof(Controls.ColorLegend))]
public class ColorLegend : FrameworkElement<ColorLegendModel, ColorLegendMap>
{
    public ColorLegend(AutomationElement element)
        : base(element)
    {
    }
        
    public bool? IsAllVisible
    {
        get => Map.AllVisibleCheckBoxPart.IsChecked;
        set => Map.AllVisibleCheckBoxPart.IsChecked = value;
    }

    public bool ShowColorVisibilityControls
    {
        get => InvokeInSettingsScope(x => x.ShowColorVisibilityControls);
        set => InvokeInSettingsScope(x => x.ShowColorVisibilityControls = value);
    }

    public bool AllowColorEditing
    {
        get => InvokeInSettingsScope(x => x.AllowColorEditing);
        set => InvokeInSettingsScope(x => x.AllowColorEditing = value);
    }

    public bool ShowColorPicker
    {
        get => InvokeInSettingsScope(x => x.ShowColorPicker);
        set => InvokeInSettingsScope(x => x.ShowColorPicker = value);
    }

    public string Filter
    {
        get => Map.SearchBoxPart.Text;
        set => Map.SearchBoxPart.Text = value;
    }

    public string FilterWaterMark
    {
        get => Map.FilterWatermarkTextPart?.Value;
    }

    public bool IsSettingsBoxVisible => Map.SettingsButtonPart.IsVisible();
    public bool IsToolBoxVisible => Map.SettingsButtonPart.IsVisible() || Map.SearchBoxPart.IsVisible();
    public bool IsBottomToolBoxVisible => Map.AllVisibleCheckBoxPart.IsVisible() || Map.SelectedItemCountLabel.IsVisible() || Map.UnselectAllButtonPart.IsVisible();
    public bool IsSearchBoxVisible => Map.SearchBoxPart.IsVisible();
    public bool CanClearSelection => Map.UnselectAllButtonPart.IsEnabled;
    public List<ColorLegendItemAutomationControl> Items => Map.Items;

    public TResult InvokeInSettingsScope<TResult>(Func<ColorLegendSettingsControl, TResult> action)
    {
        return Map.SettingsButtonPart.InvokeInDropDownScope(menu =>
            Equals(menu.AutomationProperties.AutomationId, "ColorLegendSettingsContextMenu")
                ? action.Invoke(new ColorLegendSettingsControl(menu.Element))
                : default);
    }

#pragma warning disable CS0067
    public event EventHandler<EventArgs> SelectionChanged;
#pragma warning restore CS0067

    public void SetItemCheckState(int index, bool state)
    {
        Access.Execute(nameof(ColorLegendAutomationPeer.SetItemCheckState), index, state);
    }
}
#nullable enable
