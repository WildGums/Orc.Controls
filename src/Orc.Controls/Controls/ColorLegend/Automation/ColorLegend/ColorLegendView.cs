namespace Orc.Controls.Automation
{
    using System;
    using System.Collections.Generic;
    using Catel;
    using Orc.Automation;
    using Orc.Automation.Attributes;

    public class ColorLegendView
    {
        private readonly ColorLegendMap _map;

        public ColorLegendView(ColorLegendMap map)
        {
            Argument.IsNotNull(() => map);

            _map = map;
        }

     //   public bool IsColo
        
        public bool? IsAllVisible
        {
            get => _map.AllVisibleCheckBoxPart.IsChecked;
            set => _map.AllVisibleCheckBoxPart.IsChecked = value;
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
            get => _map.SearchBoxPart.Text;
            set => _map.SearchBoxPart.Text = value;
        }

        public string FilterWaterMark
        {
            get => _map.FilterWatermarkTextPart?.Value;
        }

        public bool IsSettingsBoxVisible => _map.SettingsButtonPart.IsVisible();
        public bool IsToolBoxVisible => _map.SettingsButtonPart.IsVisible() || _map.SearchBoxPart.IsVisible();
        public bool IsBottomToolBoxVisible => _map.AllVisibleCheckBoxPart.IsVisible() || _map.SelectedItemCountLabel.IsVisible() || _map.UnselectAllButtonPart.IsVisible();

        public bool IsSearchBoxVisible => _map.SearchBoxPart.IsVisible();

        public bool CanClearSelection => _map.UnselectAllButtonPart.IsEnabled;

        public List<ColorLegendItemAutomationControl> Items => _map.Items;

        public TResult InvokeInSettingsScope<TResult>(Func<ColorLegendSettingsControl, TResult> action)
        {
            return _map.SettingsButtonPart.InvokeInDropDownScope(menu =>
                Equals(menu.AutomationProperties.AutomationId, "ColorLegendSettingsContextMenu") 
                    ? action.Invoke(new ColorLegendSettingsControl(menu.Element))
                    : default);
        }
    }
}
