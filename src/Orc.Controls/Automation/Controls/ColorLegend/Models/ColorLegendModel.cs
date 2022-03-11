namespace Orc.Controls.Automation
{
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

        [ApiProperty]
        public bool AllowColorEditing { get; set; }

        [ApiProperty]
        public bool ShowColorVisibilityControls { get; set; }

        [ApiProperty]
        public bool ShowColorPicker { get; set; }

        [ApiProperty]
        public bool ShowSearchBox { get; set; }

        [ApiProperty]
        public bool ShowToolBox { get; set; }

        [ApiProperty]
        public bool ShowBottomToolBox { get; set; }

        [ApiProperty]
        public bool ShowSettingsBox { get; set; }

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

        public IEnumerable<string> FilteredItemsIds
        {
            get => _accessor.GetValue<IEnumerable<string>>();
        }

        [ApiProperty]
        public string FilterWatermark { get; set; }

        [ApiProperty]
        public IEnumerable<IColorLegendItem> SelectedColorItems { get; set; }

        public IColorLegendItem this[int index]
        {
            get => ItemsSource.ToList()[index];
        }
    }
}
