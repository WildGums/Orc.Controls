namespace Orc.Controls.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Automation;
    using Orc.Automation;

    public class ColorLegendAutomationElement : CommandAutomationElement
    {
        public ColorLegendAutomationElement(AutomationElement element) 
            : base(element)
        {
        }

        public IEnumerable<IColorLegendItem> ItemsSource
        {
            get => GetValue(nameof(ColorLegend.ItemsSource)) as IEnumerable<IColorLegendItem>;
            set => SetValue(nameof(ColorLegend.ItemsSource), value);
        }

        public bool ShowSearchBox
        {
            get => (bool)GetValue(nameof(ColorLegend.ShowSearchBox));
            set => SetValue(nameof(ColorLegend.ShowSearchBox), value);
        }

        public bool ShowToolBox
        {
            get => (bool)GetValue(nameof(ColorLegend.ShowToolBox));
            set => SetValue(nameof(ColorLegend.ShowToolBox), value);
        }
        public bool ShowBottomToolBox
        {
            get => (bool)GetValue(nameof(ColorLegend.ShowBottomToolBox));
            set => SetValue(nameof(ColorLegend.ShowBottomToolBox), value);
        }

        //public virtual AutomationElement SearchBox => GetPart(controlType: ControlType.Text); 
        //public virtual AutomationElement SettingsButton => GetPart();
        //public virtual AutomationElement AllVisibleCheckBox => GetPart();
        //public virtual AutomationElement UnselectAllButton => GetPart();
    }
}
