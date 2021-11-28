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

        [ApiProperty]
        public bool AllowColorEditing { get; set; }

        [ApiProperty]
        public bool ShowSearchBox { get; set; }

        [ApiProperty]
        public bool ShowToolBox { get; set; }

        [ApiProperty]
        public bool ShowBottomToolBox { get; set; }

        [ApiProperty]
        public bool ShowSettingsBox { get; set; }

        [ApiProperty]
        public bool ShowColorVisibilityControls { get; set; }

        [ApiProperty]
        public bool ShowColorPicker { get; set; }

        [ApiProperty]
        public bool IsColorSelecting { get; set; }

        [ApiProperty]
        public Color EditingColor { get; set; }

        [ApiProperty]
        public string Filter { get; set; }

        [ApiProperty]
        public IEnumerable<IColorLegendItem> ItemsSource { get; set; }

        [ApiProperty]
        public bool? IsAllVisible { get; set; }

        [ApiProperty]
        public IEnumerable<IColorLegendItem> FilteredItemsSource { get; set; }

        [ApiProperty]
        public IEnumerable<string> FilteredItemsIds { get; }

        [ApiProperty]
        public string FilterWatermark { get; set; }

        [ApiProperty]
        public IEnumerable<IColorLegendItem> SelectedColorItems { get; set; }

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
