namespace Orc.Controls.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;


   // public class ColorLegend


    public class ColorLegendAutomationControl : RunMethodAutomationControl
    {
        public ColorLegendAutomationControl(AutomationElement element)
            : base(element)
        {
        }

        public bool AllowColorEditing
        {
            get => (bool)GetApiPropertyValue(nameof(ColorLegend.AllowColorEditing));
            set => SetApiPropertyValue(nameof(ColorLegend.AllowColorEditing), value);
        }

        public bool ShowSearchBox
        {
            get => (bool)GetApiPropertyValue(nameof(ColorLegend.ShowSearchBox));
            set => SetApiPropertyValue(nameof(ColorLegend.ShowSearchBox), value);
        }

        public bool ShowToolBox
        {
            get => (bool)GetApiPropertyValue(nameof(ColorLegend.ShowToolBox));
            set => SetApiPropertyValue(nameof(ColorLegend.ShowToolBox), value);
        }

        public bool ShowBottomToolBox
        {
            get => (bool)GetApiPropertyValue(nameof(ColorLegend.ShowBottomToolBox));
            set => SetApiPropertyValue(nameof(ColorLegend.ShowBottomToolBox), value);
        }

        public bool ShowSettingsBox
        {
            get => (bool)GetApiPropertyValue(nameof(ColorLegend.ShowSettingsBox));
            set => SetApiPropertyValue(nameof(ColorLegend.ShowSettingsBox), value);
        }

        public bool ShowColorVisibilityControls
        {
            get => (bool)GetApiPropertyValue(nameof(ColorLegend.ShowColorVisibilityControls));
            set => SetApiPropertyValue(nameof(ColorLegend.ShowColorVisibilityControls), value);
        }

        public bool ShowColorPicker
        {
            get => (bool)GetApiPropertyValue(nameof(ColorLegend.ShowColorPicker));
            set => SetApiPropertyValue(nameof(ColorLegend.ShowColorPicker), value);
        }

        public bool IsColorSelecting
        {
            get => (bool)GetApiPropertyValue(nameof(ColorLegend.IsColorSelecting));
            set => SetApiPropertyValue(nameof(ColorLegend.IsColorSelecting), value);
        }

        public Color EditingColor
        {
            get => (Color)GetApiPropertyValue(nameof(ColorLegend.EditingColor));
            set => SetApiPropertyValue(nameof(ColorLegend.EditingColor), value);
        }

        public string Filter
        {
            get => (string)GetApiPropertyValue(nameof(ColorLegend.Filter));
            set => SetApiPropertyValue(nameof(ColorLegend.Filter), value);
        }

        public IEnumerable<IColorLegendItem> ItemsSource
        {
            get => GetApiPropertyValue(nameof(ColorLegend.ItemsSource)) as IEnumerable<IColorLegendItem>;
            set => SetApiPropertyValue(nameof(ColorLegend.ItemsSource), value);
        }

        public bool? IsAllVisible
        {
            get => (bool)GetApiPropertyValue(nameof(ColorLegend.IsAllVisible));
            set => SetApiPropertyValue(nameof(ColorLegend.IsAllVisible), value);
        }

        public IEnumerable<IColorLegendItem> FilteredItemsSource
        {
            get => GetApiPropertyValue(nameof(ColorLegend.FilteredItemsSource)) as IEnumerable<IColorLegendItem>;
            set => SetApiPropertyValue(nameof(ColorLegend.FilteredItemsSource), value);
        }

        public IEnumerable<string> FilteredItemsIds
        {
            get => GetValue(nameof(ColorLegend.FilteredItemsIds)) as IEnumerable<string>;
        }

        public string FilterWatermark
        {
            get => (string)GetApiPropertyValue(nameof(ColorLegend.FilterWatermark));
            set => SetApiPropertyValue(nameof(ColorLegend.FilterWatermark), value);
        }

        public IEnumerable<IColorLegendItem> SelectedColorItems
        {
            get => GetApiPropertyValue(nameof(ColorLegend.SelectedColorItems)) as IEnumerable<IColorLegendItem>;
            set => SetApiPropertyValue(nameof(ColorLegend.SelectedColorItems), value);
        }

        public void ChangeItemState(int index)
        {
            Execute(nameof(ChangeItemState), index);
        }

        protected override void OnEvent(string eventName, object eventData)
        {
            if (Equals(eventName, nameof(ColorLegend.SelectionChanged)))
            {
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> SelectionChanged;
    }
}
