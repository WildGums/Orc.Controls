[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v3.1", FrameworkDisplayName="")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orc/controls", "Orc.Controls")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orc/controls", "Orc.Controls.Converters")]
[assembly: System.Windows.Markup.XmlnsPrefix("http://schemas.wildgums.com/orc/controls", "orccontrols")]
[assembly: System.Windows.ThemeInfo(System.Windows.ResourceDictionaryLocation.None, System.Windows.ResourceDictionaryLocation.SourceAssembly)]
public static class LoadAssembliesOnStartup { }
public static class ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.Controls
{
    public class AlignmentGrid : System.Windows.Controls.ContentControl
    {
        public static readonly System.Windows.DependencyProperty HorizontalStepProperty;
        public static readonly System.Windows.DependencyProperty LineBrushProperty;
        public static readonly System.Windows.DependencyProperty VerticalStepProperty;
        public AlignmentGrid() { }
        public double HorizontalStep { get; set; }
        public System.Windows.Media.Brush LineBrush { get; set; }
        public double VerticalStep { get; set; }
    }
    public class AnimatedGif : System.Windows.Controls.Image
    {
        public static readonly System.Windows.DependencyProperty GifSourceProperty;
        public AnimatedGif() { }
        public string GifSource { get; set; }
    }
    public class AnimatingTextBlock : System.Windows.Controls.UserControl, Orc.Controls.Services.IStatusRepresenter
    {
        public static readonly System.Windows.DependencyProperty HideStoryboardProperty;
        public static readonly System.Windows.DependencyProperty ShowStoryboardProperty;
        public static readonly System.Windows.DependencyProperty TextProperty;
        public AnimatingTextBlock() { }
        public System.Windows.Media.Animation.Storyboard HideStoryboard { get; set; }
        public System.Windows.Media.Animation.Storyboard ShowStoryboard { get; set; }
        public string Text { get; set; }
        public override void OnApplyTemplate() { }
        public void UpdateStatus(string status) { }
    }
    public class ApplicationLogFilterGroupService : Orc.Controls.IApplicationLogFilterGroupService
    {
        public ApplicationLogFilterGroupService(Orc.FileSystem.IFileService fileService, Catel.Runtime.Serialization.Xml.IXmlSerializer xmlSerializer) { }
        protected virtual System.Collections.Generic.List<Orc.Controls.LogFilterGroup> CreateRuntimeFilterGroups() { }
        public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Orc.Controls.LogFilterGroup>> LoadAsync() { }
        public System.Threading.Tasks.Task SaveAsync(System.Collections.Generic.IEnumerable<Orc.Controls.LogFilterGroup> filterGroups) { }
    }
    public class BindableRichTextBox : System.Windows.Controls.RichTextBox
    {
        public static readonly System.Windows.DependencyProperty AutoScrollToEndProperty;
        public static readonly System.Windows.DependencyProperty BindableDocumentProperty;
        public BindableRichTextBox() { }
        public bool AutoScrollToEnd { get; set; }
        public System.Windows.Documents.FlowDocument BindableDocument { get; set; }
    }
    public class BindableRun : System.Windows.Documents.Run
    {
        public static readonly System.Windows.DependencyProperty BoundTextProperty;
        public BindableRun() { }
        public string BoundText { get; set; }
    }
    public class BusyIndicator : Orc.Controls.VisualWrapper, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty ForegroundProperty;
        public static readonly System.Windows.DependencyProperty IgnoreUnloadedEventCountProperty;
        public BusyIndicator() { }
        public System.Windows.Media.Brush Foreground { get; set; }
        public int IgnoreUnloadedEventCount { get; set; }
        public void InitializeComponent() { }
        protected override void OnRenderSizeChanged(System.Windows.SizeChangedInfo sizeInfo) { }
    }
    [System.Windows.TemplatePart(Name="PART_A0GradientStop", Type=typeof(System.Windows.Media.GradientStop))]
    [System.Windows.TemplatePart(Name="PART_A1GradientStop", Type=typeof(System.Windows.Media.GradientStop))]
    [System.Windows.TemplatePart(Name="PART_ASlider", Type=typeof(System.Windows.Controls.Slider))]
    [System.Windows.TemplatePart(Name="PART_ATextBox", Type=typeof(System.Windows.Controls.TextBox))]
    [System.Windows.TemplatePart(Name="PART_B0GradientStop", Type=typeof(System.Windows.Media.GradientStop))]
    [System.Windows.TemplatePart(Name="PART_B1GradientStop", Type=typeof(System.Windows.Media.GradientStop))]
    [System.Windows.TemplatePart(Name="PART_BSlider", Type=typeof(System.Windows.Controls.Slider))]
    [System.Windows.TemplatePart(Name="PART_BTextBox", Type=typeof(System.Windows.Controls.TextBox))]
    [System.Windows.TemplatePart(Name="PART_CancelButton", Type=typeof(System.Windows.Controls.Button))]
    [System.Windows.TemplatePart(Name="PART_ColorBrush", Type=typeof(System.Windows.Media.SolidColorBrush))]
    [System.Windows.TemplatePart(Name="PART_ColorComboBox", Type=typeof(System.Windows.Controls.ComboBox))]
    [System.Windows.TemplatePart(Name="PART_ColorTextBox", Type=typeof(System.Windows.Controls.TextBox))]
    [System.Windows.TemplatePart(Name="PART_G0GradientStop", Type=typeof(System.Windows.Media.GradientStop))]
    [System.Windows.TemplatePart(Name="PART_G1GradientStop", Type=typeof(System.Windows.Media.GradientStop))]
    [System.Windows.TemplatePart(Name="PART_GSlider", Type=typeof(System.Windows.Controls.Slider))]
    [System.Windows.TemplatePart(Name="PART_GTextBox", Type=typeof(System.Windows.Controls.TextBox))]
    [System.Windows.TemplatePart(Name="PART_HSVCanvas", Type=typeof(System.Windows.Controls.Canvas))]
    [System.Windows.TemplatePart(Name="PART_HSVColorGradientStop", Type=typeof(System.Windows.Media.GradientStop))]
    [System.Windows.TemplatePart(Name="PART_HSVEllipse", Type=typeof(System.Windows.Shapes.Ellipse))]
    [System.Windows.TemplatePart(Name="PART_HSVRectangle", Type=typeof(System.Windows.Shapes.Rectangle))]
    [System.Windows.TemplatePart(Name="PART_HSVSlider", Type=typeof(System.Windows.Controls.Slider))]
    [System.Windows.TemplatePart(Name="PART_R0GradientStop", Type=typeof(System.Windows.Media.GradientStop))]
    [System.Windows.TemplatePart(Name="PART_R1GradientStop", Type=typeof(System.Windows.Media.GradientStop))]
    [System.Windows.TemplatePart(Name="PART_RSlider", Type=typeof(System.Windows.Controls.Slider))]
    [System.Windows.TemplatePart(Name="PART_RTextBox", Type=typeof(System.Windows.Controls.TextBox))]
    [System.Windows.TemplatePart(Name="PART_RecentColorsListBox", Type=typeof(System.Windows.Controls.ListBox))]
    [System.Windows.TemplatePart(Name="PART_RootGrid", Type=typeof(System.Windows.FrameworkElement))]
    [System.Windows.TemplatePart(Name="PART_SelectButton", Type=typeof(System.Windows.Controls.Button))]
    [System.Windows.TemplatePart(Name="PART_ThemeColorsListBox", Type=typeof(System.Windows.Controls.ListBox))]
    public class ColorBoard : System.Windows.Controls.Control
    {
        public static readonly System.Windows.DependencyProperty ColorProperty;
        public static readonly System.Windows.DependencyProperty RecentColorItemsProperty;
        public ColorBoard() { }
        public System.Windows.Media.Color Color { get; set; }
        public System.Collections.Generic.List<Orc.Controls.PredefinedColorItem> RecentColorItems { get; set; }
        public event System.Windows.RoutedEventHandler CancelClicked;
        public event System.Windows.RoutedEventHandler DoneClicked;
        public override void OnApplyTemplate() { }
        public void OnDoneClicked() { }
    }
    public class ColorChangedEventArgs : System.EventArgs
    {
        public ColorChangedEventArgs(System.Windows.Media.Color newColor, System.Windows.Media.Color oldColor) { }
        public System.Windows.Media.Color NewColor { get; set; }
        public System.Windows.Media.Color OldColor { get; set; }
    }
    public static class ColorHelper
    {
        public static double GetHSB_B(System.Windows.Media.Color color) { }
        public static double GetHSB_H(System.Windows.Media.Color color) { }
        public static double GetHSB_S(System.Windows.Media.Color color) { }
        public static double GetHSV_H(System.Windows.Media.Color color) { }
        public static double GetHSV_S(System.Windows.Media.Color color) { }
        public static double GetHSV_V(System.Windows.Media.Color color) { }
        public static System.Windows.Media.Color HSB2RGB(double h, double s, double l) { }
        public static System.Windows.Media.Color HSV2RGB(double h, double s, double v) { }
    }
    [System.Windows.TemplatePart(Name="PART_All_Visible", Type=typeof(System.Windows.Controls.CheckBox))]
    [System.Windows.TemplatePart(Name="PART_List", Type=typeof(System.Windows.Controls.ListBox))]
    [System.Windows.TemplatePart(Name="PART_Popup_Color_Board", Type=typeof(System.Windows.Controls.Primitives.Popup))]
    [System.Windows.TemplatePart(Name="PART_Settings_Button", Type=typeof(Orc.Controls.DropDownButton))]
    [System.Windows.TemplatePart(Name="PART_UnselectAll", Type=typeof(System.Windows.Controls.Primitives.ButtonBase))]
    public class ColorLegend : System.Windows.Controls.HeaderedContentControl
    {
        public static readonly System.Windows.DependencyProperty AllowColorEditingProperty;
        public static readonly System.Windows.DependencyProperty EditingColorProperty;
        public static readonly System.Windows.DependencyProperty FilterProperty;
        public static readonly System.Windows.DependencyProperty FilterWatermarkProperty;
        public static readonly System.Windows.DependencyProperty FilteredItemsIdsProperty;
        public static readonly System.Windows.DependencyProperty FilteredItemsSourceProperty;
        public static readonly System.Windows.DependencyProperty IsAllVisibleProperty;
        public static readonly System.Windows.DependencyProperty IsColorSelectingProperty;
        public static readonly System.Windows.DependencyProperty ItemsSourceProperty;
        public static readonly System.Windows.DependencyProperty SelectedColorItemsProperty;
        public static readonly System.Windows.DependencyProperty ShowBottomToolBoxProperty;
        public static readonly System.Windows.DependencyProperty ShowColorPickerProperty;
        public static readonly System.Windows.DependencyProperty ShowColorVisibilityControlsProperty;
        public static readonly System.Windows.DependencyProperty ShowSearchBoxProperty;
        public static readonly System.Windows.DependencyProperty ShowSettingsBoxProperty;
        public static readonly System.Windows.DependencyProperty ShowToolBoxProperty;
        public ColorLegend() { }
        public bool AllowColorEditing { get; set; }
        public Catel.MVVM.Command<object> ChangeColor { get; }
        public System.Windows.Media.Color EditingColor { get; set; }
        public string Filter { get; set; }
        public string FilterWatermark { get; set; }
        public System.Collections.Generic.IEnumerable<string> FilteredItemsIds { get; set; }
        public System.Collections.Generic.IEnumerable<Orc.Controls.IColorLegendItem> FilteredItemsSource { get; set; }
        public bool? IsAllVisible { get; set; }
        public bool IsColorSelecting { get; set; }
        public System.Collections.Generic.IEnumerable<Orc.Controls.IColorLegendItem> ItemsSource { get; set; }
        public System.Collections.Generic.IEnumerable<Orc.Controls.IColorLegendItem> SelectedColorItems { get; set; }
        public bool ShowBottomToolBox { get; set; }
        public bool ShowColorPicker { get; set; }
        public bool ShowColorVisibilityControls { get; set; }
        public bool ShowSearchBox { get; set; }
        public bool ShowSettingsBox { get; set; }
        public bool ShowToolBox { get; set; }
        public event System.EventHandler<System.EventArgs> SelectionChanged;
        protected System.Collections.Generic.IEnumerable<Orc.Controls.IColorLegendItem> GetFilteredItems() { }
        public System.Collections.Generic.IEnumerable<Orc.Controls.IColorLegendItem> GetSelectedList() { }
        public override void OnApplyTemplate() { }
        public void SetSelectedList(System.Collections.Generic.IEnumerable<Orc.Controls.IColorLegendItem> selectedList) { }
        public void UpdateColorEditingControlsVisibility() { }
        public void UpdateColorPickerColorVisibility() { }
        public void UpdateVisibilityControlsVisibility() { }
    }
    [Catel.Runtime.Serialization.SerializerModifier(typeof(Orc.Controls.ColorLegendItemSerializerModifier))]
    public class ColorLegendItem : Catel.Data.ModelBase, Orc.Controls.IColorLegendItem, System.ComponentModel.INotifyPropertyChanged
    {
        public static readonly Catel.Data.PropertyData AdditionalDataProperty;
        public static readonly Catel.Data.PropertyData ColorProperty;
        public static readonly Catel.Data.PropertyData DescriptionProperty;
        public static readonly Catel.Data.PropertyData IdProperty;
        public static readonly Catel.Data.PropertyData IsCheckedProperty;
        public static readonly Catel.Data.PropertyData IsSelectedProperty;
        public ColorLegendItem() { }
        public string AdditionalData { get; set; }
        public System.Windows.Media.Color Color { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public bool IsChecked { get; set; }
        public bool IsSelected { get; set; }
    }
    public class ColorLegendItemSerializerModifier : Catel.Runtime.Serialization.SerializerModifierBase<Orc.Controls.ColorLegendItem>
    {
        public ColorLegendItemSerializerModifier() { }
        public override void DeserializeMember(Catel.Runtime.Serialization.ISerializationContext context, Catel.Runtime.Serialization.MemberValue memberValue) { }
        public override void SerializeMember(Catel.Runtime.Serialization.ISerializationContext context, Catel.Runtime.Serialization.MemberValue memberValue) { }
    }
    [System.Windows.TemplatePart(Name="PART_Popup", Type=typeof(System.Windows.Controls.Primitives.Popup))]
    public class ColorPicker : System.Windows.Controls.Control
    {
        public static readonly System.Windows.DependencyProperty ColorProperty;
        public static readonly System.Windows.DependencyProperty CurrentColorProperty;
        public static readonly System.Windows.DependencyProperty IsDropDownOpenProperty;
        public static readonly System.Windows.DependencyProperty PopupPlacementProperty;
        public ColorPicker() { }
        public System.Windows.Media.Color Color { get; set; }
        public System.Windows.Media.Color CurrentColor { get; set; }
        public bool IsDropDownOpen { get; set; }
        public System.Windows.Controls.Primitives.PlacementMode PopupPlacement { get; set; }
        public event System.EventHandler<Orc.Controls.ColorChangedEventArgs> ColorChanged;
        public override void OnApplyTemplate() { }
    }
    public class ComboboxEditableControlBehavior : Orc.Controls.EditableControlBehaviorBase<System.Windows.Controls.ComboBox>
    {
        public ComboboxEditableControlBehavior() { }
        protected override void OnAssociatedObjectLoaded() { }
        protected override void OnAssociatedObjectUnloaded() { }
    }
    public abstract class ControlToolBase : Catel.Data.ModelBase, Orc.Controls.IControlTool
    {
        protected object Target;
        public static readonly Catel.Data.PropertyData IsOpenedProperty;
        protected ControlToolBase() { }
        public virtual bool IsEnabled { get; }
        public bool IsOpened { get; }
        public abstract string Name { get; }
        public event System.EventHandler<System.EventArgs> Attached;
        public event System.EventHandler<System.EventArgs> Closed;
        public event System.EventHandler<System.EventArgs> Detached;
        public event System.EventHandler<System.EventArgs> Opened;
        public event System.EventHandler<System.EventArgs> Opening;
        public virtual void Attach(object target) { }
        public virtual void Close() { }
        public virtual void Detach() { }
        protected virtual void OnAddParameter(object parameter) { }
        protected abstract void OnOpen(object parameter = null);
        public void Open(object parameter = null) { }
    }
    public sealed class CulturePicker : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty SelectedCultureProperty;
        public CulturePicker() { }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public System.Globalization.CultureInfo SelectedCulture { get; set; }
        public void InitializeComponent() { }
    }
    [System.Windows.TemplatePart(Name="PART_ClearButton", Type=typeof(System.Windows.Controls.Button))]
    [System.Windows.TemplatePart(Name="PART_CopyButton", Type=typeof(System.Windows.Controls.Button))]
    [System.Windows.TemplatePart(Name="PART_DatePickerIconToggleButton", Type=typeof(System.Windows.Controls.Primitives.ToggleButton))]
    [System.Windows.TemplatePart(Name="PART_DaysMonthsSeparatorTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_DaysNumericTextBox", Type=typeof(Orc.Controls.NumericTextBox))]
    [System.Windows.TemplatePart(Name="PART_DaysToggleButton", Type=typeof(System.Windows.Controls.Primitives.ToggleButton))]
    [System.Windows.TemplatePart(Name="PART_MainGrid", Type=typeof(System.Windows.Controls.Grid))]
    [System.Windows.TemplatePart(Name="PART_MonthNumericTextBox", Type=typeof(Orc.Controls.NumericTextBox))]
    [System.Windows.TemplatePart(Name="PART_MonthToggleButton", Type=typeof(System.Windows.Controls.Primitives.ToggleButton))]
    [System.Windows.TemplatePart(Name="PART_MonthsYearSeparatorTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_PasteButton", Type=typeof(System.Windows.Controls.Button))]
    [System.Windows.TemplatePart(Name="PART_SelectDateButton", Type=typeof(System.Windows.Controls.Button))]
    [System.Windows.TemplatePart(Name="PART_TodayButton", Type=typeof(System.Windows.Controls.Button))]
    [System.Windows.TemplatePart(Name="PART_YearNumericTextBox", Type=typeof(Orc.Controls.NumericTextBox))]
    [System.Windows.TemplatePart(Name="PART_YearSeparatorTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_YearToggleButton", Type=typeof(System.Windows.Controls.Primitives.ToggleButton))]
    public class DatePicker : System.Windows.Controls.Control
    {
        public static readonly System.Windows.DependencyProperty AllowCopyPasteProperty;
        public static readonly System.Windows.DependencyProperty AllowNullProperty;
        public static readonly System.Windows.DependencyProperty FormatProperty;
        public static readonly System.Windows.DependencyProperty IsReadOnlyProperty;
        public static readonly System.Windows.DependencyProperty IsYearShortFormatProperty;
        public static readonly System.Windows.DependencyProperty ShowOptionsButtonProperty;
        public static readonly System.Windows.DependencyProperty ValueProperty;
        public DatePicker() { }
        public bool AllowCopyPaste { get; set; }
        public bool AllowNull { get; set; }
        public string Format { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsYearShortFormat { get; }
        public bool ShowOptionsButton { get; set; }
        public System.DateTime? Value { get; set; }
        public override void OnApplyTemplate() { }
    }
    public class DateRange : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData EndProperty;
        public static readonly Catel.Data.PropertyData IsTemporaryProperty;
        public static readonly Catel.Data.PropertyData NameProperty;
        public static readonly Catel.Data.PropertyData StartProperty;
        public DateRange() { }
        public System.TimeSpan Duration { get; }
        public System.DateTime End { get; set; }
        public bool IsTemporary { get; }
        public string Name { get; set; }
        public System.DateTime Start { get; set; }
        protected override void OnPropertyChanged(Catel.Data.AdvancedPropertyChangedEventArgs e) { }
        public override string ToString() { }
    }
    public class DateRangePicker : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty AllowCopyPasteProperty;
        public static readonly System.Windows.DependencyProperty EndDateProperty;
        public static readonly System.Windows.DependencyProperty FormatProperty;
        public static readonly System.Windows.DependencyProperty HideSecondsProperty;
        public static readonly System.Windows.DependencyProperty HideTimeProperty;
        public static readonly System.Windows.DependencyProperty IsAdvancedModeProperty;
        public static readonly System.Windows.DependencyProperty IsReadOnlyProperty;
        public static readonly System.Windows.DependencyProperty RangesProperty;
        public static readonly System.Windows.DependencyProperty SelectedRangeProperty;
        public static readonly System.Windows.DependencyProperty ShowOptionsButtonProperty;
        public static readonly System.Windows.DependencyProperty SpanProperty;
        public static readonly System.Windows.DependencyProperty StartDateProperty;
        public DateRangePicker() { }
        public bool AllowCopyPaste { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public System.DateTime EndDate { get; set; }
        public string Format { get; set; }
        public bool HideSeconds { get; set; }
        public bool HideTime { get; set; }
        public bool IsAdvancedMode { get; set; }
        public bool IsReadOnly { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.DateRange> Ranges { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public Orc.Controls.DateRange SelectedRange { get; set; }
        public bool ShowOptionsButton { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public System.TimeSpan Span { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public System.DateTime StartDate { get; set; }
        public void InitializeComponent() { }
    }
    public class DateRangePickerViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData TimeAdjustmentStrategyProperty;
        public DateRangePickerViewModel() { }
        public System.DateTime EndDate { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.DateRange> Ranges { get; set; }
        public Orc.Controls.DateRange SelectedRange { get; set; }
        public System.TimeSpan Span { get; set; }
        public System.DateTime StartDate { get; set; }
        public Orc.Controls.TimeAdjustmentStrategy TimeAdjustmentStrategy { get; set; }
        protected override void OnValidating(Catel.Data.IValidationContext validationContext) { }
    }
    public static class DateTimeFormatHelper
    {
        public static string ExtractDatePatternFromFormat(string format) { }
        public static string ExtractTimePatternFromFormat(string format) { }
        public static string FindMatchedLongTimePattern(System.Globalization.CultureInfo cultureInfo, string timePattern) { }
        public static Orc.Controls.DateTimeFormatInfo GetDateTimeFormatInfo(string format, bool isDateOnly = false) { }
        public static string[] Split(string format, char[] formatCharacters) { }
    }
    public class DateTimeFormatInfo
    {
        public DateTimeFormatInfo() { }
        public string AmPmFormat { get; set; }
        public int? AmPmPosition { get; set; }
        public string DayFormat { get; set; }
        public int DayPosition { get; set; }
        public string HourFormat { get; set; }
        public int? HourPosition { get; set; }
        public bool? IsAmPmShortFormat { get; set; }
        public bool IsDateOnly { get; }
        public bool? IsHour12Format { get; set; }
        public bool IsYearShortFormat { get; set; }
        public int MaxPosition { get; set; }
        public string MinuteFormat { get; set; }
        public int? MinutePosition { get; set; }
        public string MonthFormat { get; set; }
        public int MonthPosition { get; set; }
        public string SecondFormat { get; set; }
        public int? SecondPosition { get; set; }
        public string Separator0 { get; set; }
        public string Separator1 { get; set; }
        public string Separator2 { get; set; }
        public string Separator3 { get; set; }
        public string Separator4 { get; set; }
        public string Separator5 { get; set; }
        public string Separator6 { get; set; }
        public string Separator7 { get; set; }
        public string YearFormat { get; set; }
        public int YearPosition { get; set; }
        public string GetSeparator(int position) { }
    }
    public static class DateTimeFormatter
    {
        public static string Format(System.DateTime dateTime, string format, bool isDateOnly = false) { }
    }
    public static class DateTimeParser
    {
        public static System.DateTime Parse(string input, string format, bool isDateOnly = false) { }
        public static bool TryParse(string input, string format, out System.DateTime dateTime, bool isDateOnly = false) { }
    }
    public enum DateTimePart
    {
        Day = 0,
        Month = 1,
        Year = 2,
        Hour = 3,
        Hour12 = 4,
        Minute = 5,
        Second = 6,
        AmPmDesignator = 7,
    }
    public class DateTimePartHelper
    {
        public DateTimePartHelper(System.DateTime dateTime, Orc.Controls.DateTimePart dateTimePart, Orc.Controls.DateTimeFormatInfo dateTimeFormatInfo, System.Windows.Controls.TextBox textBox, System.Windows.Controls.Primitives.ToggleButton activeToggleButton) { }
        public System.Windows.Controls.Primitives.Popup CreatePopup() { }
    }
    [System.Windows.TemplatePart(Name="PART_AmPmListTextBox", Type=typeof(Orc.Controls.ListTextBox))]
    [System.Windows.TemplatePart(Name="PART_AmPmSeparatorTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_AmPmToggleButton", Type=typeof(System.Windows.Controls.Primitives.ToggleButton))]
    [System.Windows.TemplatePart(Name="PART_Calendar", Type=typeof(System.Windows.Controls.Calendar))]
    [System.Windows.TemplatePart(Name="PART_CalendarPopup", Type=typeof(System.Windows.Controls.Primitives.Popup))]
    [System.Windows.TemplatePart(Name="PART_ClearMenuItem", Type=typeof(System.Windows.Controls.MenuItem))]
    [System.Windows.TemplatePart(Name="PART_CopyMenuItem", Type=typeof(System.Windows.Controls.MenuItem))]
    [System.Windows.TemplatePart(Name="PART_DatePickerIconDropDownButton", Type=typeof(Orc.Controls.DropDownButton))]
    [System.Windows.TemplatePart(Name="PART_DaysMonthsSeparatorTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_DaysNumericTextBox", Type=typeof(Orc.Controls.NumericTextBox))]
    [System.Windows.TemplatePart(Name="PART_DaysToggleButton", Type=typeof(System.Windows.Controls.Primitives.ToggleButton))]
    [System.Windows.TemplatePart(Name="PART_HourMinuteSeparatorTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_HourNumericTextBox", Type=typeof(Orc.Controls.NumericTextBox))]
    [System.Windows.TemplatePart(Name="PART_HourToggleButton", Type=typeof(System.Windows.Controls.Primitives.ToggleButton))]
    [System.Windows.TemplatePart(Name="PART_MainGrid", Type=typeof(System.Windows.Controls.Grid))]
    [System.Windows.TemplatePart(Name="PART_MinuteNumericTextBox", Type=typeof(Orc.Controls.NumericTextBox))]
    [System.Windows.TemplatePart(Name="PART_MinuteSecondSeparatorTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_MinuteToggleButton", Type=typeof(System.Windows.Controls.Primitives.ToggleButton))]
    [System.Windows.TemplatePart(Name="PART_MonthNumericTextBox", Type=typeof(Orc.Controls.NumericTextBox))]
    [System.Windows.TemplatePart(Name="PART_MonthToggleButton", Type=typeof(System.Windows.Controls.Primitives.ToggleButton))]
    [System.Windows.TemplatePart(Name="PART_MonthsYearSeparatorTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_NowMenuItem", Type=typeof(System.Windows.Controls.MenuItem))]
    [System.Windows.TemplatePart(Name="PART_PasteMenuItem", Type=typeof(System.Windows.Controls.MenuItem))]
    [System.Windows.TemplatePart(Name="PART_SecondAmPmSeparatorTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_SecondNumericTextBox", Type=typeof(Orc.Controls.NumericTextBox))]
    [System.Windows.TemplatePart(Name="PART_SecondToggleButton", Type=typeof(System.Windows.Controls.Primitives.ToggleButton))]
    [System.Windows.TemplatePart(Name="PART_SelectDateMenuItem", Type=typeof(System.Windows.Controls.MenuItem))]
    [System.Windows.TemplatePart(Name="PART_TodayMenuItem", Type=typeof(System.Windows.Controls.MenuItem))]
    [System.Windows.TemplatePart(Name="PART_YearNumericTextBox", Type=typeof(Orc.Controls.NumericTextBox))]
    [System.Windows.TemplatePart(Name="PART_YearSeparatorTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_YearToggleButton", Type=typeof(System.Windows.Controls.Primitives.ToggleButton))]
    public class DateTimePicker : System.Windows.Controls.Control, Orc.Controls.IEditableControl
    {
        public static readonly System.Windows.DependencyProperty AllowCopyPasteProperty;
        public static readonly System.Windows.DependencyProperty AllowNullProperty;
        public static readonly System.Windows.DependencyProperty CultureProperty;
        public static readonly System.Windows.DependencyProperty FirstDayOfWeekProperty;
        public static readonly System.Windows.DependencyProperty FormatProperty;
        public static readonly System.Windows.DependencyProperty HideSecondsProperty;
        public static readonly System.Windows.DependencyProperty HideTimeProperty;
        public static readonly System.Windows.DependencyProperty IsAmPmShortFormatProperty;
        public static readonly System.Windows.DependencyProperty IsHour12FormatProperty;
        public static readonly System.Windows.DependencyProperty IsReadOnlyProperty;
        public static readonly System.Windows.DependencyProperty IsYearShortFormatProperty;
        public static readonly System.Windows.DependencyProperty ShowOptionsButtonProperty;
        public static readonly System.Windows.DependencyProperty ValueProperty;
        public DateTimePicker() { }
        public bool AllowCopyPaste { get; set; }
        public bool AllowNull { get; set; }
        public System.Globalization.CultureInfo Culture { get; set; }
        public System.DayOfWeek? FirstDayOfWeek { get; set; }
        public string Format { get; set; }
        public bool HideSeconds { get; set; }
        public bool HideTime { get; set; }
        public bool IsAmPmShortFormat { get; }
        public bool IsHour12Format { get; }
        public bool IsInEditMode { get; }
        public bool IsReadOnly { get; set; }
        public bool IsYearShortFormat { get; }
        public bool ShowOptionsButton { get; set; }
        public System.DateTime? Value { get; set; }
        public event System.EventHandler<System.EventArgs> EditEnded;
        public event System.EventHandler<System.EventArgs> EditStarted;
        public override void OnApplyTemplate() { }
        protected override void OnGotKeyboardFocus(System.Windows.Input.KeyboardFocusChangedEventArgs e) { }
        protected override void OnIsKeyboardFocusedChanged(System.Windows.DependencyPropertyChangedEventArgs e) { }
        protected override void OnLostKeyboardFocus(System.Windows.Input.KeyboardFocusChangedEventArgs e) { }
        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e) { }
    }
    public static class DependencyObjectExtensions
    {
        public static System.Collections.Generic.IEnumerable<System.Windows.DependencyObject> GetDescendents(this System.Windows.DependencyObject root) { }
        public static System.Windows.DependencyObject GetVisualRoot(this System.Windows.DependencyObject dependencyObject) { }
    }
    public abstract class DialogWindowHostedToolBase<T> : Orc.Controls.ControlToolBase
        where T : Catel.MVVM.ViewModelBase
    {
        protected object Parameter;
        protected readonly Catel.IoC.ITypeFactory TypeFactory;
        protected T WindowViewModel;
        protected DialogWindowHostedToolBase(Catel.IoC.ITypeFactory typeFactory, Catel.Services.IUIVisualizerService uiVisualizerService) { }
        public virtual bool IsModal { get; }
        protected virtual void ApplyParameter(object parameter) { }
        public override void Close() { }
        protected abstract T InitializeViewModel();
        protected abstract void OnAccepted();
        protected override void OnOpen(object parameter = null) { }
    }
    public class DirectoryPicker : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty LabelTextProperty;
        public static readonly System.Windows.DependencyProperty LabelWidthProperty;
        public static readonly System.Windows.DependencyProperty SelectedDirectoryProperty;
        public DirectoryPicker() { }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string LabelText { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public double LabelWidth { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string SelectedDirectory { get; set; }
        public void InitializeComponent() { }
    }
    public class DirectoryPickerViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData LabelTextProperty;
        public static readonly Catel.Data.PropertyData LabelWidthProperty;
        public static readonly Catel.Data.PropertyData SelectedDirectoryProperty;
        public DirectoryPickerViewModel(Catel.Services.ISelectDirectoryService selectDirectoryService, Catel.Services.IProcessService processService) { }
        public Catel.MVVM.Command Clear { get; }
        public string LabelText { get; set; }
        public double LabelWidth { get; set; }
        public Catel.MVVM.Command OpenDirectory { get; }
        public Catel.MVVM.TaskCommand SelectDirectory { get; }
        public string SelectedDirectory { get; set; }
    }
    public class DoNotShowDropDownOnClickComboboxBehavior : Catel.Windows.Interactivity.BehaviorBase<System.Windows.Controls.ComboBox>
    {
        public DoNotShowDropDownOnClickComboboxBehavior() { }
        protected override void OnAssociatedObjectLoaded() { }
        protected override void OnAssociatedObjectUnloaded() { }
    }
    [System.Windows.TemplatePart(Name="PART_Arrow", Type=typeof(System.Windows.Shapes.Path))]
    [System.Windows.TemplatePart(Name="PART_ArrowBorder", Type=typeof(System.Windows.Controls.Border))]
    public class DropDownButton : System.Windows.Controls.Primitives.ToggleButton
    {
        public static readonly System.Windows.DependencyProperty ArrowLocationProperty;
        public static readonly System.Windows.DependencyProperty ArrowMarginProperty;
        public static readonly System.Windows.DependencyProperty DropDownProperty;
        public static readonly System.Windows.DependencyProperty HeaderProperty;
        public static readonly System.Windows.DependencyProperty IsArrowVisibleProperty;
        public DropDownButton() { }
        public Orc.Controls.DropdownArrowLocation ArrowLocation { get; set; }
        public System.Windows.Thickness ArrowMargin { get; set; }
        public System.Windows.Controls.ContextMenu DropDown { get; set; }
        public object Header { get; set; }
        public bool IsArrowVisible { get; set; }
        public override void OnApplyTemplate() { }
        protected override void OnChecked(System.Windows.RoutedEventArgs e) { }
        protected override void OnContentChanged(object oldContent, object newContent) { }
        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e) { }
        protected override void OnPreviewMouseDown(System.Windows.Input.MouseButtonEventArgs e) { }
        protected override void OnUnchecked(System.Windows.RoutedEventArgs e) { }
    }
    public enum DropdownArrowLocation
    {
        Left = 0,
        Top = 1,
        Right = 2,
        Bottom = 3,
    }
    public class DropdownArrowLocationConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public DropdownArrowLocationConverter() { }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
    }
    public abstract class EditableControlBehaviorBase<T> : Catel.Windows.Interactivity.BehaviorBase<T>, Orc.Controls.IEditableControl
        where T : System.Windows.FrameworkElement
    {
        public static readonly System.Windows.DependencyProperty IsInEditModeProperty;
        protected EditableControlBehaviorBase() { }
        public bool IsInEditMode { get; set; }
        public event System.EventHandler<System.EventArgs> EditEnded;
        public event System.EventHandler<System.EventArgs> EditStarted;
        protected void RaiseEditEnded() { }
        protected void RaiseEditStarted() { }
    }
    public class EmptyCell : System.Windows.Controls.Control
    {
        public EmptyCell() { }
    }
    public class EmptyColumn : System.Windows.Controls.Control
    {
        public EmptyColumn() { }
    }
    public class EmptyRow : System.Windows.Controls.Control
    {
        public EmptyRow() { }
    }
    public static class EnterKeyTraversal
    {
        public static readonly System.Windows.DependencyProperty IsEnabledProperty;
        public static bool GetIsEnabled(System.Windows.DependencyObject obj) { }
        public static void SetIsEnabled(System.Windows.DependencyObject obj, bool value) { }
    }
    public enum ExpandDirection
    {
        Down = 0,
        Up = 1,
        Left = 2,
        Right = 3,
    }
    [System.Windows.TemplatePart(Name="PART_ExpandSite", Type=typeof(System.Windows.Controls.ContentPresenter))]
    [System.Windows.TemplatePart(Name="PART_HeaderSiteBorder", Type=typeof(System.Windows.Controls.Border))]
    [System.Windows.TemplateVisualState(GroupName="Expander", Name="Collapsed")]
    [System.Windows.TemplateVisualState(GroupName="Expander", Name="Expanded")]
    public class Expander : System.Windows.Controls.HeaderedContentControl
    {
        public static readonly System.Windows.DependencyProperty AutoResizeGridProperty;
        public static readonly System.Windows.DependencyProperty ExpandDirectionProperty;
        public static readonly System.Windows.DependencyProperty ExpandDurationProperty;
        public static readonly System.Windows.DependencyProperty IsExpandedProperty;
        public Expander() { }
        public bool AutoResizeGrid { get; set; }
        public Orc.Controls.ExpandDirection ExpandDirection { get; set; }
        public double ExpandDuration { get; set; }
        public bool IsExpanded { get; set; }
        public override void OnApplyTemplate() { }
        protected virtual void OnCollapsed() { }
        protected virtual void OnExpanded() { }
    }
    [System.Windows.TemplatePart(Name="PART_ClearButton", Type=typeof(System.Windows.Controls.Button))]
    public class FilterBox : System.Windows.Controls.TextBox
    {
        public static readonly System.Windows.DependencyProperty AllowAutoCompletionProperty;
        public static readonly System.Windows.DependencyProperty FilterSourceProperty;
        public static readonly System.Windows.DependencyProperty PropertyNameProperty;
        public static readonly System.Windows.DependencyProperty WatermarkProperty;
        public FilterBox() { }
        public bool AllowAutoCompletion { get; set; }
        public System.Collections.IEnumerable FilterSource { get; set; }
        public string PropertyName { get; set; }
        public string Watermark { get; set; }
        public event System.EventHandler<Orc.Controls.InitializingAutoCompletionServiceEventArgs> InitializingAutoCompletionService;
        public override void OnApplyTemplate() { }
        protected virtual void OnInitializingAutoCompletionService(Orc.Controls.InitializingAutoCompletionServiceEventArgs e) { }
        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e) { }
        protected override void OnTextChanged(System.Windows.Controls.TextChangedEventArgs e) { }
    }
    public class FindReplaceSettings : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData CaseSensitiveProperty;
        public static readonly Catel.Data.PropertyData IsSearchUpProperty;
        public static readonly Catel.Data.PropertyData UseRegexProperty;
        public static readonly Catel.Data.PropertyData UseWildcardsProperty;
        public static readonly Catel.Data.PropertyData WholeWordProperty;
        public FindReplaceSettings() { }
        public bool CaseSensitive { get; set; }
        public bool IsSearchUp { get; set; }
        public bool UseRegex { get; set; }
        public bool UseWildcards { get; set; }
        public bool WholeWord { get; set; }
    }
    public class FindReplaceTool<TFindReplaceService> : Orc.Controls.ControlToolBase
        where TFindReplaceService : Orc.Controls.Services.IFindReplaceService
    {
        public FindReplaceTool(Catel.Services.IUIVisualizerService uiVisualizerService, Catel.IoC.ITypeFactory typeFactory) { }
        public override string Name { get; }
        public override void Attach(object target) { }
        public override void Close() { }
        protected virtual TFindReplaceService CreateFindReplaceService(object target) { }
        public override void Detach() { }
        protected override void OnOpen(object parameter = null) { }
    }
    public class FluidProgressBar : System.Windows.Controls.UserControl, System.IDisposable, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty DelayProperty;
        public static readonly System.Windows.DependencyProperty DotHeightProperty;
        public static readonly System.Windows.DependencyProperty DotRadiusXProperty;
        public static readonly System.Windows.DependencyProperty DotRadiusYProperty;
        public static readonly System.Windows.DependencyProperty DotWidthProperty;
        public static readonly System.Windows.DependencyProperty DurationAProperty;
        public static readonly System.Windows.DependencyProperty DurationBProperty;
        public static readonly System.Windows.DependencyProperty DurationCProperty;
        public static readonly System.Windows.DependencyProperty KeyFrameAProperty;
        public static readonly System.Windows.DependencyProperty KeyFrameBProperty;
        public static readonly System.Windows.DependencyProperty OscillateProperty;
        public static readonly System.Windows.DependencyProperty ReverseDurationProperty;
        public static readonly System.Windows.DependencyProperty TotalDurationProperty;
        public FluidProgressBar() { }
        public System.Windows.Duration Delay { get; set; }
        public double DotHeight { get; set; }
        public double DotRadiusX { get; set; }
        public double DotRadiusY { get; set; }
        public double DotWidth { get; set; }
        public System.Windows.Duration DurationA { get; set; }
        public System.Windows.Duration DurationB { get; set; }
        public System.Windows.Duration DurationC { get; set; }
        public double KeyFrameA { get; set; }
        public double KeyFrameB { get; set; }
        public bool Oscillate { get; set; }
        public System.Windows.Duration ReverseDuration { get; set; }
        public System.Windows.Duration TotalDuration { get; set; }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        protected override void Finalize() { }
        public void InitializeComponent() { }
        protected virtual void OnDelayChanged(System.Windows.Duration oldDelay, System.Windows.Duration newDelay) { }
        protected virtual void OnDotHeightChanged(double oldDotHeight, double newDotHeight) { }
        protected virtual void OnDotRadiusXChanged(double oldDotRadiusX, double newDotRadiusX) { }
        protected virtual void OnDotRadiusYChanged(double oldDotRadiusY, double newDotRadiusY) { }
        protected virtual void OnDotWidthChanged(double oldDotWidth, double newDotWidth) { }
        protected virtual void OnDurationAChanged(System.Windows.Duration oldDurationA, System.Windows.Duration newDurationA) { }
        protected virtual void OnDurationBChanged(System.Windows.Duration oldDurationB, System.Windows.Duration newDurationB) { }
        protected virtual void OnDurationCChanged(System.Windows.Duration oldDurationC, System.Windows.Duration newDurationC) { }
        protected virtual void OnKeyFrameAChanged(double oldKeyFrameA, double newKeyFrameA) { }
        protected virtual void OnKeyFrameBChanged(double oldKeyFrameB, double newKeyFrameB) { }
        protected virtual void OnOscillateChanged(bool oldOscillate, bool newOscillate) { }
        protected virtual void OnReverseDurationChanged(System.Windows.Duration oldReverseDuration, System.Windows.Duration newReverseDuration) { }
        protected virtual void OnTotalDurationChanged(System.Windows.Duration oldTotalDuration, System.Windows.Duration newTotalDuration) { }
    }
    public class FrameRateCounter : System.Windows.Controls.TextBlock
    {
        public static readonly System.Windows.DependencyProperty PrefixProperty;
        public FrameRateCounter() { }
        public string Prefix { get; set; }
    }
    public static class FrameworkElementExtensions
    {
        public static void AttachAndOpenTool(this System.Windows.FrameworkElement frameworkElement, System.Type toolType, object parameter = null) { }
        public static void AttachAndOpenTool<T>(this System.Windows.FrameworkElement frameworkElement, object parameter = null)
            where T :  class, Orc.Controls.IControlTool { }
        public static object AttachTool(this System.Windows.FrameworkElement frameworkElement, System.Type toolType) { }
        public static T AttachTool<T>(this System.Windows.FrameworkElement frameworkElement)
            where T :  class, Orc.Controls.IControlTool { }
        public static bool CanAttach(this System.Windows.FrameworkElement frameworkElement, System.Type toolType) { }
        public static bool DetachTool(this System.Windows.FrameworkElement frameworkElement, System.Type toolType) { }
        public static System.Windows.Point GetCenterPointInRoot(this System.Windows.FrameworkElement frameworkElement, System.Windows.FrameworkElement root) { }
        public static Orc.Controls.Tools.IControlToolManager GetControlToolManager(this System.Windows.FrameworkElement frameworkElement) { }
        public static System.Collections.Generic.IList<Orc.Controls.IControlTool> GetTools(this System.Windows.FrameworkElement frameworkElement) { }
        public static Orc.Controls.IEditableControl TryGetEditableControl(this System.Windows.FrameworkElement frameworkElement) { }
    }
    public class HeaderBar : System.Windows.Controls.Control
    {
        public static readonly System.Windows.DependencyProperty HeaderProperty;
        public HeaderBar() { }
        public string Header { get; set; }
    }
    public interface IApplicationLogFilterGroupService
    {
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Orc.Controls.LogFilterGroup>> LoadAsync();
        System.Threading.Tasks.Task SaveAsync(System.Collections.Generic.IEnumerable<Orc.Controls.LogFilterGroup> filterGroups);
    }
    public interface IColorLegendItem : System.ComponentModel.INotifyPropertyChanged
    {
        string AdditionalData { get; set; }
        System.Windows.Media.Color Color { get; set; }
        string Description { get; set; }
        string Id { get; set; }
        bool IsChecked { get; set; }
        bool IsSelected { get; set; }
    }
    public interface IControlTool
    {
        bool IsEnabled { get; }
        bool IsOpened { get; }
        string Name { get; }
        event System.EventHandler<System.EventArgs> Attached;
        event System.EventHandler<System.EventArgs> Closed;
        event System.EventHandler<System.EventArgs> Detached;
        event System.EventHandler<System.EventArgs> Opened;
        event System.EventHandler<System.EventArgs> Opening;
        void Attach(object target);
        void Close();
        void Detach();
        void Open(object parameter);
    }
    public interface IEditableControl
    {
        bool IsInEditMode { get; }
        event System.EventHandler<System.EventArgs> EditEnded;
        event System.EventHandler<System.EventArgs> EditStarted;
    }
    public interface ISuggestionListService
    {
        System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> GetSuggestionList(System.DateTime dateTime, Orc.Controls.DateTimePart editablePart, Orc.Controls.DateTimeFormatInfo dateTimeFormatInfo);
    }
    public interface ITimeAdjustmentProvider
    {
        Orc.Controls.TimeAdjustment GetTimeAdjustment(Orc.Controls.TimeAdjustmentStrategy strategy);
    }
    public interface IValidationContextTreeNode
    {
        System.Collections.Generic.IEnumerable<Orc.Controls.IValidationContextTreeNode> Children { get; }
        string DisplayName { get; }
        bool IsExpanded { get; set; }
        bool IsVisible { get; set; }
        Catel.Data.ValidationResultType? ResultType { get; }
    }
    public static class IValidationContextTreeNodeExtensions
    {
        public static void CollapseAll(this System.Collections.Generic.IEnumerable<Orc.Controls.IValidationContextTreeNode> nodes) { }
        public static void ExpandAll(this System.Collections.Generic.IEnumerable<Orc.Controls.IValidationContextTreeNode> nodes) { }
        public static bool HasAnyCollapsed(this System.Collections.Generic.IEnumerable<Orc.Controls.IValidationContextTreeNode> nodes) { }
        public static bool HasAnyExpanded(this System.Collections.Generic.IEnumerable<Orc.Controls.IValidationContextTreeNode> nodes) { }
        public static string ToText(this System.Collections.Generic.IEnumerable<Orc.Controls.IValidationContextTreeNode> validationContextTreeNodes) { }
    }
    public interface IValidationNamesService
    {
        void Clear();
        System.Collections.Generic.IEnumerable<Catel.Data.IValidationResult> GetCachedResultsByTagName(string tagName);
        string GetDisplayName(Catel.Data.IValidationResult validationResult);
        int? GetLineNumber(Catel.Data.IValidationResult validationResult);
        string GetTagName(Catel.Data.IValidationResult validationResult);
    }
    public class InitializingAutoCompletionServiceEventArgs : System.Windows.RoutedEventArgs
    {
        public InitializingAutoCompletionServiceEventArgs() { }
        public Catel.Services.IAutoCompletionService AutoCompletionService { get; set; }
    }
    public static class InlineExtensions
    {
        public static System.Windows.Documents.Inline Append(this System.Windows.Documents.Inline inline, System.Windows.Documents.Inline inlineToAdd) { }
        public static System.Windows.Documents.Inline AppendRange(this System.Windows.Documents.Inline inline, System.Collections.Generic.IEnumerable<System.Windows.Documents.Inline> inlines) { }
        public static System.Windows.Documents.Bold Bold(this System.Windows.Documents.Inline inline) { }
    }
    [System.Windows.StyleTypedProperty(Property="HyperlinkStyle", StyleTargetType=typeof(System.Windows.Documents.Hyperlink))]
    [System.Windows.TemplatePart(Name="PART_InnerHyperlink", Type=typeof(System.Windows.Documents.Hyperlink))]
    public class LinkLabel : System.Windows.Controls.Label
    {
        public static readonly System.Windows.DependencyProperty ClickBehaviorProperty;
        [System.ComponentModel.Category("Behavior")]
        public static readonly System.Windows.RoutedEvent ClickEvent;
        public static readonly System.Windows.DependencyProperty CommandParameterProperty;
        public static readonly System.Windows.DependencyProperty CommandProperty;
        public static readonly System.Windows.DependencyProperty CommandTargetProperty;
        public static readonly System.Windows.DependencyProperty HasUrlProperty;
        public static readonly System.Windows.DependencyProperty HoverForegroundProperty;
        public static readonly System.Windows.DependencyProperty HyperlinkStyleProperty;
        public static readonly System.Windows.DependencyProperty LinkLabelBehaviorProperty;
        [System.ComponentModel.Category("Behavior")]
        public static readonly System.Windows.RoutedEvent RequestNavigateEvent;
        public static readonly System.Windows.DependencyProperty UrlProperty;
        public LinkLabel() { }
        [System.ComponentModel.Bindable(true)]
        [System.ComponentModel.Category("Common Properties")]
        public Orc.Controls.LinkLabelClickBehavior ClickBehavior { get; set; }
        [System.ComponentModel.Bindable(true)]
        [System.ComponentModel.Category("Action")]
        [System.Windows.Localizability(System.Windows.LocalizationCategory.NeverLocalize)]
        public System.Windows.Input.ICommand Command { get; set; }
        [System.ComponentModel.Bindable(true)]
        [System.ComponentModel.Category("Action")]
        [System.Windows.Localizability(System.Windows.LocalizationCategory.NeverLocalize)]
        public object CommandParameter { get; set; }
        [System.ComponentModel.Bindable(true)]
        [System.ComponentModel.Category("Action")]
        public System.Windows.IInputElement CommandTarget { get; set; }
        public bool HasUrl { get; }
        [System.ComponentModel.Bindable(true)]
        [System.ComponentModel.Category("Brushes")]
        public System.Windows.Media.Brush HoverForeground { get; set; }
        public System.Windows.Style HyperlinkStyle { get; set; }
        [System.ComponentModel.Bindable(true)]
        [System.ComponentModel.Category("Common Properties")]
        public Orc.Controls.LinkLabelBehavior LinkLabelBehavior { get; set; }
        [System.ComponentModel.Bindable(true)]
        [System.ComponentModel.Category("Common Properties")]
        public System.Uri Url { get; set; }
        public event System.Windows.RoutedEventHandler Click;
        public event System.Windows.Navigation.RequestNavigateEventHandler RequestNavigate;
        public override void OnApplyTemplate() { }
    }
    public enum LinkLabelBehavior
    {
        SystemDefault = 0,
        AlwaysUnderline = 1,
        HoverUnderline = 2,
        NeverUnderline = 3,
    }
    public enum LinkLabelClickBehavior
    {
        Undefined = 0,
        OpenUrlInBrowser = 1,
    }
    public class ListTextBox : System.Windows.Controls.TextBox
    {
        public static readonly System.Windows.DependencyProperty ListOfValuesProperty;
        public static readonly System.Windows.DependencyProperty ValueProperty;
        public ListTextBox() { }
        public System.Collections.Generic.IList<string> ListOfValues { get; set; }
        public string Value { get; set; }
        public event System.EventHandler LeftBoundReached;
        public event System.EventHandler RightBoundReached;
        public event System.EventHandler ValueChanged;
        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e) { }
        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e) { }
    }
    public enum LoadTabItemsBehavior
    {
        LazyLoading = 0,
        LazyLoadingUnloadOthers = 1,
        EagerLoading = 2,
        EagerLoadingOnFirstUse = 3,
    }
    public class LogEntryDoubleClickEventArgs : System.EventArgs
    {
        public LogEntryDoubleClickEventArgs(Catel.Logging.LogEntry logEntry) { }
        public Catel.Logging.LogEntry LogEntry { get; }
    }
    public class LogEntryEventArgs : System.EventArgs
    {
        public LogEntryEventArgs(System.Collections.Generic.List<Catel.Logging.LogEntry> logEntries, System.Collections.Generic.List<Catel.Logging.LogEntry> filteredLogEntries) { }
        public System.Collections.Generic.List<Catel.Logging.LogEntry> FilteredLogEntries { get; }
        public System.Collections.Generic.List<Catel.Logging.LogEntry> LogEntries { get; }
    }
    public class LogFilter : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData ActionProperty;
        public static readonly Catel.Data.PropertyData ExpressionTypeProperty;
        public static readonly Catel.Data.PropertyData ExpressionValueProperty;
        public static readonly Catel.Data.PropertyData NameProperty;
        public static readonly Catel.Data.PropertyData TargetProperty;
        public LogFilter() { }
        public Orc.Controls.LogFilterAction Action { get; set; }
        public Orc.Controls.LogFilterExpressionType ExpressionType { get; set; }
        public string ExpressionValue { get; set; }
        public string Name { get; set; }
        public Orc.Controls.LogFilterTarget Target { get; set; }
        public bool Pass(Catel.Logging.LogEntry logEntry) { }
    }
    public enum LogFilterAction
    {
        Include = 0,
        Exclude = 1,
    }
    public class LogFilterEditorControl : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty LogFilterProperty;
        public LogFilterEditorControl() { }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewModelToView)]
        public Orc.Controls.LogFilter LogFilter { get; set; }
        public void InitializeComponent() { }
    }
    public class LogFilterEditorViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData ActionProperty;
        public static readonly Catel.Data.PropertyData ActionsProperty;
        public static readonly Catel.Data.PropertyData ExpressionTypeProperty;
        public static readonly Catel.Data.PropertyData ExpressionTypesProperty;
        public static readonly Catel.Data.PropertyData ExpressionValueProperty;
        public static readonly Catel.Data.PropertyData LogFilterProperty;
        public static readonly Catel.Data.PropertyData NameProperty;
        public static readonly Catel.Data.PropertyData TargetProperty;
        public static readonly Catel.Data.PropertyData TargetsProperty;
        public LogFilterEditorViewModel(Orc.Controls.LogFilter logFilter) { }
        [Catel.MVVM.ViewModelToModel("", "")]
        public Orc.Controls.LogFilterAction Action { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.LogFilterAction> Actions { get; }
        [Catel.MVVM.ViewModelToModel("", "")]
        public Orc.Controls.LogFilterExpressionType ExpressionType { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.LogFilterExpressionType> ExpressionTypes { get; }
        [Catel.MVVM.ViewModelToModel("", "")]
        public string ExpressionValue { get; set; }
        [Catel.MVVM.Model]
        public Orc.Controls.LogFilter LogFilter { get; set; }
        [Catel.MVVM.ViewModelToModel("", "")]
        public string Name { get; set; }
        [Catel.MVVM.ViewModelToModel("", "")]
        public Orc.Controls.LogFilterTarget Target { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.LogFilterTarget> Targets { get; }
        protected override void ValidateFields(System.Collections.Generic.List<Catel.Data.IFieldValidationResult> validationResults) { }
    }
    public enum LogFilterExpressionType
    {
        Contains = 0,
        NotStartsWith = 1,
        StartsWith = 2,
        NotContains = 3,
        Equals = 4,
        NotEquals = 5,
    }
    public class LogFilterGroup : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData IsEnabledProperty;
        public static readonly Catel.Data.PropertyData IsRuntimeProperty;
        public static readonly Catel.Data.PropertyData LogFiltersProperty;
        public static readonly Catel.Data.PropertyData NameProperty;
        public LogFilterGroup() { }
        public bool IsEnabled { get; set; }
        public bool IsRuntime { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.LogFilter> LogFilters { get; set; }
        public string Name { get; set; }
        public bool Pass(Catel.Logging.LogEntry logEntry) { }
        public override string ToString() { }
    }
    public class LogFilterGroupEditorViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData LogFilterGroupProperty;
        public static readonly Catel.Data.PropertyData LogFiltersProperty;
        public static readonly Catel.Data.PropertyData NameProperty;
        public static readonly Catel.Data.PropertyData SelectedLogFilterProperty;
        public LogFilterGroupEditorViewModel(Orc.Controls.LogFilterGroup logFilterGroup, Catel.Services.IMessageService messageService, Catel.Services.IUIVisualizerService uiVisualizerService) { }
        public Catel.MVVM.TaskCommand AddCommand { get; }
        public Catel.MVVM.TaskCommand EditCommand { get; }
        [Catel.MVVM.Model]
        public Orc.Controls.LogFilterGroup LogFilterGroup { get; set; }
        [Catel.MVVM.ViewModelToModel("LogFilterGroup", "")]
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.LogFilter> LogFilters { get; set; }
        [Catel.MVVM.ViewModelToModel("LogFilterGroup", "")]
        public string Name { get; set; }
        public Catel.MVVM.TaskCommand RemoveCommand { get; }
        public Orc.Controls.LogFilter SelectedLogFilter { get; set; }
        public override string Title { get; }
        protected override void OnValidating(Catel.Data.IValidationContext validationContext) { }
    }
    public class LogFilterGroupEditorWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public LogFilterGroupEditorWindow() { }
        public void InitializeComponent() { }
    }
    public class LogFilterGroupList : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public LogFilterGroupList() { }
        public event System.EventHandler<System.EventArgs> Updated;
        public void InitializeComponent() { }
        protected override void OnViewModelChanged() { }
    }
    public class LogFilterGroupListViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData FilterGroupsProperty;
        public static readonly Catel.Data.PropertyData SelectedFilterGroupProperty;
        public static readonly Catel.Data.PropertyData SelectedFilterGroupsProperty;
        public LogFilterGroupListViewModel(Orc.Controls.IApplicationLogFilterGroupService applicationLogFilterGroupService, Catel.Services.IMessageService messageService, Catel.Services.IUIVisualizerService uiVisualizerService) { }
        public Catel.MVVM.TaskCommand AddCommand { get; set; }
        public Catel.MVVM.TaskCommand EditCommand { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.LogFilterGroup> FilterGroups { get; }
        public Catel.MVVM.TaskCommand RemoveCommand { get; set; }
        public Orc.Controls.LogFilterGroup SelectedFilterGroup { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.LogFilterGroup> SelectedFilterGroups { get; }
        public event System.EventHandler<System.EventArgs> Updated;
        protected override System.Threading.Tasks.Task InitializeAsync() { }
    }
    public enum LogFilterTarget
    {
        TypeName = 0,
        AssemblyName = 1,
        LogMessage = 2,
    }
    public class LogRecord
    {
        public LogRecord() { }
        public System.DateTime DateTime { get; set; }
        public Catel.Logging.LogEvent LogEvent { get; set; }
        public string Message { get; set; }
    }
    public class LogViewerControl : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty ActiveFilterGroupProperty;
        public static readonly System.Windows.DependencyProperty AutoScrollProperty;
        public static readonly System.Windows.DependencyProperty DebugMessageBrushProperty;
        public static readonly System.Windows.DependencyProperty EnableIconsProperty;
        public static readonly System.Windows.DependencyProperty EnableTextColoringProperty;
        public static readonly System.Windows.DependencyProperty EnableThreadIdProperty;
        public static readonly System.Windows.DependencyProperty EnableTimestampProperty;
        public static readonly System.Windows.DependencyProperty ErrorMessageBrushProperty;
        public static readonly System.Windows.DependencyProperty IgnoreCatelLoggingProperty;
        public static readonly System.Windows.DependencyProperty InfoMessageBrushProperty;
        public static readonly System.Windows.DependencyProperty LogFilterProperty;
        public static readonly System.Windows.DependencyProperty LogListenerTypeProperty;
        public static readonly System.Windows.DependencyProperty MaximumUpdateBatchSizeProperty;
        public static readonly System.Windows.DependencyProperty ScrollModeProperty;
        public static readonly System.Windows.DependencyProperty ShowDebugProperty;
        public static readonly System.Windows.DependencyProperty ShowErrorProperty;
        public static readonly System.Windows.DependencyProperty ShowInfoProperty;
        public static readonly System.Windows.DependencyProperty ShowMultilineMessagesExpandedProperty;
        public static readonly System.Windows.DependencyProperty ShowWarningProperty;
        public static readonly System.Windows.DependencyProperty SupportCommandManagerProperty;
        public static readonly System.Windows.DependencyProperty WarningMessageBrushProperty;
        public LogViewerControl() { }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public Orc.Controls.LogFilterGroup ActiveFilterGroup { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool AutoScroll { get; set; }
        public System.Windows.Media.Brush DebugMessageBrush { get; set; }
        public bool EnableIcons { get; set; }
        public bool EnableTextColoring { get; set; }
        public bool EnableThreadId { get; set; }
        public bool EnableTimestamp { get; set; }
        public System.Windows.Media.Brush ErrorMessageBrush { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool IgnoreCatelLogging { get; set; }
        public System.Windows.Media.Brush InfoMessageBrush { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string LogFilter { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public System.Type LogListenerType { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public int MaximumUpdateBatchSize { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public Orc.Controls.ScrollMode ScrollMode { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowDebug { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowError { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowInfo { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowMultilineMessagesExpanded { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowWarning { get; set; }
        public bool SupportCommandManager { get; set; }
        public System.Windows.Media.Brush WarningMessageBrush { get; set; }
        public event System.EventHandler<Orc.Controls.LogEntryDoubleClickEventArgs> LogEntryDoubleClick;
        public void Clear() { }
        public void CollapseAllMultilineLogMessages() { }
        public void CopyToClipboard() { }
        public void ExpandAllMultilineLogMessages() { }
        public void InitializeComponent() { }
        protected override void OnLoaded(System.EventArgs e) { }
        protected override void OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs e) { }
        protected override void OnViewModelChanged() { }
        protected override void OnViewModelPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e) { }
    }
    public class LogViewerLogListener : Catel.Logging.RollingInMemoryLogListener
    {
        public LogViewerLogListener() { }
    }
    public static class LoggingExtensions
    {
        public static System.Collections.Generic.IEnumerable<Orc.Controls.RichTextBoxParagraph> ConvertToParagraphs(this System.Collections.Generic.IEnumerable<Catel.Logging.LogEntry> logEntries) { }
    }
    public static class MediaElementThreadFactory
    {
        public static Orc.Controls.MediaElementThreadInfo CreateMediaElementsOnWorkerThread(System.Func<System.Windows.Media.Visual> createVisual) { }
    }
    public class MediaElementThreadInfo : Catel.Disposable
    {
        public MediaElementThreadInfo(System.Windows.Media.HostVisual hostVisual, System.Func<System.Windows.Media.Visual> createVisual, System.Threading.Thread thread) { }
        public System.Func<System.Windows.Media.Visual> CreateVisual { get; }
        public System.Windows.Threading.Dispatcher Dispatcher { get; }
        public System.Windows.Media.HostVisual HostVisual { get; }
        public int Id { get; }
        public System.Threading.Thread Thread { get; }
        protected override void DisposeManaged() { }
        public void UpdateDispatcher(System.Windows.Threading.Dispatcher dispatcher) { }
    }
    public class MultipleCommandParameterConverter : System.Windows.Data.IMultiValueConverter
    {
        public MultipleCommandParameterConverter() { }
        public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) { }
        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) { }
    }
    public static class NumberFormatHelper
    {
        public static string GetFormat(int digits) { }
    }
    public class NumericTextBox : System.Windows.Controls.TextBox
    {
        public static readonly System.Windows.DependencyProperty CultureInfoProperty;
        public static readonly System.Windows.DependencyProperty FormatProperty;
        public static readonly System.Windows.DependencyProperty IsChangeValueByUpDownKeyEnabledProperty;
        public static readonly System.Windows.DependencyProperty IsDecimalAllowedProperty;
        public static readonly System.Windows.DependencyProperty IsNegativeAllowedProperty;
        public static readonly System.Windows.DependencyProperty IsNullValueAllowedProperty;
        public static readonly System.Windows.DependencyProperty MaxValueProperty;
        public static readonly System.Windows.DependencyProperty MinValueProperty;
        public static readonly System.Windows.DependencyProperty ValueProperty;
        public NumericTextBox() { }
        public System.Globalization.CultureInfo CultureInfo { get; set; }
        public string Format { get; set; }
        public bool IsChangeValueByUpDownKeyEnabled { get; set; }
        public bool IsDecimalAllowed { get; set; }
        public bool IsNegativeAllowed { get; set; }
        public bool IsNullValueAllowed { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double? Value { get; set; }
        public event System.EventHandler LeftBoundReached;
        public event System.EventHandler RightBoundReached;
        public event System.EventHandler ValueChanged;
        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e) { }
        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e) { }
    }
    public class NumericTextBoxAdapterBehavior : Catel.Windows.Interactivity.BehaviorBase<Orc.Controls.NumericTextBox>
    {
        public static readonly System.Windows.DependencyProperty ValueProperty;
        public static readonly System.Windows.DependencyProperty ValueTypeProperty;
        public NumericTextBoxAdapterBehavior() { }
        public object Value { get; set; }
        public System.Type ValueType { get; set; }
        protected virtual object Convert(double? value) { }
        protected virtual double? ConvertBack(object value) { }
        protected override void OnAssociatedObjectLoaded() { }
        protected override void OnAssociatedObjectUnloaded() { }
    }
    public class OpenFilePicker : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty BaseDirectoryProperty;
        public static readonly System.Windows.DependencyProperty FilterProperty;
        public static readonly System.Windows.DependencyProperty LabelTextProperty;
        public static readonly System.Windows.DependencyProperty LabelWidthProperty;
        public static readonly System.Windows.DependencyProperty SelectedFileProperty;
        public OpenFilePicker() { }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string BaseDirectory { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string Filter { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string LabelText { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public double LabelWidth { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string SelectedFile { get; set; }
        public void InitializeComponent() { }
    }
    public class OpenFilePickerViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData BaseDirectoryProperty;
        public static readonly Catel.Data.PropertyData FilterProperty;
        public static readonly Catel.Data.PropertyData LabelTextProperty;
        public static readonly Catel.Data.PropertyData LabelWidthProperty;
        public static readonly Catel.Data.PropertyData SelectedFileDisplayNameProperty;
        public static readonly Catel.Data.PropertyData SelectedFileProperty;
        public OpenFilePickerViewModel(Catel.Services.IOpenFileService openFileService, Catel.Services.IProcessService processService) { }
        public string BaseDirectory { get; set; }
        public Catel.MVVM.Command Clear { get; }
        public string Filter { get; set; }
        public string LabelText { get; set; }
        public double LabelWidth { get; set; }
        public Catel.MVVM.Command OpenDirectory { get; }
        public Catel.MVVM.TaskCommand SelectFile { get; }
        public string SelectedFile { get; set; }
        public string SelectedFileDisplayName { get; }
    }
    public class OpenToolCommandExtension : Catel.Windows.Markup.UpdatableMarkupExtension
    {
        public OpenToolCommandExtension(System.Type toolType, System.Type frameworkElementType) { }
        protected Catel.MVVM.Command<object> Command { get; }
        protected virtual System.Windows.FrameworkElement GetAttachmentTarget(object parameter = null) { }
        protected override object ProvideDynamicValue(System.IServiceProvider serviceProvider) { }
    }
    public class PasswordBindBehavior : Catel.Windows.Interactivity.BehaviorBase<System.Windows.Controls.PasswordBox>
    {
        public static readonly System.Windows.DependencyProperty PasswordProperty;
        public PasswordBindBehavior() { }
        public string Password { get; set; }
        protected override void OnAssociatedObjectLoaded() { }
        protected override void OnAssociatedObjectUnloaded() { }
    }
    [System.Windows.TemplatePart(Name="PART_CloseButton", Type=typeof(System.Windows.Controls.Button))]
    [System.Windows.TemplatePart(Name="PART_DragGrip", Type=typeof(System.Windows.FrameworkElement))]
    [System.Windows.TemplatePart(Name="PART_GripDrawing", Type=typeof(System.Windows.Media.GeometryDrawing))]
    [System.Windows.TemplatePart(Name="PART_PinButton", Type=typeof(System.Windows.Controls.Primitives.ToggleButton))]
    public class PinnableToolTip : System.Windows.Controls.ContentControl
    {
        public static readonly System.Windows.DependencyProperty AllowCloseByUserProperty;
        public static readonly System.Windows.DependencyProperty GripColorProperty;
        public static readonly System.Windows.DependencyProperty HorizontalOffsetProperty;
        public static readonly System.Windows.DependencyProperty IsPinnedProperty;
        public static readonly System.Windows.DependencyProperty OpenLinkCommandProperty;
        public static readonly System.Windows.DependencyProperty ResizeModeProperty;
        public static readonly System.Windows.DependencyProperty VerticalOffsetProperty;
        public PinnableToolTip() { }
        public bool AllowCloseByUser { get; set; }
        public System.Windows.Media.Color GripColor { get; set; }
        public double HorizontalOffset { get; set; }
        public bool IsOpen { get; }
        public bool IsPinned { get; set; }
        public System.Windows.Input.ICommand OpenLinkCommand { get; set; }
        public System.Windows.UIElement Owner { get; set; }
        public System.Windows.ResizeMode ResizeMode { get; set; }
        public double VerticalOffset { get; set; }
        public event System.EventHandler<System.EventArgs> IsOpenChanged;
        public event System.EventHandler<System.EventArgs> IsPinnedChanged;
        public void BringToFront() { }
        public System.Windows.Point GetPosition() { }
        public void Hide() { }
        public void IgnoreTimerStartupWhenMouseLeave(bool value) { }
        public override void OnApplyTemplate() { }
        public void SetUserDefinedAdorner(System.Windows.UIElement element) { }
        public void SetupTimer(int initialShowDelay, int showDuration) { }
        public void Show() { }
        public void StartTimer() { }
        public void StopTimer(bool reset = true) { }
    }
    public static class PinnableToolTipService
    {
        public static readonly System.Windows.DependencyProperty InitialShowDelayProperty;
        public static readonly System.Windows.DependencyProperty IsToolTipOwnerProperty;
        public static readonly System.Windows.DependencyProperty PlacementProperty;
        public static readonly System.Windows.DependencyProperty PlacementTargetProperty;
        public static readonly System.Windows.DependencyProperty ShowDurationProperty;
        public static readonly System.Windows.DependencyProperty ToolTipProperty;
        public static int GetInitialShowDelay(System.Windows.DependencyObject element) { }
        public static bool GetIsToolTipOwner(System.Windows.DependencyObject element) { }
        public static System.Windows.Controls.Primitives.PlacementMode GetPlacement(System.Windows.DependencyObject element) { }
        public static System.Windows.UIElement GetPlacementTarget(System.Windows.DependencyObject element) { }
        public static int GetShowDuration(System.Windows.DependencyObject element) { }
        public static object GetToolTip(System.Windows.DependencyObject element) { }
        public static void SetInitialShowDelay(System.Windows.DependencyObject element, int value) { }
        public static void SetIsToolTipOwner(System.Windows.DependencyObject element, bool value) { }
        public static void SetPlacement(System.Windows.DependencyObject element, System.Windows.Controls.Primitives.PlacementMode value) { }
        public static void SetPlacementTarget(System.Windows.DependencyObject element, System.Windows.UIElement value) { }
        public static void SetShowDuration(System.Windows.DependencyObject element, int value) { }
        public static void SetToolTip(System.Windows.DependencyObject element, object value) { }
    }
    public class PredefinedColor : System.IEquatable<Orc.Controls.PredefinedColor>
    {
        public string Name { get; }
        public System.Windows.Media.Color Value { get; }
        public static System.Collections.Generic.List<Orc.Controls.PredefinedColor> All { get; }
        public static System.Collections.Generic.List<Orc.Controls.PredefinedColor> AllThemeColors { get; }
        public bool Equals(Orc.Controls.PredefinedColor other) { }
        public override bool Equals(object obj) { }
        public override int GetHashCode() { }
        public static string GetColorName(System.Windows.Media.Color color) { }
        public static Orc.Controls.PredefinedColor GetPredefinedColor(System.Windows.Media.Color color) { }
        public static bool IsPredefined(System.Windows.Media.Color color) { }
        public static bool operator !=(Orc.Controls.PredefinedColor color1, Orc.Controls.PredefinedColor color2) { }
        public static bool operator ==(Orc.Controls.PredefinedColor color1, Orc.Controls.PredefinedColor color2) { }
    }
    public class PredefinedColorItem : System.Windows.Controls.Control
    {
        public static readonly System.Windows.DependencyProperty ColorProperty;
        public static readonly System.Windows.DependencyProperty TextProperty;
        public PredefinedColorItem() { }
        public PredefinedColorItem(System.Windows.Media.Color color, string text) { }
        public System.Windows.Media.Color Color { get; set; }
        public string Text { get; set; }
    }
    public static class PredefinedDateRanges
    {
        public static Orc.Controls.DateRange LastMonth { get; }
        public static Orc.Controls.DateRange LastWeek { get; }
        public static Orc.Controls.DateRange ThisMonth { get; }
        public static Orc.Controls.DateRange ThisWeek { get; }
        public static Orc.Controls.DateRange Today { get; }
        public static Orc.Controls.DateRange Yesterday { get; }
    }
    [System.Windows.TemplatePart(Name="PART_LowerSlider", Type=typeof(System.Windows.Controls.Slider))]
    [System.Windows.TemplatePart(Name="PART_SelectedRange", Type=typeof(System.Windows.Shapes.Rectangle))]
    [System.Windows.TemplatePart(Name="PART_TrackBackground", Type=typeof(System.Windows.Controls.Border))]
    [System.Windows.TemplatePart(Name="PART_UpperSlider", Type=typeof(System.Windows.Controls.Slider))]
    public class RangeSlider : System.Windows.Controls.Primitives.RangeBase
    {
        public static readonly System.Windows.DependencyProperty HighlightSelectedRangeProperty;
        public static readonly System.Windows.RoutedEvent LowerValueChangedEvent;
        public static readonly System.Windows.DependencyProperty LowerValueProperty;
        public static readonly System.Windows.DependencyProperty OrientationProperty;
        public static readonly System.Windows.RoutedEvent UpperValueChangedEvent;
        public static readonly System.Windows.DependencyProperty UpperValueProperty;
        public RangeSlider() { }
        [System.ComponentModel.Bindable(true)]
        [System.ComponentModel.Category("Behavior")]
        public bool HighlightSelectedRange { get; set; }
        [System.ComponentModel.Bindable(true)]
        [System.ComponentModel.Category("Behavior")]
        public double LowerValue { get; set; }
        [System.ComponentModel.Bindable(true)]
        [System.ComponentModel.Category("Behavior")]
        public System.Windows.Controls.Orientation Orientation { get; set; }
        [System.ComponentModel.Bindable(true)]
        [System.ComponentModel.Category("Behavior")]
        public double UpperValue { get; set; }
        [System.ComponentModel.Category("Behavior")]
        public event System.Windows.RoutedPropertyChangedEventHandler<double> LowerValueChanged;
        [System.ComponentModel.Category("Behavior")]
        public event System.Windows.RoutedPropertyChangedEventHandler<double> UpperValueChanged;
        public override void OnApplyTemplate() { }
        protected override void OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs e) { }
    }
    public class ResizingAdorner : System.Windows.Documents.Adorner
    {
        protected override int VisualChildrenCount { get; }
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize) { }
        protected override System.Windows.Media.Visual GetVisualChild(int index) { }
        public static Orc.Controls.ResizingAdorner Attach(System.Windows.FrameworkElement element) { }
        public static void Detach(Orc.Controls.ResizingAdorner adorner) { }
    }
    public static class RichTextBoxExtensions
    {
        public static string GetInlineText(this System.Windows.Controls.RichTextBox richTextBox) { }
    }
    public class RichTextBoxParagraph : System.Windows.Documents.Paragraph
    {
        public RichTextBoxParagraph(Catel.Logging.LogEntry logEntry) { }
        public Catel.Logging.LogEntry LogEntry { get; }
    }
    public static class RichTextBoxParagraphExtensions
    {
        public static void SetData(this Orc.Controls.RichTextBoxParagraph paragraph, bool showTimestamp = true, bool showThreadId = true, bool showMultilineMessagesExpanded = false) { }
    }
    public class SaveFilePicker : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty FilterProperty;
        public static readonly System.Windows.DependencyProperty LabelTextProperty;
        public static readonly System.Windows.DependencyProperty LabelWidthProperty;
        public static readonly System.Windows.DependencyProperty SelectedFileProperty;
        public SaveFilePicker() { }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string Filter { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string LabelText { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public double LabelWidth { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string SelectedFile { get; set; }
        public void InitializeComponent() { }
    }
    public class SaveFilePickerViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData FilterProperty;
        public static readonly Catel.Data.PropertyData LabelTextProperty;
        public static readonly Catel.Data.PropertyData LabelWidthProperty;
        public static readonly Catel.Data.PropertyData SelectedFileProperty;
        public SaveFilePickerViewModel(Catel.Services.ISaveFileService saveFileService, Catel.Services.IProcessService processService) { }
        public Catel.MVVM.Command Clear { get; }
        public string Filter { get; set; }
        public string LabelText { get; set; }
        public double LabelWidth { get; set; }
        public Catel.MVVM.Command OpenDirectory { get; }
        public Catel.MVVM.TaskCommand SelectFile { get; }
        public string SelectedFile { get; set; }
    }
    public enum ScrollMode
    {
        OnlyManual = 0,
        AutoScrollPriority = 1,
        ManualScrollPriority = 2,
    }
    public class StackGrid : System.Windows.Controls.Grid
    {
        public StackGrid() { }
    }
    public class StaggeredPanel : System.Windows.Controls.Panel
    {
        public static readonly System.Windows.DependencyProperty ColumnSpacingProperty;
        public static readonly System.Windows.DependencyProperty DesiredColumnWidthProperty;
        public static readonly System.Windows.DependencyProperty PaddingProperty;
        public static readonly System.Windows.DependencyProperty RowSpacingProperty;
        public StaggeredPanel() { }
        public double ColumnSpacing { get; set; }
        public double DesiredColumnWidth { get; set; }
        public System.Windows.Thickness Padding { get; set; }
        public double RowSpacing { get; set; }
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize) { }
        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize) { }
    }
    public static class StringExtensions
    {
        public static string ConstructWildcardRegex(this string pattern) { }
        public static string ExtractRegexString(this string filter) { }
        public static string GetRegexStringFromSearchPattern(this string pattern) { }
        public static bool IsValidRegexPattern(this string pattern) { }
        public static System.Windows.Documents.Inline ToInline(this string text) { }
    }
    public class SuggestionListService : Orc.Controls.ISuggestionListService
    {
        public SuggestionListService() { }
        public System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> GetSuggestionList(System.DateTime dateTime, Orc.Controls.DateTimePart editablePart, Orc.Controls.DateTimeFormatInfo dateTimeFormatInfo) { }
    }
    [System.Windows.TemplatePart(Name="PART_ItemsHolder", Type=typeof(System.Windows.Controls.Panel))]
    public class TabControl : System.Windows.Controls.TabControl
    {
        public static readonly System.Windows.DependencyProperty LoadTabItemsProperty;
        public TabControl() { }
        public bool IsLazyLoading { get; }
        public Orc.Controls.LoadTabItemsBehavior LoadTabItems { get; set; }
        protected new System.Windows.Controls.TabItem GetSelectedTabItem() { }
        public virtual void LoadTabItem(int index) { }
        public virtual void LoadTabItem(System.Windows.Controls.ContentPresenter tabItem) { }
        public override void OnApplyTemplate() { }
        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e) { }
    }
    public class TabControlItemData
    {
        public TabControlItemData(object container, object content, System.Windows.DataTemplate contentTemplate, object item) { }
        public object Container { get; }
        public object Content { get; }
        public System.Windows.DataTemplate ContentTemplate { get; }
        public object Item { get; }
        public System.Windows.Controls.TabItem TabItem { get; }
    }
    public static class TextBoxExtensions { }
    public class TimeAdjustment
    {
        public TimeAdjustment() { }
        public string Name { get; set; }
        public Orc.Controls.TimeAdjustmentStrategy Strategy { get; set; }
    }
    public class TimeAdjustmentCollectionConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public TimeAdjustmentCollectionConverter() { }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
    }
    public class TimeAdjustmentConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public TimeAdjustmentConverter() { }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
        protected override object ConvertBack(object value, System.Type targetType, object parameter) { }
    }
    public class TimeAdjustmentProvider : Orc.Controls.ITimeAdjustmentProvider
    {
        public TimeAdjustmentProvider() { }
        public Orc.Controls.TimeAdjustment GetTimeAdjustment(Orc.Controls.TimeAdjustmentStrategy strategy) { }
    }
    public enum TimeAdjustmentStrategy
    {
        AdjustEndTime = 0,
        AdjustDuration = 1,
    }
    public enum TimeSpanPart
    {
        Days = 0,
        Hours = 1,
        Minutes = 2,
        Seconds = 3,
    }
    public static class TimeSpanPartExtensions
    {
        public static System.TimeSpan CreateTimeSpan(this Orc.Controls.TimeSpanPart timeSpanPart, double value) { }
        public static string GetTimeSpanPartName(this Orc.Controls.TimeSpanPart timeSpanPart) { }
        public static double GetTimeSpanPartValue(this System.TimeSpan value, Orc.Controls.TimeSpanPart timeSpanPart) { }
    }
    [System.Windows.TemplatePart(Name="PART_DaysAbbreviationTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_DaysHoursSeparatorTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_DaysNumericTextBox", Type=typeof(Orc.Controls.NumericTextBox))]
    [System.Windows.TemplatePart(Name="PART_EditedUnitTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_EditorNumericTextBox", Type=typeof(Orc.Controls.NumericTextBox))]
    [System.Windows.TemplatePart(Name="PART_HoursAbbreviationTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_HoursMinutesSeparatorTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_HoursNumericTextBox", Type=typeof(Orc.Controls.NumericTextBox))]
    [System.Windows.TemplatePart(Name="PART_MinutesAbbreviationTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_MinutesNumericTextBox", Type=typeof(Orc.Controls.NumericTextBox))]
    [System.Windows.TemplatePart(Name="PART_MinutesSecondsSeparatorTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_NumericTBEditorContainerBorder", Type=typeof(System.Windows.Controls.Border))]
    [System.Windows.TemplatePart(Name="PART_SecondsAbbreviationTextBlock", Type=typeof(System.Windows.Controls.TextBlock))]
    [System.Windows.TemplatePart(Name="PART_SecondsNumericTextBox", Type=typeof(Orc.Controls.NumericTextBox))]
    public class TimeSpanPicker : System.Windows.Controls.Control
    {
        public static readonly System.Windows.DependencyProperty IsReadOnlyProperty;
        public static readonly System.Windows.DependencyProperty ValueProperty;
        public TimeSpanPicker() { }
        public bool IsReadOnly { get; set; }
        public System.TimeSpan? Value { get; set; }
        public override void OnApplyTemplate() { }
        protected override void OnIsKeyboardFocusedChanged(System.Windows.DependencyPropertyChangedEventArgs e) { }
        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e) { }
    }
    public static class TreeViewItemExtensions
    {
        public static int GetDepth(this System.Windows.Controls.TreeViewItem item) { }
    }
    public static class TypeExtensions
    {
        public static System.Array GetEnumValues(this System.Type enumType) { }
        public static T[] GetEnumValues<T>() { }
    }
    public class ValidationContextTreeNode : Catel.Data.ChildAwareModelBase, Orc.Controls.IValidationContextTreeNode, System.IComparable
    {
        public static readonly Catel.Data.PropertyData DisplayNameProperty;
        public static readonly Catel.Data.PropertyData IsExpandedProperty;
        public static readonly Catel.Data.PropertyData IsVisibleProperty;
        public static readonly Catel.Data.PropertyData ResultTypeProperty;
        protected ValidationContextTreeNode(bool isExpanded) { }
        public Catel.Collections.FastObservableCollection<Orc.Controls.ValidationContextTreeNode> Children { get; }
        public string DisplayName { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsVisible { get; set; }
        public Catel.Data.ValidationResultType? ResultType { get; set; }
        public void ApplyFilter(bool showErrors, bool showWarnings, string filter) { }
        public virtual int CompareTo(Orc.Controls.ValidationContextTreeNode node) { }
        public int CompareTo(object obj) { }
        protected override void OnPropertyChanged(Catel.Data.AdvancedPropertyChangedEventArgs e) { }
    }
    public class ValidationContextTreeViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData FilterProperty;
        public static readonly Catel.Data.PropertyData IsExpandedByDefaultProperty;
        public static readonly Catel.Data.PropertyData ShowErrorsProperty;
        public static readonly Catel.Data.PropertyData ShowWarningsProperty;
        public static readonly Catel.Data.PropertyData ValidationContextProperty;
        public ValidationContextTreeViewModel(Orc.Controls.IValidationNamesService validationNamesService) { }
        public string Filter { get; set; }
        public bool IsExpandedByDefault { get; set; }
        public System.Collections.Generic.IEnumerable<Orc.Controls.IValidationContextTreeNode> Nodes { get; }
        public bool ShowErrors { get; set; }
        public bool ShowWarnings { get; set; }
        public Catel.Data.IValidationContext ValidationContext { get; set; }
        public Catel.Collections.FastObservableCollection<Orc.Controls.ValidationResultTagNode> ValidationResultTags { get; }
        protected override System.Threading.Tasks.Task CloseAsync() { }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
        protected override void OnPropertyChanged(Catel.Data.AdvancedPropertyChangedEventArgs e) { }
    }
    public sealed class ValidationContextView : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty IsExpandedAllOnStartupProperty;
        public static readonly System.Windows.DependencyProperty ShowButtonsProperty;
        public static readonly System.Windows.DependencyProperty ShowFilterBoxProperty;
        public static readonly System.Windows.DependencyProperty ValidationContextProperty;
        public ValidationContextView() { }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool IsExpandedAllOnStartup { get; set; }
        public bool ShowButtons { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowFilterBox { get; set; }
        [Catel.MVVM.Views.ViewToViewModel("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public Catel.Data.IValidationContext ValidationContext { get; set; }
        public void InitializeComponent() { }
    }
    public class ValidationContextViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData ErrorsCountProperty;
        public static readonly Catel.Data.PropertyData FilterProperty;
        public static readonly Catel.Data.PropertyData IsExpandedAllOnStartupProperty;
        public static readonly Catel.Data.PropertyData IsExpandedProperty;
        public static readonly Catel.Data.PropertyData NodesProperty;
        public static readonly Catel.Data.PropertyData ShowErrorsProperty;
        public static readonly Catel.Data.PropertyData ShowFilterBoxProperty;
        public static readonly Catel.Data.PropertyData ShowWarningsProperty;
        public static readonly Catel.Data.PropertyData ValidationContextProperty;
        public static readonly Catel.Data.PropertyData ValidationResultsProperty;
        public static readonly Catel.Data.PropertyData WarningsCountProperty;
        public ValidationContextViewModel(Catel.Services.IProcessService processService) { }
        public ValidationContextViewModel(Catel.Data.ValidationContext validationContext, Catel.Services.IProcessService processService, Catel.Services.IDispatcherService dispatcherService) { }
        public Catel.MVVM.Command CollapseAll { get; }
        public Catel.MVVM.Command Copy { get; }
        public int ErrorsCount { get; }
        public Catel.MVVM.Command ExpandAll { get; }
        public string Filter { get; set; }
        public bool IsCollapsed { get; }
        public bool IsExpanded { get; }
        public bool IsExpandedAllOnStartup { get; set; }
        public System.Collections.Generic.IEnumerable<Orc.Controls.IValidationContextTreeNode> Nodes { get; set; }
        public Catel.MVVM.Command Open { get; }
        public bool ShowErrors { get; set; }
        public bool ShowFilterBox { get; set; }
        public bool ShowWarnings { get; set; }
        public Catel.Data.IValidationContext ValidationContext { get; set; }
        public System.Collections.Generic.List<Catel.Data.IValidationResult> ValidationResults { get; }
        public int WarningsCount { get; }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
        protected override void OnPropertyChanged(Catel.Data.AdvancedPropertyChangedEventArgs e) { }
    }
    public class ValidationNamesService : Orc.Controls.IValidationNamesService
    {
        public ValidationNamesService(Catel.Services.ILanguageService languageService) { }
        public void Clear() { }
        protected virtual Orc.Controls.ValidationNamesService.TagDetails ExtractTagData(Catel.Data.IValidationResult validationResult) { }
        protected virtual int? ExtractTagLine(Catel.Data.IValidationResult validationResult) { }
        protected virtual string ExtractTagName(Catel.Data.IValidationResult validationResult) { }
        public virtual System.Collections.Generic.IEnumerable<Catel.Data.IValidationResult> GetCachedResultsByTagName(string tagName) { }
        public virtual string GetDisplayName(Catel.Data.IValidationResult validationResult) { }
        public int? GetLineNumber(Catel.Data.IValidationResult validationResult) { }
        public string GetTagName(Catel.Data.IValidationResult validationResult) { }
        protected class TagDetails
        {
            public TagDetails() { }
            public int? ColumnIndex { get; set; }
            public string ColumnName { get; set; }
            public int? Line { get; set; }
        }
    }
    public class ValidationResultNode : Orc.Controls.ValidationContextTreeNode
    {
        public static readonly Catel.Data.PropertyData LineNumberProperty;
        public ValidationResultNode(Catel.Data.IValidationResult validationResult, Orc.Controls.IValidationNamesService validationNamesService, bool isExpanded) { }
        public int? LineNumber { get; }
    }
    public class ValidationResultTagNode : Orc.Controls.ValidationContextTreeNode
    {
        public ValidationResultTagNode(string tagName, bool isExpanded) { }
        public string TagName { get; }
        public override int CompareTo(Orc.Controls.ValidationContextTreeNode node) { }
        protected override void OnPropertyChanged(Catel.Data.AdvancedPropertyChangedEventArgs e) { }
        protected override void OnPropertyObjectCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) { }
    }
    public class ValidationResultTypeNode : Orc.Controls.ValidationContextTreeNode
    {
        public ValidationResultTypeNode(Catel.Data.ValidationResultType resultType, System.Collections.Generic.IEnumerable<Catel.Data.IValidationResult> validationResults, Orc.Controls.IValidationNamesService validationNamesService, bool isExpanded) { }
    }
    public class ValidationResultTypeToColorMultiValueConverter : System.Windows.Data.IMultiValueConverter
    {
        public ValidationResultTypeToColorMultiValueConverter() { }
        public System.Windows.Media.SolidColorBrush DefaultBrush { get; set; }
        public System.Windows.Media.SolidColorBrush ErrorBrush { get; set; }
        public System.Windows.Media.SolidColorBrush WarningBrush { get; set; }
        public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) { }
        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) { }
    }
    public class VisualTargetPresentationSource : System.Windows.PresentationSource
    {
        public VisualTargetPresentationSource(System.Windows.Media.HostVisual hostVisual) { }
        public override bool IsDisposed { get; }
        public override System.Windows.Media.Visual RootVisual { get; set; }
        protected override System.Windows.Media.CompositionTarget GetCompositionTargetCore() { }
    }
    [System.Windows.Markup.ContentProperty("Child")]
    public class VisualWrapper : System.Windows.FrameworkElement
    {
        public VisualWrapper() { }
        public System.Windows.Media.Visual Child { get; set; }
        protected override int VisualChildrenCount { get; }
        protected override System.Windows.Media.Visual GetVisualChild(int index) { }
    }
    public class WatermarkTextBox : System.Windows.Controls.TextBox
    {
        public static readonly System.Windows.DependencyProperty SelectAllOnGotFocusProperty;
        public static readonly System.Windows.DependencyProperty WatermarkProperty;
        public static readonly System.Windows.DependencyProperty WatermarkTemplateProperty;
        public WatermarkTextBox() { }
        public object Watermark { get; set; }
        public System.Windows.DataTemplate WatermarkTemplate { get; set; }
        protected override void OnGotKeyboardFocus(System.Windows.Input.KeyboardFocusChangedEventArgs e) { }
        protected override void OnPreviewMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e) { }
    }
    public static class WindowExtensions
    {
        public static void CenterWindowToParent(this System.Windows.Window window) { }
        public static void CenterWindowToSize(this System.Windows.Window window, System.Windows.Rect parentRect) { }
        public static void LoadWindowSize(this System.Windows.Window window, bool restoreWindowState) { }
        public static void LoadWindowSize(this System.Windows.Window window, string tag = null, bool restoreWindowState = false, bool restoreWindowPosition = true) { }
        public static void SaveWindowSize(this System.Windows.Window window) { }
        public static void SaveWindowSize(this System.Windows.Window window, string tag) { }
    }
    public class WrapPanel : System.Windows.Controls.Panel
    {
        public WrapPanel() { }
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize) { }
        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize) { }
    }
}
namespace Orc.Controls.Converters
{
    public class BooleanAndToCollapsingVisibilityMultiValueConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IMultiValueConverter
    {
        public BooleanAndToCollapsingVisibilityMultiValueConverter() { }
        public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) { }
        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) { }
        public override object ProvideValue(System.IServiceProvider serviceProvider) { }
    }
    public class TextToTextArrayMultiValueConverter : System.Windows.Data.IMultiValueConverter
    {
        public TextToTextArrayMultiValueConverter() { }
        public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) { }
        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) { }
    }
    public class TreeViewItemToLeftMarginValueConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public TreeViewItemToLeftMarginValueConverter() { }
        public double Length { get; set; }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
    }
    public class YearLongToYearShortConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public YearLongToYearShortConverter() { }
        public bool IsEnabled { get; set; }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
        protected override object ConvertBack(object value, System.Type targetType, object parameter) { }
    }
}
namespace Orc.Controls.Extensions
{
    public static class FindReplaceSettingsExtensions
    {
        public static System.Text.RegularExpressions.Regex GetRegEx(this Orc.Controls.FindReplaceSettings settings, string textToFind, bool isLeftToRight = false) { }
    }
}
namespace Orc.Controls.Services
{
    public interface IFindReplaceService
    {
        bool FindNext(string textToFind, Orc.Controls.FindReplaceSettings settings);
        string GetInitialFindText();
        bool Replace(string textToFind, string textToReplace, Orc.Controls.FindReplaceSettings settings);
        void ReplaceAll(string textToFind, string textToReplace, Orc.Controls.FindReplaceSettings settings);
    }
    public interface IStatusRepresenter
    {
        void UpdateStatus(string status);
    }
}
namespace Orc.Controls.Tools.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.All)]
    public class ToolSettingsAttribute : System.Attribute
    {
        public ToolSettingsAttribute() { }
        public string Storage { get; set; }
    }
}
namespace Orc.Controls.Tools
{
    public class ControlToolManager : Orc.Controls.Tools.IControlToolManager
    {
        public ControlToolManager(System.Windows.FrameworkElement frameworkElement, Catel.IoC.ITypeFactory typeFactory, Orc.FileSystem.IDirectoryService directoryService) { }
        public System.Collections.Generic.IList<Orc.Controls.IControlTool> Tools { get; }
        public event System.EventHandler<Orc.Controls.Tools.ToolManagementEventArgs> ToolAttached;
        public event System.EventHandler<Orc.Controls.Tools.ToolManagementEventArgs> ToolClosed;
        public event System.EventHandler<Orc.Controls.Tools.ToolManagementEventArgs> ToolDetached;
        public event System.EventHandler<Orc.Controls.Tools.ToolManagementEventArgs> ToolOpened;
        public event System.EventHandler<Orc.Controls.Tools.ToolManagementEventArgs> ToolOpening;
        public object AttachTool(System.Type toolType) { }
        public bool CanAttachTool(System.Type toolType) { }
        public bool DetachTool(System.Type toolType) { }
        protected virtual void LoadSettings(Orc.Controls.IControlTool tool) { }
        protected virtual void SaveSettings(Orc.Controls.IControlTool tool) { }
    }
    public class ControlToolManagerFactory : Orc.Controls.Tools.IControlToolManagerFactory
    {
        public ControlToolManagerFactory(Catel.IoC.ITypeFactory typeFactory) { }
        public Orc.Controls.Tools.IControlToolManager GetOrCreateManager(System.Windows.FrameworkElement frameworkElement) { }
    }
    public interface IControlToolManager
    {
        System.Collections.Generic.IList<Orc.Controls.IControlTool> Tools { get; }
        event System.EventHandler<Orc.Controls.Tools.ToolManagementEventArgs> ToolAttached;
        event System.EventHandler<Orc.Controls.Tools.ToolManagementEventArgs> ToolClosed;
        event System.EventHandler<Orc.Controls.Tools.ToolManagementEventArgs> ToolDetached;
        event System.EventHandler<Orc.Controls.Tools.ToolManagementEventArgs> ToolOpened;
        event System.EventHandler<Orc.Controls.Tools.ToolManagementEventArgs> ToolOpening;
        object AttachTool(System.Type toolType);
        bool CanAttachTool(System.Type toolType);
        bool DetachTool(System.Type toolType);
    }
    public interface IControlToolManagerFactory
    {
        Orc.Controls.Tools.IControlToolManager GetOrCreateManager(System.Windows.FrameworkElement frameworkElement);
    }
    public class ToolManagementEventArgs : System.EventArgs
    {
        public ToolManagementEventArgs(Orc.Controls.IControlTool tool) { }
        public Orc.Controls.IControlTool Tool { get; }
    }
}
namespace Orc.Controls.ViewModels
{
    public class FindReplaceViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData TextToFindForReplaceProperty;
        public static readonly Catel.Data.PropertyData TextToFindProperty;
        public FindReplaceViewModel(Orc.Controls.FindReplaceSettings findReplaceSettings, Orc.Controls.Services.IFindReplaceService findReplaceService) { }
        public Catel.MVVM.Command<string> FindNext { get; }
        [Catel.MVVM.Model]
        public Orc.Controls.FindReplaceSettings FindReplaceSettings { get; }
        public Catel.MVVM.Command<object> Replace { get; }
        public Catel.MVVM.Command<object> ReplaceAll { get; }
        public string TextToFind { get; set; }
        public string TextToFindForReplace { get; set; }
        public override string Title { get; }
    }
    public class LogViewerViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData ActiveFilterGroupProperty;
        public static readonly Catel.Data.PropertyData AutoScrollProperty;
        public static readonly Catel.Data.PropertyData DebugEntriesCountProperty;
        public static readonly Catel.Data.PropertyData ErrorEntriesCountProperty;
        public static readonly Catel.Data.PropertyData IgnoreCatelLoggingProperty;
        public static readonly Catel.Data.PropertyData InfoEntriesCountProperty;
        public static readonly Catel.Data.PropertyData LogFilterProperty;
        public static readonly Catel.Data.PropertyData LogListenerTypeProperty;
        public static readonly Catel.Data.PropertyData MaximumUpdateBatchSizeProperty;
        public static readonly Catel.Data.PropertyData ScrollModeProperty;
        public static readonly Catel.Data.PropertyData ShowDebugProperty;
        public static readonly Catel.Data.PropertyData ShowErrorProperty;
        public static readonly Catel.Data.PropertyData ShowInfoProperty;
        public static readonly Catel.Data.PropertyData ShowMultilineMessagesExpandedProperty;
        public static readonly Catel.Data.PropertyData ShowWarningProperty;
        public static readonly Catel.Data.PropertyData TypeFilterProperty;
        public static readonly Catel.Data.PropertyData TypeNamesProperty;
        public static readonly Catel.Data.PropertyData WarningEntriesCountProperty;
        public LogViewerViewModel(Catel.IoC.ITypeFactory typeFactory, Catel.Services.IDispatcherService dispatcherService, Orc.Controls.IApplicationLogFilterGroupService applicationLogFilterGroupService, Orc.Controls.LogViewerLogListener logViewerLogListener) { }
        public Orc.Controls.LogFilterGroup ActiveFilterGroup { get; set; }
        public bool AutoScroll { get; set; }
        public int DebugEntriesCount { get; }
        public int ErrorEntriesCount { get; }
        public bool IgnoreCatelLogging { get; set; }
        public int InfoEntriesCount { get; }
        public System.Collections.ObjectModel.ObservableCollection<Catel.Logging.LogEntry> LogEntries { get; }
        public string LogFilter { get; set; }
        public System.Type LogListenerType { get; set; }
        public int MaximumUpdateBatchSize { get; set; }
        public Orc.Controls.ScrollMode ScrollMode { get; set; }
        public bool ShowDebug { get; set; }
        public bool ShowError { get; set; }
        public bool ShowInfo { get; set; }
        public bool ShowMultilineMessagesExpanded { get; set; }
        public bool ShowWarning { get; set; }
        public string TypeFilter { get; set; }
        public Catel.Collections.FastObservableCollection<string> TypeNames { get; }
        public int WarningEntriesCount { get; }
        public event System.EventHandler<System.EventArgs> ActiveFilterGroupChanged;
        public event System.EventHandler<Orc.Controls.LogEntryEventArgs> LogMessage;
        public void ClearEntries() { }
        protected override System.Threading.Tasks.Task CloseAsync() { }
        public System.Collections.Generic.IEnumerable<Catel.Logging.LogEntry> GetFilteredLogEntries() { }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
        public bool IsValidLogEntry(Catel.Logging.LogEntry logEntry) { }
    }
}
namespace Orc.Controls.Views
{
    public class FindReplaceView : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public FindReplaceView() { }
        public void InitializeComponent() { }
    }
}