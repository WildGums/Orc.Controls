namespace Orc.Controls.Automation
{
    using System.Collections.Generic;
    using Catel;
    using Orc.Automation;

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
            get => _map.AllVisibleCheckBoxPart.GetToggleState();
            set => _map.AllVisibleCheckBoxPart.TrySetToggleState(value);
        }

        public List<ColorLegendItemAutomationControl> Items => _map.Items;
    }
}
