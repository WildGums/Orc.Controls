namespace Orc.Controls.Automation
{
    using System;
    using System.Collections.Generic;
    using Catel;

    public class ColorLegendView
    {
        private readonly ColorLegendMap _map;

        public ColorLegendView(ColorLegendMap map)
        {
            Argument.IsNotNull(() => map);

            _map = map;
        }

        public bool? IsAllChecked
        {
            get => _map.AllVisibleCheckBoxPart.IsChecked;
            set => _map.AllVisibleCheckBoxPart.IsChecked = value;
        }

        public bool IsVisibilityColumnVisible
        {
            get => InvokeInSettingsScope(x => x.IsVisibilityColumnVisible);
            set => InvokeInSettingsScope(x => x.IsVisibilityColumnVisible = value);
        }

        public bool IsColorEditAllowed
        {
            get => InvokeInSettingsScope(x => x.IsColorEditAllowed);
            set => InvokeInSettingsScope(x => x.IsColorEditAllowed = value);
        }

        public bool IsColorsVisible
        {
            get => InvokeInSettingsScope(x => x.IsColorsVisible);
            set => InvokeInSettingsScope(x => x.IsColorsVisible = value);
        }

        public bool CanClearSelection => _map.UnselectAllButtonPart.IsEnabled;

        public List<ColorLegendItemAutomationControl> Items => _map.Items;

        public TResult InvokeInSettingsScope<TResult>(Func<ColorLegendSettingsControl, TResult> action)
        {
            return _map.SettingsButtonPart.InvokeInDropDownScope(menu =>
                Equals(menu.AutomationProperties.AutomationId, "ColorLegendSettingsContextMenu") 
                    ? action.Invoke(new ColorLegendSettingsControl(menu.Element)) 
                    : default);
        }

        public void SetFilter(string filter)
        {
            _map.SearchBoxPart.Text = filter;
        }
    }
}
