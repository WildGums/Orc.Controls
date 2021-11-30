namespace Orc.Controls.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using System.Windows.Media;
    using Orc.Automation;
    using AutomationEventArgs = Orc.Automation.AutomationEventArgs;

    public class ColorLegendAutomationControl : AutomationControl
    {
        public ColorLegendAutomationControl(AutomationElement element)
            : base(element)
        {
            View = new ColorLegendView(Map);
        }

        private ColorLegendMap Map => Map<ColorLegendMap>();
        public ColorLegendView View { get; }

        [ApiProperty]
        public bool AllowColorEditing
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }

        [ApiProperty]
        public bool ShowSearchBox
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }

        [ApiProperty]
        public bool ShowToolBox
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }

        [ApiProperty]
        public bool ShowBottomToolBox
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }

        [ApiProperty]
        public bool ShowSettingsBox
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }

        [ApiProperty]
        public bool ShowColorVisibilityControls
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }

        [ApiProperty]
        public bool ShowColorPicker
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }

        [ApiProperty]
        public bool IsColorSelecting
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }

        [ApiProperty]
        public Color EditingColor
        {
            get => Access.GetValue<Color>();
            set => Access.SetValue(value);
        }

        [ApiProperty]
        public string Filter
        {
            get => Access.GetValue<string>();
            set => Access.SetValue(value);
        }

        [ApiProperty]
        public IEnumerable<IColorLegendItem> ItemsSource
        {
            get => Access.GetValue<IEnumerable<IColorLegendItem>>();
            set => Access.SetValue(value);
        }

        [ApiProperty]
        public bool? IsAllVisible
        {
            get => Access.GetValue<bool?>();
            set => Access.SetValue(value);
        }

        [ApiProperty]
        public IEnumerable<IColorLegendItem> FilteredItemsSource
        {
            get => Access.GetValue<IEnumerable<IColorLegendItem>>();
            set => Access.SetValue(value);
        }

        [ApiProperty]
        public IEnumerable<string> FilteredItemsIds
        {
            get => Access.GetValue<IEnumerable<string>>();
        }

        [ApiProperty]
        public string FilterWatermark
        {
            get => Access.GetValue<string>();
            set => Access.SetValue(value);
        }

        [ApiProperty]
        public IEnumerable<IColorLegendItem> SelectedColorItems
        {
            get => Access.GetValue<IEnumerable<IColorLegendItem>>();
            set => Access.SetValue(value);
        }

        public IColorLegendItem this[int index]
        {
            get => ItemsSource.ToList()[index];
        }

        public void SetItemCheckState(int index, bool state)
        {
            Access.Execute(nameof(ColorLegendAutomationPeer.SetItemCheckState), index, state);
        }

        protected override void OnEvent(object sender, AutomationEventArgs args)
        {
            if (Equals(args.EventName, nameof(ColorLegend.SelectionChanged)))
            {
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<EventArgs> SelectionChanged;
    }
}
