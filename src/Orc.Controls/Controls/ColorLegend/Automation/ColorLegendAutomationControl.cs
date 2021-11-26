namespace Orc.Controls.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;

    public class ColorLegendAutomationControl : RunMethodAutomationControl
    {
        public ColorLegendAutomationControl(AutomationElement element)
            : base(element)
        {
        }

        public bool AllowColorEditing
        {
            get => (bool)GetValue(nameof(ColorLegend.AllowColorEditing));
            set => SetValue(nameof(ColorLegend.AllowColorEditing), value);
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

        public bool ShowSettingsBox
        {
            get => (bool)GetValue(nameof(ColorLegend.ShowSettingsBox));
            set => SetValue(nameof(ColorLegend.ShowSettingsBox), value);
        }

        public bool ShowColorVisibilityControls
        {
            get => (bool)GetValue(nameof(ColorLegend.ShowColorVisibilityControls));
            set => SetValue(nameof(ColorLegend.ShowColorVisibilityControls), value);
        }

        public bool ShowColorPicker
        {
            get => (bool)GetValue(nameof(ColorLegend.ShowColorPicker));
            set => SetValue(nameof(ColorLegend.ShowColorPicker), value);
        }

        public bool IsColorSelecting
        {
            get => (bool)GetValue(nameof(ColorLegend.IsColorSelecting));
            set => SetValue(nameof(ColorLegend.IsColorSelecting), value);
        }

        public Color EditingColor
        {
            get => (Color)GetValue(nameof(ColorLegend.EditingColor));
            set => SetValue(nameof(ColorLegend.EditingColor), value);
        }

        public string Filter
        {
            get => (string)GetValue(nameof(ColorLegend.Filter));
            set => SetValue(nameof(ColorLegend.Filter), value);
        }

        public IEnumerable<IColorLegendItem> ItemsSource
        {
            get => GetValue(nameof(ColorLegend.ItemsSource)) as IEnumerable<IColorLegendItem>;
            set => SetValue(nameof(ColorLegend.ItemsSource), value);
        }

        public bool? IsAllVisible
        {
            get => (bool)GetValue(nameof(ColorLegend.IsAllVisible));
            set => SetValue(nameof(ColorLegend.IsAllVisible), value);
        }

        public IEnumerable<IColorLegendItem> FilteredItemsSource
        {
            get => GetValue(nameof(ColorLegend.FilteredItemsSource)) as IEnumerable<IColorLegendItem>;
            set => SetValue(nameof(ColorLegend.FilteredItemsSource), value);
        }

        public IEnumerable<string> FilteredItemsIds
        {
            get => GetValue(nameof(ColorLegend.FilteredItemsIds)) as IEnumerable<string>;
        }

        public string FilterWatermark
        {
            get => (string)GetValue(nameof(ColorLegend.FilterWatermark));
            set => SetValue(nameof(ColorLegend.FilterWatermark), value);
        }

        public IEnumerable<IColorLegendItem> SelectedColorItems
        {
            get => GetValue(nameof(ColorLegend.SelectedColorItems)) as IEnumerable<IColorLegendItem>;
            set => SetValue(nameof(ColorLegend.SelectedColorItems), value);
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
