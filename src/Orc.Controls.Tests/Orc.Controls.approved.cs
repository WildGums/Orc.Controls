[assembly: System.Resources.NeutralResourcesLanguageAttribute("en-US")]
[assembly: System.Runtime.Versioning.TargetFrameworkAttribute(".NETFramework,Version=v4.6", FrameworkDisplayName=".NET Framework 4.6")]
[assembly: System.Windows.Markup.XmlnsDefinitionAttribute("http://schemas.wildgums.com/orc/controls", "Orc.Controls")]
[assembly: System.Windows.Markup.XmlnsDefinitionAttribute("http://schemas.wildgums.com/orc/controls", "Orc.Controls.Converters")]
[assembly: System.Windows.Markup.XmlnsPrefixAttribute("http://schemas.wildgums.com/orc/controls", "orccontrols")]
[assembly: System.Windows.ThemeInfoAttribute(System.Windows.ResourceDictionaryLocation.None, System.Windows.ResourceDictionaryLocation.SourceAssembly)]
public class static ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.Controls
{
    public class AnimatedGif : System.Windows.Controls.Image
    {
        public static readonly System.Windows.DependencyProperty GifSourceProperty;
        public AnimatedGif() { }
        public string GifSource { get; set; }
    }
    public class ApplicationLogFilterGroupService : Orc.Controls.IApplicationLogFilterGroupService
    {
        public ApplicationLogFilterGroupService(Orc.FileSystem.IFileService fileService, Catel.Runtime.Serialization.Xml.IXmlSerializer xmlSerializer) { }
        public System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Orc.Controls.LogFilterGroup>> LoadAsync() { }
        public System.Threading.Tasks.Task SaveAsync(System.Collections.Generic.IEnumerable<Orc.Controls.LogFilterGroup> filterGroups) { }
    }
    public class BindableRichTextBox : System.Windows.Controls.RichTextBox
    {
        public static readonly System.Windows.DependencyProperty BindableDocumentProperty;
        public BindableRichTextBox() { }
        public System.Windows.Documents.FlowDocument BindableDocument { get; set; }
    }
    public class BindableRun : System.Windows.Documents.Run
    {
        public static readonly System.Windows.DependencyProperty BoundTextProperty;
        public BindableRun() { }
        public string BoundText { get; set; }
    }
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
        public void OnCancelClicked() { }
        public void OnDoneClicked() { }
    }
    public class ColorChangedEventArgs : System.EventArgs
    {
        public ColorChangedEventArgs(System.Windows.Media.Color newColor, System.Windows.Media.Color oldColor) { }
        public System.Windows.Media.Color NewColor { get; set; }
        public System.Windows.Media.Color OldColor { get; set; }
    }
    public class static ColorExtensions
    {
        public static void CreateAccentColorResourceDictionary(this System.Windows.Media.Color color, string controlName) { }
    }
    public class static ColorHelper
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
    [System.Windows.TemplatePartAttribute(Name="PART_All_Visible", Type=typeof(System.Windows.Controls.CheckBox))]
    [System.Windows.TemplatePartAttribute(Name="PART_List", Type=typeof(System.Windows.Controls.ListBox))]
    [System.Windows.TemplatePartAttribute(Name="PART_Popup_Color_Board", Type=typeof(System.Windows.Controls.Primitives.Popup))]
    [System.Windows.TemplatePartAttribute(Name="PART_Settings_Button", Type=typeof(Orc.Controls.DropDownButton))]
    [System.Windows.TemplatePartAttribute(Name="PART_UnselectAll", Type=typeof(System.Windows.Controls.Primitives.ButtonBase))]
    public class ColorLegend : System.Windows.Controls.HeaderedContentControl
    {
        public static readonly System.Windows.DependencyProperty AccentColorBrushProperty;
        public static readonly System.Windows.DependencyProperty AllowColorEditingProperty;
        public static readonly System.Windows.DependencyProperty EditingColorProperty;
        public static readonly System.Windows.DependencyProperty FilteredItemsIdsProperty;
        public static readonly System.Windows.DependencyProperty FilteredItemsSourceProperty;
        public static readonly System.Windows.DependencyProperty FilterProperty;
        public static readonly System.Windows.DependencyProperty FilterWatermarkProperty;
        public static readonly System.Windows.DependencyProperty IsAllVisibleProperty;
        public static readonly System.Windows.DependencyProperty IsColorSelectingProperty;
        public static readonly System.Windows.DependencyProperty ItemsSourceProperty;
        public static readonly System.Windows.DependencyProperty SelectedColorItemsProperty;
        public static readonly System.Windows.DependencyProperty ShowBottomToolBoxProperty;
        public static readonly System.Windows.DependencyProperty ShowColorVisibilityControlsProperty;
        public static readonly System.Windows.DependencyProperty ShowSearchBoxProperty;
        public static readonly System.Windows.DependencyProperty ShowSettingsBoxProperty;
        public static readonly System.Windows.DependencyProperty ShowToolBoxProperty;
        public ColorLegend() { }
        public System.Windows.Media.Brush AccentColorBrush { get; set; }
        public bool AllowColorEditing { get; set; }
        public Catel.MVVM.Command<object> ChangeColor { get; }
        public System.Windows.Media.Color EditingColor { get; set; }
        public string Filter { get; set; }
        public System.Collections.Generic.IEnumerable<string> FilteredItemsIds { get; set; }
        public System.Collections.Generic.IEnumerable<Orc.Controls.IColorLegendItem> FilteredItemsSource { get; set; }
        public string FilterWatermark { get; set; }
        public System.Nullable<bool> IsAllVisible { get; set; }
        public bool IsColorSelecting { get; set; }
        public System.Collections.Generic.IEnumerable<Orc.Controls.IColorLegendItem> ItemsSource { get; set; }
        public System.Collections.Generic.IEnumerable<Orc.Controls.IColorLegendItem> SelectedColorItems { get; set; }
        public bool ShowBottomToolBox { get; set; }
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
        public void UpdateVisibilityControlsVisibility() { }
    }
    [Catel.Runtime.Serialization.SerializerModifierAttribute(typeof(Orc.Controls.ColorLegendItemSerializerModifier))]
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
    [System.Windows.TemplatePartAttribute(Name="PART_Popup", Type=typeof(System.Windows.Controls.Primitives.Popup))]
    [System.Windows.TemplatePartAttribute(Name="PART_ToggleDropDown", Type=typeof(System.Windows.Controls.Primitives.ToggleButton))]
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
    public enum ConnectionState
    {
        Undefined = 0,
        Valid = 1,
        Invalid = 2,
    }
    public class ConnectionStateToColorBrushValueConverter : Catel.MVVM.Converters.ValueConverterBase<Orc.Controls.ConnectionState, System.Windows.Media.SolidColorBrush>
    {
        public ConnectionStateToColorBrushValueConverter() { }
        protected override object Convert(Orc.Controls.ConnectionState value, System.Type targetType, object parameter) { }
    }
    public class ConnectionStringAdvancedOptionsViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData ConnectionStringPropertiesProperty;
        public ConnectionStringAdvancedOptionsViewModel(Orc.Controls.SqlConnectionString connectionString) { }
        public Orc.Controls.SqlConnectionString ConnectionString { get; }
        public System.Collections.Generic.IList<Orc.Controls.ConnectionStringProperty> ConnectionStringProperties { get; }
        public override string Title { get; }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
    }
    public sealed class ConnectionStringAdvancedOptionsWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public ConnectionStringAdvancedOptionsWindow() { }
        public void InitializeComponent() { }
    }
    public sealed class ConnectionStringBuilder : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty AccentColorBrushProperty;
        public static readonly System.Windows.DependencyProperty ConnectionStateProperty;
        public static readonly System.Windows.DependencyProperty ConnectionStringProperty;
        public static readonly System.Windows.DependencyProperty IsInEditModeProperty;
        public ConnectionStringBuilder() { }
        public System.Windows.Media.Brush AccentColorBrush { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewModelToView)]
        public Orc.Controls.ConnectionState ConnectionState { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewModelToView)]
        public string ConnectionString { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewModelToView)]
        public bool IsInEditMode { get; set; }
        public void InitializeComponent() { }
        public override void OnApplyTemplate() { }
    }
    public class ConnectionStringBuilderService : Orc.Controls.IConnectionStringBuilderService
    {
        public ConnectionStringBuilderService(Orc.Controls.IConnectionStringBuilderServiceInitializer connectionStringBuilderServiceInitializer) { }
        public void AddDataSourceProvider(string invariantName, Orc.Controls.IDataSourceProvider provider) { }
        public Orc.Controls.SqlConnectionString CreateConnectionString(Orc.Controls.DbProvider dbProvider, string connectionString = "") { }
        public Orc.Controls.ConnectionState GetConnectionState(Orc.Controls.SqlConnectionString connectionString) { }
        public System.Collections.Generic.IList<string> GetDatabases(Orc.Controls.SqlConnectionString connectionString) { }
        public System.Collections.Generic.IList<string> GetDataSources(Orc.Controls.SqlConnectionString connectionString) { }
    }
    public class ConnectionStringBuilderServiceInitializer : Orc.Controls.IConnectionStringBuilderServiceInitializer
    {
        public ConnectionStringBuilderServiceInitializer(Catel.IoC.ITypeFactory typeFactory) { }
        public void Initialize(Orc.Controls.IConnectionStringBuilderService connectionStringBuilderService) { }
    }
    public class ConnectionStringBuilderViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData ConnectionStateProperty;
        public static readonly Catel.Data.PropertyData ConnectionStringProperty;
        public static readonly Catel.Data.PropertyData DisplayConnectionStringProperty;
        public static readonly Catel.Data.PropertyData IsInEditModeProperty;
        public ConnectionStringBuilderViewModel(Catel.Services.IUIVisualizerService uiVisualizerService, Catel.IoC.ITypeFactory typeFactory) { }
        public Catel.MVVM.Command Clear { get; }
        public Orc.Controls.ConnectionState ConnectionState { get; set; }
        public string ConnectionString { get; }
        public string DisplayConnectionString { get; }
        public Catel.MVVM.TaskCommand Edit { get; }
        public bool IsInEditMode { get; set; }
    }
    public class ConnectionStringEditViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData ConnectionStateProperty;
        public static readonly Catel.Data.PropertyData ConnectionStringProperty;
        public static readonly Catel.Data.PropertyData DbProviderProperty;
        public static readonly Catel.Data.PropertyData IsDatabaseListVisibleProperty;
        public static readonly Catel.Data.PropertyData IsDatabasesRefreshingProperty;
        public static readonly Catel.Data.PropertyData IsServerListVisibleProperty;
        public static readonly Catel.Data.PropertyData IsServersRefreshingProperty;
        public ConnectionStringEditViewModel(string connectionString, Orc.Controls.DbProvider provider, Catel.Services.IMessageService messageService, Orc.Controls.IConnectionStringBuilderService connectionStringBuilderService, Catel.Services.IUIVisualizerService uiVisualizerService, Catel.IoC.ITypeFactory typeFactory) { }
        public bool CanLogOnToServer { get; }
        public Orc.Controls.ConnectionState ConnectionState { get; }
        public Orc.Controls.SqlConnectionString ConnectionString { get; }
        public Catel.Collections.FastObservableCollection<string> Databases { get; }
        public Orc.Controls.ConnectionStringProperty DataSource { get; }
        public Orc.Controls.DbProvider DbProvider { get; set; }
        public Catel.MVVM.Command InitDatabases { get; }
        public Orc.Controls.ConnectionStringProperty InitialCatalog { get; }
        public Catel.MVVM.Command InitServers { get; }
        public Orc.Controls.ConnectionStringProperty IntegratedSecurity { get; }
        public System.Nullable<bool> IntegratedSecurityValue { get; set; }
        public bool IsDatabaseListVisible { get; set; }
        public bool IsDatabasesRefreshing { get; }
        public bool IsLogOnEnabled { get; }
        public bool IsServerListVisible { get; set; }
        public bool IsServersRefreshing { get; }
        public Orc.Controls.ConnectionStringProperty Password { get; }
        public Catel.MVVM.Command RefreshDatabases { get; }
        public Catel.MVVM.Command RefreshServers { get; }
        public Catel.Collections.FastObservableCollection<string> Servers { get; }
        public Catel.MVVM.TaskCommand ShowAdvancedOptions { get; }
        public Catel.MVVM.Command TestConnection { get; }
        public override string Title { get; }
        public Orc.Controls.ConnectionStringProperty UserId { get; }
        public bool CanInitDatabases() { }
        protected override void OnPropertyChanged(Catel.Data.AdvancedPropertyChangedEventArgs e) { }
    }
    public sealed class ConnectionStringEditWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public ConnectionStringEditWindow() { }
        public void InitializeComponent() { }
    }
    public class ConnectionStringProperty : Catel.Data.ObservableObject
    {
        public ConnectionStringProperty(string name, bool isSensitive, System.Data.Common.DbConnectionStringBuilder dbConnectionStringBuilder) { }
        public bool IsSensitive { get; }
        public string Name { get; }
        public object Value { get; set; }
    }
    public sealed class CulturePicker : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty SelectedCultureProperty;
        public CulturePicker() { }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public System.Globalization.CultureInfo SelectedCulture { get; set; }
        public void InitializeComponent() { }
    }
    public class DatePicker : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty AccentColorBrushProperty;
        public static readonly System.Windows.DependencyProperty AllowCopyPasteProperty;
        public static readonly System.Windows.DependencyProperty AllowNullProperty;
        public static readonly System.Windows.DependencyProperty FormatProperty;
        public static readonly System.Windows.DependencyProperty IsReadOnlyProperty;
        public static readonly System.Windows.DependencyProperty IsYearShortFormatProperty;
        public static readonly System.Windows.DependencyProperty ShowOptionsButtonProperty;
        public static readonly System.Windows.DependencyProperty ValueProperty;
        public DatePicker() { }
        public System.Windows.Media.Brush AccentColorBrush { get; set; }
        public bool AllowCopyPaste { get; set; }
        public bool AllowNull { get; set; }
        public string Format { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsYearShortFormat { get; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowOptionsButton { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public System.Nullable<System.DateTime> Value { get; set; }
        public void InitializeComponent() { }
        public override void OnApplyTemplate() { }
        protected override void OnLoaded(System.EventArgs e) { }
        protected override void OnUnloaded(System.EventArgs e) { }
    }
    public class DatePickerViewModel : Catel.MVVM.ViewModelBase
    {
        public DatePickerViewModel() { }
        public System.Nullable<int> Day { get; set; }
        public System.Nullable<int> Month { get; set; }
        public bool ShowOptionsButton { get; set; }
        public System.Nullable<System.DateTime> Value { get; set; }
        public System.Nullable<int> Year { get; set; }
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
        public static readonly System.Windows.DependencyProperty IsReadOnlyProperty;
        public static readonly System.Windows.DependencyProperty RangesProperty;
        public static readonly System.Windows.DependencyProperty SelectedRangeProperty;
        public static readonly System.Windows.DependencyProperty ShowOptionsButtonProperty;
        public static readonly System.Windows.DependencyProperty SpanProperty;
        public static readonly System.Windows.DependencyProperty StartDateProperty;
        public DateRangePicker() { }
        public bool AllowCopyPaste { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public System.DateTime EndDate { get; set; }
        public string Format { get; set; }
        public bool HideSeconds { get; set; }
        public bool HideTime { get; set; }
        public bool IsReadOnly { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.DateRange> Ranges { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public Orc.Controls.DateRange SelectedRange { get; set; }
        public bool ShowOptionsButton { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public System.TimeSpan Span { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public System.DateTime StartDate { get; set; }
        public void InitializeComponent() { }
    }
    public class DateRangePickerViewModel : Catel.MVVM.ViewModelBase
    {
        public DateRangePickerViewModel() { }
        public System.DateTime EndDate { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.DateRange> Ranges { get; set; }
        public Orc.Controls.DateRange SelectedRange { get; set; }
        public System.TimeSpan Span { get; set; }
        public System.DateTime StartDate { get; set; }
    }
    public class static DateTimeFormatHelper
    {
        public static Orc.Controls.DateTimeFormatInfo GetDateTimeFormatInfo(string format, bool isDateOnly = False) { }
        public static string[] Split(string format, char[] formatCharacters) { }
    }
    public class DateTimeFormatInfo
    {
        public DateTimeFormatInfo() { }
        public string AmPmFormat { get; set; }
        public System.Nullable<int> AmPmPosition { get; set; }
        public string DayFormat { get; set; }
        public int DayPosition { get; set; }
        public string HourFormat { get; set; }
        public System.Nullable<int> HourPosition { get; set; }
        public System.Nullable<bool> IsAmPmShortFormat { get; set; }
        public bool IsDateOnly { get; }
        public System.Nullable<bool> IsHour12Format { get; set; }
        public bool IsYearShortFormat { get; set; }
        public int MaxPosition { get; set; }
        public string MinuteFormat { get; set; }
        public System.Nullable<int> MinutePosition { get; set; }
        public string MonthFormat { get; set; }
        public int MonthPosition { get; set; }
        public string SecondFormat { get; set; }
        public System.Nullable<int> SecondPosition { get; set; }
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
    public class static DateTimeFormatter
    {
        public static string Format(System.DateTime dateTime, string format, bool isDateOnly = False) { }
    }
    public class static DateTimeParser
    {
        public static System.DateTime Parse(string input, string format, bool isDateOnly = False) { }
        public static bool TryParse(string input, string format, out System.DateTime dateTime, bool isDateOnly = False) { }
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
    public class static DateTimePartExtensions
    {
        public static string GetDateTimePartName(this Orc.Controls.DateTimePart dateTimePart) { }
        public static string GetDateTimePartToggleButtonName(this Orc.Controls.DateTimePart dateTimePart) { }
    }
    public class DateTimePartHelper
    {
        public DateTimePartHelper(System.DateTime dateTime, Orc.Controls.DateTimePart dateTimePart, Orc.Controls.DateTimeFormatInfo dateTimeFormatInfo, System.Windows.Controls.TextBox textBox, System.Windows.Controls.Primitives.ToggleButton activeToggleButton) { }
        public System.Windows.Controls.Primitives.Popup CreatePopup() { }
    }
    public class DateTimePicker : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty AccentColorBrushProperty;
        public static readonly System.Windows.DependencyProperty AllowCopyPasteProperty;
        public static readonly System.Windows.DependencyProperty AllowNullProperty;
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
        public System.Windows.Media.Brush AccentColorBrush { get; set; }
        public bool AllowCopyPaste { get; set; }
        public bool AllowNull { get; set; }
        public string Format { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool HideSeconds { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool HideTime { get; set; }
        public bool IsAmPmShortFormat { get; }
        public bool IsHour12Format { get; }
        public bool IsReadOnly { get; set; }
        public bool IsYearShortFormat { get; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowOptionsButton { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public System.Nullable<System.DateTime> Value { get; set; }
        public void InitializeComponent() { }
        public override void OnApplyTemplate() { }
        protected override void OnLoaded(System.EventArgs e) { }
        protected override void OnUnloaded(System.EventArgs e) { }
    }
    public class DateTimePickerViewModel : Catel.MVVM.ViewModelBase
    {
        public DateTimePickerViewModel() { }
        public string AmPm { get; set; }
        public System.Nullable<int> Day { get; set; }
        public bool HideSeconds { get; set; }
        public bool HideTime { get; set; }
        public System.Nullable<int> Hour { get; set; }
        public System.Nullable<int> Minute { get; set; }
        public System.Nullable<int> Month { get; set; }
        public System.Nullable<int> Second { get; set; }
        public bool ShowOptionsButton { get; set; }
        public System.Nullable<System.DateTime> Value { get; set; }
        public System.Nullable<int> Year { get; set; }
    }
    public class DbConnectionProviderListViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData DbProviderProperty;
        public static readonly Catel.Data.PropertyData DbProvidersProperty;
        public DbConnectionProviderListViewModel(Orc.Controls.DbProvider selectedProvider) { }
        public Orc.Controls.DbProvider DbProvider { get; set; }
        public System.Collections.Generic.IList<Orc.Controls.DbProvider> DbProviders { get; }
        public Catel.MVVM.Command Open { get; }
        public Catel.MVVM.Command Refresh { get; }
        public override string Title { get; }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
    }
    public sealed class DbConnectionProviderListWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public DbConnectionProviderListWindow() { }
        public void InitializeComponent() { }
    }
    public class DbProvider
    {
        public DbProvider() { }
        public string Description { get; set; }
        public string InvariantName { get; set; }
        public string Name { get; set; }
        protected bool Equals(Orc.Controls.DbProvider other) { }
        public override bool Equals(object obj) { }
        public override int GetHashCode() { }
    }
    public sealed class DbProviderPicker : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty DbProviderProperty;
        public DbProviderPicker() { }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewModelWins)]
        public Orc.Controls.DbProvider DbProvider { get; set; }
        public void InitializeComponent() { }
    }
    public class DbProviderPickerViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData DbProviderProperty;
        public DbProviderPickerViewModel(Catel.Services.IUIVisualizerService uiVisualizerService, Catel.IoC.ITypeFactory typeFactory) { }
        public Catel.MVVM.TaskCommand ChangeDbProvider { get; }
        public Orc.Controls.DbProvider DbProvider { get; set; }
    }
    public class static DependencyObjectExtensions
    {
        public static System.Collections.Generic.IEnumerable<System.Windows.DependencyObject> GetDescendents(this System.Windows.DependencyObject root) { }
        public static System.Windows.DependencyObject GetVisualRoot(this System.Windows.DependencyObject dependencyObject) { }
    }
    public class DirectoryPicker : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty LabelTextProperty;
        public static readonly System.Windows.DependencyProperty LabelWidthProperty;
        public static readonly System.Windows.DependencyProperty SelectedDirectoryProperty;
        public DirectoryPicker() { }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string LabelText { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public double LabelWidth { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string SelectedDirectory { get; set; }
        public void InitializeComponent() { }
    }
    public class DirectoryPickerViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData LabelTextProperty;
        public static readonly Catel.Data.PropertyData LabelWidthProperty;
        public static readonly Catel.Data.PropertyData SelectedDirectoryProperty;
        public DirectoryPickerViewModel(Catel.Services.ISelectDirectoryService selectDirectoryService, Catel.Services.IProcessService processService) { }
        public string LabelText { get; set; }
        public double LabelWidth { get; set; }
        public Catel.MVVM.Command OpenDirectory { get; }
        public Catel.MVVM.TaskCommand SelectDirectory { get; }
        public string SelectedDirectory { get; set; }
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
    public class DropDownButton : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty AccentColorBrushProperty;
        public static readonly System.Windows.DependencyProperty ArrowLocationProperty;
        public static readonly System.Windows.DependencyProperty ArrowMarginProperty;
        public static readonly System.Windows.DependencyProperty CommandParameterProperty;
        public static readonly System.Windows.DependencyProperty CommandProperty;
        public static readonly System.Windows.DependencyProperty DropDownProperty;
        public static readonly System.Windows.DependencyProperty EnableTransparentBackgroundProperty;
        public static readonly System.Windows.DependencyProperty HeaderProperty;
        public static readonly System.Windows.DependencyProperty ShowDefaultButtonProperty;
        public DropDownButton() { }
        public System.Windows.Media.Brush AccentColorBrush { get; set; }
        public Orc.Controls.DropdownArrowLocation ArrowLocation { get; set; }
        public System.Windows.Thickness ArrowMargin { get; set; }
        public System.Windows.Input.ICommand Command { get; set; }
        public object CommandParameter { get; set; }
        public System.Windows.Controls.ContextMenu DropDown { get; set; }
        public bool EnableTransparentBackground { get; set; }
        public object Header { get; set; }
        public bool ShowDefaultButton { get; set; }
        public event System.EventHandler<System.EventArgs> ContentLayoutUpdated;
        public void InitializeComponent() { }
        public override void OnApplyTemplate() { }
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
    public enum ExpandDirection
    {
        Down = 0,
        Up = 1,
        Left = 2,
        Right = 3,
    }
    public class Expander : System.Windows.Controls.HeaderedContentControl
    {
        public static readonly System.Windows.DependencyProperty AccentColorBrushProperty;
        public static readonly System.Windows.DependencyProperty AutoResizeGridProperty;
        public static readonly System.Windows.DependencyProperty ExpandDirectionProperty;
        public static readonly System.Windows.DependencyProperty IsExpandedProperty;
        public Expander() { }
        public System.Windows.Media.Brush AccentColorBrush { get; set; }
        public bool AutoResizeGrid { get; set; }
        public Orc.Controls.ExpandDirection ExpandDirection { get; set; }
        public bool IsExpanded { get; set; }
        public override void OnApplyTemplate() { }
        protected virtual void OnCollapsed() { }
        protected virtual void OnExpanded() { }
    }
    [System.Windows.TemplatePartAttribute(Name="PART_ClearButton", Type=typeof(System.Windows.Controls.Button))]
    [System.Windows.TemplatePartAttribute(Name="PART_FilterTextBox", Type=typeof(System.Windows.Controls.TextBox))]
    public class FilterBox : System.Windows.Controls.ContentControl
    {
        public static readonly System.Windows.DependencyProperty AccentColorBrushProperty;
        public static readonly System.Windows.DependencyProperty AllowAutoCompletionProperty;
        public static readonly System.Windows.DependencyProperty FilterSourceProperty;
        public static readonly System.Windows.DependencyProperty PropertyNameProperty;
        public static readonly System.Windows.DependencyProperty TextProperty;
        public static readonly System.Windows.DependencyProperty WatermarkProperty;
        public FilterBox() { }
        public System.Windows.Media.Brush AccentColorBrush { get; set; }
        public bool AllowAutoCompletion { get; set; }
        public System.Collections.IEnumerable FilterSource { get; set; }
        public string PropertyName { get; set; }
        public string Text { get; set; }
        public string Watermark { get; set; }
        public event System.EventHandler<Orc.Controls.InitializingAutoCompletionServiceEventArgs> InitializingAutoCompletionService;
        public override void OnApplyTemplate() { }
        protected override void OnGotFocus(System.Windows.RoutedEventArgs e) { }
        protected virtual void OnInitializingAutoCompletionService(Orc.Controls.InitializingAutoCompletionServiceEventArgs e) { }
        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e) { }
    }
    public class FrameRateCounter : System.Windows.Controls.TextBlock
    {
        public static readonly System.Windows.DependencyProperty PrefixProperty;
        public FrameRateCounter() { }
        public string Prefix { get; set; }
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
    public interface IConnectionStringBuilderService
    {
        void AddDataSourceProvider(string invariantName, Orc.Controls.IDataSourceProvider provider);
        Orc.Controls.SqlConnectionString CreateConnectionString(Orc.Controls.DbProvider dbProvider, string connectionString = "");
        Orc.Controls.ConnectionState GetConnectionState(Orc.Controls.SqlConnectionString connectionString);
        System.Collections.Generic.IList<string> GetDatabases(Orc.Controls.SqlConnectionString connectionString);
        System.Collections.Generic.IList<string> GetDataSources(Orc.Controls.SqlConnectionString connectionString);
    }
    public interface IConnectionStringBuilderServiceInitializer
    {
        void Initialize(Orc.Controls.IConnectionStringBuilderService connectionStringBuilderService);
    }
    public interface IDataSourceProvider
    {
        string DataBasesQuery { get; }
        System.Collections.Generic.IList<string> GetDataSources();
    }
    public class InitializingAutoCompletionServiceEventArgs : System.Windows.RoutedEventArgs
    {
        public InitializingAutoCompletionServiceEventArgs() { }
        public Catel.Services.IAutoCompletionService AutoCompletionService { get; set; }
    }
    public class static InlineExtensions
    {
        public static System.Windows.Documents.Inline Append(this System.Windows.Documents.Inline inline, System.Windows.Documents.Inline inlineToAdd) { }
        public static System.Windows.Documents.Inline AppendRange(this System.Windows.Documents.Inline inline, System.Collections.Generic.IEnumerable<System.Windows.Documents.Inline> inlines) { }
        public static System.Windows.Documents.Bold Bold(this System.Windows.Documents.Inline inline) { }
    }
    public interface ISuggestionListService
    {
        System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> GetSuggestionList(System.DateTime dateTime, Orc.Controls.DateTimePart editablePart, Orc.Controls.DateTimeFormatInfo dateTimeFormatInfo);
    }
    public interface IValidationContextTreeNode
    {
        System.Collections.Generic.IEnumerable<Orc.Controls.IValidationContextTreeNode> Children { get; }
        string DisplayName { get; }
        bool IsExpanded { get; set; }
        bool IsVisible { get; set; }
        System.Nullable<Catel.Data.ValidationResultType> ResultType { get; }
    }
    public class static IValidationContextTreeNodeExtensions
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
        System.Nullable<int> GetLineNumber(Catel.Data.IValidationResult validationResult);
        string GetTagName(Catel.Data.IValidationResult validationResult);
    }
    [System.Windows.TemplatePartAttribute(Name="PART_InnerHyperlink", Type=typeof(System.Windows.Documents.Hyperlink))]
    public class LinkLabel : System.Windows.Controls.Label
    {
        public static readonly System.Windows.DependencyProperty ClickBehaviorProperty;
        [System.ComponentModel.CategoryAttribute("Behavior")]
        public static readonly System.Windows.RoutedEvent ClickEvent;
        public static readonly System.Windows.DependencyProperty CommandParameterProperty;
        public static readonly System.Windows.DependencyProperty CommandProperty;
        public static readonly System.Windows.DependencyProperty CommandTargetProperty;
        public static readonly System.Windows.DependencyProperty HasUrlProperty;
        public static readonly System.Windows.DependencyProperty HoverForegroundProperty;
        public static readonly System.Windows.DependencyProperty HyperlinkStyleProperty;
        public static readonly System.Windows.DependencyProperty LinkLabelBehaviorProperty;
        [System.ComponentModel.CategoryAttribute("Behavior")]
        public static readonly System.Windows.RoutedEvent RequestNavigateEvent;
        public static readonly System.Windows.DependencyProperty UrlProperty;
        public LinkLabel() { }
        [System.ComponentModel.BindableAttribute(true)]
        [System.ComponentModel.CategoryAttribute("Common Properties")]
        public Orc.Controls.LinkLabelClickBehavior ClickBehavior { get; set; }
        [System.ComponentModel.BindableAttribute(true)]
        [System.ComponentModel.CategoryAttribute("Action")]
        [System.Windows.LocalizabilityAttribute(System.Windows.LocalizationCategory.NeverLocalize)]
        public System.Windows.Input.ICommand Command { get; set; }
        [System.ComponentModel.BindableAttribute(true)]
        [System.ComponentModel.CategoryAttribute("Action")]
        [System.Windows.LocalizabilityAttribute(System.Windows.LocalizationCategory.NeverLocalize)]
        public object CommandParameter { get; set; }
        [System.ComponentModel.BindableAttribute(true)]
        [System.ComponentModel.CategoryAttribute("Action")]
        public System.Windows.IInputElement CommandTarget { get; set; }
        public bool HasUrl { get; }
        [System.ComponentModel.BindableAttribute(true)]
        [System.ComponentModel.CategoryAttribute("Brushes")]
        public System.Windows.Media.Brush HoverForeground { get; set; }
        public System.Windows.Style HyperlinkStyle { get; set; }
        [System.ComponentModel.BindableAttribute(true)]
        [System.ComponentModel.CategoryAttribute("Common Properties")]
        public Orc.Controls.LinkLabelBehavior LinkLabelBehavior { get; set; }
        [System.ComponentModel.BindableAttribute(true)]
        [System.ComponentModel.CategoryAttribute("Common Properties")]
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
    public class LinkToolTipItem
    {
        public LinkToolTipItem() { }
        public string Key { get; set; }
        public string Value { get; set; }
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
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.ViewModelToView)]
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
        [Catel.MVVM.ViewModelToModelAttribute("", "")]
        public Orc.Controls.LogFilterAction Action { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.LogFilterAction> Actions { get; }
        [Catel.MVVM.ViewModelToModelAttribute("", "")]
        public Orc.Controls.LogFilterExpressionType ExpressionType { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.LogFilterExpressionType> ExpressionTypes { get; }
        [Catel.MVVM.ViewModelToModelAttribute("", "")]
        public string ExpressionValue { get; set; }
        [Catel.MVVM.ModelAttribute()]
        public Orc.Controls.LogFilter LogFilter { get; set; }
        [Catel.MVVM.ViewModelToModelAttribute("", "")]
        public string Name { get; set; }
        [Catel.MVVM.ViewModelToModelAttribute("", "")]
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
        public static readonly Catel.Data.PropertyData LogFiltersProperty;
        public static readonly Catel.Data.PropertyData NameProperty;
        public LogFilterGroup() { }
        public bool IsEnabled { get; set; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.LogFilter> LogFilters { get; set; }
        public string Name { get; set; }
        public bool Pass(Catel.Logging.LogEntry logEntry) { }
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
        [Catel.MVVM.ModelAttribute()]
        public Orc.Controls.LogFilterGroup LogFilterGroup { get; set; }
        [Catel.MVVM.ViewModelToModelAttribute("LogFilterGroup", "")]
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.LogFilter> LogFilters { get; set; }
        [Catel.MVVM.ViewModelToModelAttribute("LogFilterGroup", "")]
        public string Name { get; set; }
        public Catel.MVVM.TaskCommand RemoveCommand { get; set; }
        public Orc.Controls.LogFilter SelectedLogFilter { get; set; }
        protected override void OnValidating(Catel.Data.IValidationContext validationContext) { }
    }
    public class LogFilterGroupEditorWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public LogFilterGroupEditorWindow() { }
        public void InitializeComponent() { }
    }
    public class LogFilterGroupListControl : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public LogFilterGroupListControl() { }
        public void InitializeComponent() { }
    }
    public class LogFilterGroupListControlViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData FilterGroupsProperty;
        public static readonly Catel.Data.PropertyData SelectedFilterGroupProperty;
        public LogFilterGroupListControlViewModel(Orc.Controls.IApplicationLogFilterGroupService applicationLogFilterGroupService, Catel.Services.IMessageService messageService, Catel.Services.IUIVisualizerService uIVisualizerService) { }
        public Catel.MVVM.TaskCommand AddCommand { get; set; }
        public Catel.MVVM.TaskCommand EditCommand { get; set; }
        public Catel.MVVM.Command EnableOrDisableCommand { get; set; }
        public string EnableOrDisableCommandText { get; }
        public System.Collections.ObjectModel.ObservableCollection<Orc.Controls.LogFilterGroup> FilterGroups { get; set; }
        public bool IsFilterGroupSelected { get; }
        public Catel.MVVM.TaskCommand RemoveCommand { get; set; }
        public Orc.Controls.LogFilterGroup SelectedFilterGroup { get; set; }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
        protected override void OnPropertyChanged(Catel.Data.AdvancedPropertyChangedEventArgs e) { }
    }
    public enum LogFilterTarget
    {
        TypeName = 0,
        AssemblyName = 1,
    }
    public class static LoggingExtensions
    {
        public static System.Collections.Generic.IEnumerable<Orc.Controls.RichTextBoxParagraph> ConvertToParagraphs(this System.Collections.Generic.IEnumerable<Catel.Logging.LogEntry> logEntries) { }
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
        public static readonly System.Windows.DependencyProperty AutoScrollProperty;
        public static readonly System.Windows.DependencyProperty EnableIconsProperty;
        public static readonly System.Windows.DependencyProperty EnableTextColoringProperty;
        public static readonly System.Windows.DependencyProperty EnableThreadIdProperty;
        public static readonly System.Windows.DependencyProperty EnableTimestampProperty;
        public static readonly System.Windows.DependencyProperty IgnoreCatelLoggingProperty;
        public static readonly System.Windows.DependencyProperty LogFilterProperty;
        public static readonly System.Windows.DependencyProperty LogListenerTypeProperty;
        public static readonly System.Windows.DependencyProperty MaximumUpdateBatchSizeProperty;
        public static readonly System.Windows.DependencyProperty ShowDebugProperty;
        public static readonly System.Windows.DependencyProperty ShowErrorProperty;
        public static readonly System.Windows.DependencyProperty ShowInfoProperty;
        public static readonly System.Windows.DependencyProperty ShowMultilineMessagesExpandedProperty;
        public static readonly System.Windows.DependencyProperty ShowWarningProperty;
        public static readonly System.Windows.DependencyProperty SupportCommandManagerProperty;
        public static readonly System.Windows.DependencyProperty UseApplicationFilterGroupsConfigurationProperty;
        public LogViewerControl() { }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool AutoScroll { get; set; }
        public bool EnableIcons { get; set; }
        public bool EnableTextColoring { get; set; }
        public bool EnableThreadId { get; set; }
        public bool EnableTimestamp { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool IgnoreCatelLogging { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string LogFilter { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public System.Type LogListenerType { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public int MaximumUpdateBatchSize { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowDebug { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowError { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowInfo { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowMultilineMessagesExpanded { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowWarning { get; set; }
        public bool SupportCommandManager { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool UseApplicationFilterGroupsConfiguration { get; set; }
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
    public class MsSqlDataSourceProvider : Orc.Controls.IDataSourceProvider
    {
        public MsSqlDataSourceProvider() { }
        public string DataBasesQuery { get; }
        public System.Collections.Generic.IList<string> GetDataSources() { }
    }
    public class MultipleCommandParameterConverter : System.Windows.Data.IMultiValueConverter
    {
        public MultipleCommandParameterConverter() { }
        public object Convert(object[] values, System.Type targetType, object parameter, System.Globalization.CultureInfo culture) { }
        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) { }
    }
    public class static NumberFormatHelper
    {
        public static string GetFormat(int digits) { }
    }
    public class NumericTextBox : System.Windows.Controls.TextBox
    {
        public static readonly System.Windows.DependencyProperty FormatProperty;
        public static readonly System.Windows.DependencyProperty IsDecimalAllowedProperty;
        public static readonly System.Windows.DependencyProperty IsNegativeAllowedProperty;
        public static readonly System.Windows.DependencyProperty IsNullValueAllowedProperty;
        public static readonly System.Windows.DependencyProperty MaxValueProperty;
        public static readonly System.Windows.DependencyProperty MinValueProperty;
        public static readonly System.Windows.DependencyProperty ValueProperty;
        public NumericTextBox() { }
        public string Format { get; set; }
        public bool IsDecimalAllowed { get; set; }
        public bool IsNegativeAllowed { get; set; }
        public bool IsNullValueAllowed { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public System.Nullable<double> Value { get; set; }
        public event System.EventHandler LeftBoundReached;
        public event System.EventHandler RightBoundReached;
        public event System.EventHandler ValueChanged;
        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e) { }
        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e) { }
    }
    public class OpenFilePicker : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty BaseDirectoryProperty;
        public static readonly System.Windows.DependencyProperty FilterProperty;
        public static readonly System.Windows.DependencyProperty LabelTextProperty;
        public static readonly System.Windows.DependencyProperty LabelWidthProperty;
        public static readonly System.Windows.DependencyProperty SelectedFileProperty;
        public OpenFilePicker() { }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string BaseDirectory { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string Filter { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string LabelText { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public double LabelWidth { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
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
        public OpenFilePickerViewModel(Catel.Services.IOpenFileService selectFileService, Catel.Services.IProcessService processService) { }
        public string BaseDirectory { get; set; }
        public string Filter { get; set; }
        public string LabelText { get; set; }
        public double LabelWidth { get; set; }
        public Catel.MVVM.Command OpenDirectory { get; }
        public string SelectedFile { get; set; }
        public string SelectedFileDisplayName { get; }
        public Catel.MVVM.TaskCommand SelectFile { get; }
    }
    public class OrdinalToolTipItem
    {
        public OrdinalToolTipItem() { }
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class PasswordBindBehavior : Catel.Windows.Interactivity.BehaviorBase<System.Windows.Controls.PasswordBox>
    {
        public static readonly System.Windows.DependencyProperty PasswordProperty;
        public PasswordBindBehavior() { }
        public string Password { get; set; }
        protected override void OnAssociatedObjectLoaded() { }
        protected override void OnAssociatedObjectUnloaded() { }
    }
    public class PinnableItemTemplateSelector : System.Windows.Controls.DataTemplateSelector
    {
        public PinnableItemTemplateSelector() { }
        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container) { }
    }
    [System.Windows.TemplatePartAttribute(Name="CloseButton", Type=typeof(System.Windows.Controls.Button))]
    [System.Windows.TemplatePartAttribute(Name="DragGrip", Type=typeof(System.Windows.FrameworkElement))]
    [System.Windows.TemplatePartAttribute(Name="GripDrawing", Type=typeof(System.Windows.Media.GeometryDrawing))]
    [System.Windows.TemplatePartAttribute(Name="PinButton", Type=typeof(System.Windows.Controls.Primitives.ToggleButton))]
    public class PinnableToolTip : System.Windows.Controls.ContentControl
    {
        public static readonly System.Windows.DependencyProperty AccentColorBrushProperty;
        public static readonly System.Windows.DependencyProperty AllowCloseByUserProperty;
        public static readonly System.Windows.DependencyProperty GripColorProperty;
        public static readonly System.Windows.DependencyProperty HorizontalOffsetProperty;
        public static readonly System.Windows.DependencyProperty IsPinnedProperty;
        public static readonly System.Windows.DependencyProperty OpenLinkCommandProperty;
        public static readonly System.Windows.DependencyProperty ResizeModeProperty;
        public static readonly System.Windows.DependencyProperty VerticalOffsetProperty;
        public PinnableToolTip() { }
        public System.Windows.Media.Brush AccentColorBrush { get; set; }
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
        public void SetupTimer(int initialShowDelay, int showDuration) { }
        public void SetUserDefinedAdorner(System.Windows.UIElement element) { }
        public void Show() { }
        public void StartTimer() { }
        public void StopTimer(bool reset = True) { }
    }
    public class static PinnableToolTipService
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
        public static System.Collections.Generic.List<Orc.Controls.PredefinedColor> All { get; }
        public static System.Collections.Generic.List<Orc.Controls.PredefinedColor> AllThemeColors { get; }
        public string Name { get; }
        public System.Windows.Media.Color Value { get; }
        public override bool Equals(object obj) { }
        public bool Equals(Orc.Controls.PredefinedColor other) { }
        public static string GetColorName(System.Windows.Media.Color color) { }
        public override int GetHashCode() { }
        public static Orc.Controls.PredefinedColor GetPredefinedColor(System.Windows.Media.Color color) { }
        public static bool IsPredefined(System.Windows.Media.Color color) { }
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
    public class static PredefinedDateRanges
    {
        public static Orc.Controls.DateRange LastMonth { get; }
        public static Orc.Controls.DateRange LastWeek { get; }
        public static Orc.Controls.DateRange ThisMonth { get; }
        public static Orc.Controls.DateRange ThisWeek { get; }
        public static Orc.Controls.DateRange Today { get; }
        public static Orc.Controls.DateRange Yesterday { get; }
    }
    [System.Windows.TemplatePartAttribute(Name="PART_LowerSlider", Type=typeof(System.Windows.Controls.Slider))]
    [System.Windows.TemplatePartAttribute(Name="PART_SelectedRange", Type=typeof(System.Windows.Shapes.Rectangle))]
    [System.Windows.TemplatePartAttribute(Name="PART_TrackBackground", Type=typeof(System.Windows.Controls.Border))]
    [System.Windows.TemplatePartAttribute(Name="PART_UpperSlider", Type=typeof(System.Windows.Controls.Slider))]
    public class RangeSlider : System.Windows.Controls.Primitives.RangeBase
    {
        public static readonly System.Windows.DependencyProperty HighlightSelectedRangeProperty;
        public static readonly System.Windows.DependencyProperty LowerValueProperty;
        public static readonly System.Windows.DependencyProperty OrientationProperty;
        public static readonly System.Windows.DependencyProperty UpperValueProperty;
        public RangeSlider() { }
        [System.ComponentModel.BindableAttribute(true)]
        [System.ComponentModel.CategoryAttribute("Behavior")]
        public bool HighlightSelectedRange { get; set; }
        [System.ComponentModel.BindableAttribute(true)]
        [System.ComponentModel.CategoryAttribute("Behavior")]
        public double LowerValue { get; set; }
        [System.ComponentModel.BindableAttribute(true)]
        [System.ComponentModel.CategoryAttribute("Behavior")]
        public System.Windows.Controls.Orientation Orientation { get; set; }
        [System.ComponentModel.BindableAttribute(true)]
        [System.ComponentModel.CategoryAttribute("Behavior")]
        public double UpperValue { get; set; }
        public override void OnApplyTemplate() { }
        protected override void OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs e) { }
    }
    public class ResizingAdorner : System.Windows.Documents.Adorner
    {
        protected override int VisualChildrenCount { get; }
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize) { }
        public static Orc.Controls.ResizingAdorner Attach(System.Windows.FrameworkElement element) { }
        public static void Detach(Orc.Controls.ResizingAdorner adorner) { }
        protected override System.Windows.Media.Visual GetVisualChild(int index) { }
    }
    public class static RichTextBoxExtensions
    {
        public static string GetInlineText(this System.Windows.Controls.RichTextBox richTextBox) { }
    }
    public class RichTextBoxParagraph : System.Windows.Documents.Paragraph
    {
        public RichTextBoxParagraph(Catel.Logging.LogEntry logEntry) { }
        public Catel.Logging.LogEntry LogEntry { get; }
    }
    public class static RichTextBoxParagraphExtensions
    {
        public static void SetData(this Orc.Controls.RichTextBoxParagraph paragraph, bool showTimestamp = True, bool showThreadId = True, bool showMultilineMessagesExpanded = False) { }
    }
    public class SaveFilePicker : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty FilterProperty;
        public static readonly System.Windows.DependencyProperty LabelTextProperty;
        public static readonly System.Windows.DependencyProperty LabelWidthProperty;
        public static readonly System.Windows.DependencyProperty SelectedFileProperty;
        public SaveFilePicker() { }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string Filter { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public string LabelText { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public double LabelWidth { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
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
        public string Filter { get; set; }
        public string LabelText { get; set; }
        public double LabelWidth { get; set; }
        public Catel.MVVM.Command OpenDirectory { get; }
        public string SelectedFile { get; set; }
        public Catel.MVVM.TaskCommand SelectFile { get; }
    }
    public class SqlConnectionString : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData PropertiesProperty;
        public SqlConnectionString(System.Data.Common.DbConnectionStringBuilder connectionStringBuilder, Orc.Controls.DbProvider dbProvider) { }
        public Orc.Controls.DbProvider DbProvider { get; }
        public System.Collections.Generic.Dictionary<string, Orc.Controls.ConnectionStringProperty> Properties { get; }
        public virtual string ToDisplayString() { }
        public override string ToString() { }
    }
    public class static SqlConnectionStringExtensions
    {
        public static Orc.Controls.ConnectionStringProperty TryGetProperty(this Orc.Controls.SqlConnectionString connectionString, string propertyName) { }
    }
    public class StackGrid : System.Windows.Controls.Grid
    {
        public StackGrid() { }
    }
    public class static StringExtensions
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
    [System.Windows.TemplatePartAttribute(Name="PART_ItemsHolder", Type=typeof(System.Windows.Controls.Panel))]
    public class TabControl : System.Windows.Controls.TabControl
    {
        public static readonly System.Windows.DependencyProperty LoadTabItemsProperty;
        public TabControl() { }
        public bool IsLazyLoading { get; }
        public Orc.Controls.LoadTabItemsBehavior LoadTabItems { get; set; }
        protected new System.Windows.Controls.TabItem GetSelectedTabItem() { }
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
    public class static TextBoxExtensions { }
    public enum TimeSpanPart
    {
        Days = 0,
        Hours = 1,
        Minutes = 2,
        Seconds = 3,
    }
    public class static TimeSpanPartExtensions
    {
        public static System.TimeSpan CreateTimeSpan(this Orc.Controls.TimeSpanPart timeSpanPart, double value) { }
        public static string GetTimeSpanPartName(this Orc.Controls.TimeSpanPart timeSpanPart) { }
        public static double GetTimeSpanPartValue(this System.TimeSpan value, Orc.Controls.TimeSpanPart timeSpanPart) { }
    }
    public class TimeSpanPicker : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public static readonly System.Windows.DependencyProperty AccentColorBrushProperty;
        public static readonly System.Windows.DependencyProperty IsReadOnlyProperty;
        public static readonly System.Windows.DependencyProperty ValueProperty;
        public TimeSpanPicker() { }
        public System.Windows.Media.Brush AccentColorBrush { get; set; }
        public bool IsReadOnly { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public System.Nullable<System.TimeSpan> Value { get; set; }
        public void InitializeComponent() { }
        public override void OnApplyTemplate() { }
        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e) { }
    }
    public class TimeSpanPickerViewModel : Catel.MVVM.ViewModelBase
    {
        public TimeSpanPickerViewModel() { }
        public System.Nullable<int> Days { get; set; }
        public System.Nullable<int> Hours { get; set; }
        public System.Nullable<int> Minutes { get; set; }
        public System.Nullable<int> Seconds { get; set; }
        public System.Nullable<System.TimeSpan> Value { get; set; }
    }
    public class static TreeViewItemExtensions
    {
        public static int GetDepth(this System.Windows.Controls.TreeViewItem item) { }
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
        public System.Nullable<Catel.Data.ValidationResultType> ResultType { get; set; }
        public void ApplyFilter(bool showErrors, bool showWarnings, string filter) { }
        public int CompareTo(object obj) { }
        public virtual int CompareTo(Orc.Controls.ValidationContextTreeNode node) { }
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
        public static readonly System.Windows.DependencyProperty AccentColorBrushProperty;
        public static readonly System.Windows.DependencyProperty IsExpandedAllOnStartupProperty;
        public static readonly System.Windows.DependencyProperty ShowButtonsProperty;
        public static readonly System.Windows.DependencyProperty ShowFilterBoxProperty;
        public static readonly System.Windows.DependencyProperty ValidationContextProperty;
        public ValidationContextView() { }
        public System.Windows.Media.Brush AccentColorBrush { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool IsExpandedAllOnStartup { get; set; }
        public bool ShowButtons { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowFilterBox { get; set; }
        [Catel.MVVM.Views.ViewToViewModelAttribute("", MappingType=Catel.MVVM.Views.ViewToViewModelMappingType.TwoWayViewWins)]
        public Catel.Data.IValidationContext ValidationContext { get; set; }
        public void InitializeComponent() { }
        public override void OnApplyTemplate() { }
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
        protected virtual System.Nullable<int> ExtractTagLine(Catel.Data.IValidationResult validationResult) { }
        protected virtual string ExtractTagName(Catel.Data.IValidationResult validationResult) { }
        public virtual System.Collections.Generic.IEnumerable<Catel.Data.IValidationResult> GetCachedResultsByTagName(string tagName) { }
        public virtual string GetDisplayName(Catel.Data.IValidationResult validationResult) { }
        public System.Nullable<int> GetLineNumber(Catel.Data.IValidationResult validationResult) { }
        public string GetTagName(Catel.Data.IValidationResult validationResult) { }
        protected class TagDetails
        {
            public TagDetails() { }
            public System.Nullable<int> ColumnIndex { get; set; }
            public string ColumnName { get; set; }
            public System.Nullable<int> Line { get; set; }
        }
    }
    public class ValidationResultNode : Orc.Controls.ValidationContextTreeNode
    {
        public static readonly Catel.Data.PropertyData LineNumberProperty;
        public ValidationResultNode(Catel.Data.IValidationResult validationResult, Orc.Controls.IValidationNamesService validationNamesService, bool isExpanded) { }
        public System.Nullable<int> LineNumber { get; }
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
    public class ValidationResultTypeToColorConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public ValidationResultTypeToColorConverter() { }
        public System.Windows.Media.Color DefaultColor { get; set; }
        public System.Windows.Media.Color ErrorColor { get; set; }
        public System.Windows.Media.Color WarningColor { get; set; }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
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
    public class WrapPanel : System.Windows.Controls.Panel
    {
        public WrapPanel() { }
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize) { }
        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize) { }
    }
}
namespace Orc.Controls.Behavior
{
    public class DropDownButtonBehavior : Catel.Windows.Interactivity.BehaviorBase<Orc.Controls.DropDownButton>
    {
        public DropDownButtonBehavior() { }
        protected override void OnAssociatedObjectLoaded() { }
        protected override void OnAssociatedObjectUnloaded() { }
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
    public class TreeViewItemToLeftMarginValueConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public TreeViewItemToLeftMarginValueConverter() { }
        public double Length { get; set; }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
    }
}
namespace Orc.Controls.Services
{
    public class MicrosoftApiSelectDirectoryService : Catel.Services.ISelectDirectoryService
    {
        public MicrosoftApiSelectDirectoryService() { }
        public string DirectoryName { get; }
        public string FileName { get; set; }
        public string Filter { get; set; }
        public string InitialDirectory { get; set; }
        public bool ShowNewFolderButton { get; set; }
        public string Title { get; set; }
        public System.Threading.Tasks.Task<bool> DetermineDirectoryAsync() { }
    }
}
namespace Orc.Controls.ViewModels
{
    public class LogViewerViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData AutoScrollProperty;
        public static readonly Catel.Data.PropertyData DebugEntriesCountProperty;
        public static readonly Catel.Data.PropertyData ErrorEntriesCountProperty;
        public static readonly Catel.Data.PropertyData IgnoreCatelLoggingProperty;
        public static readonly Catel.Data.PropertyData InfoEntriesCountProperty;
        public static readonly Catel.Data.PropertyData LogFilterProperty;
        public static readonly Catel.Data.PropertyData LogListenerTypeProperty;
        public static readonly Catel.Data.PropertyData MaximumUpdateBatchSizeProperty;
        public static readonly Catel.Data.PropertyData ShowDebugProperty;
        public static readonly Catel.Data.PropertyData ShowErrorProperty;
        public static readonly Catel.Data.PropertyData ShowInfoProperty;
        public static readonly Catel.Data.PropertyData ShowMultilineMessagesExpandedProperty;
        public static readonly Catel.Data.PropertyData ShowWarningProperty;
        public static readonly Catel.Data.PropertyData TypeFilterProperty;
        public static readonly Catel.Data.PropertyData TypeNamesProperty;
        public static readonly Catel.Data.PropertyData UseApplicationFilterGroupsConfigurationProperty;
        public static readonly Catel.Data.PropertyData WarningEntriesCountProperty;
        public LogViewerViewModel(Catel.IoC.ITypeFactory typeFactory, Catel.Services.IDispatcherService dispatcherService, Orc.Controls.IApplicationLogFilterGroupService applicationLogFilterGroupService, Orc.Controls.LogViewerLogListener logViewerLogListener) { }
        public bool AutoScroll { get; set; }
        public int DebugEntriesCount { get; }
        public int ErrorEntriesCount { get; }
        public bool IgnoreCatelLogging { get; set; }
        public int InfoEntriesCount { get; }
        public System.Collections.ObjectModel.ObservableCollection<Catel.Logging.LogEntry> LogEntries { get; }
        public string LogFilter { get; set; }
        public System.Type LogListenerType { get; set; }
        public int MaximumUpdateBatchSize { get; set; }
        public bool ShowDebug { get; set; }
        public bool ShowError { get; set; }
        public bool ShowInfo { get; set; }
        public bool ShowMultilineMessagesExpanded { get; set; }
        public bool ShowWarning { get; set; }
        public string TypeFilter { get; set; }
        public Catel.Collections.FastObservableCollection<string> TypeNames { get; }
        public bool UseApplicationFilterGroupsConfiguration { get; set; }
        public int WarningEntriesCount { get; }
        public event System.EventHandler<Orc.Controls.LogEntryEventArgs> LogMessage;
        public void ClearEntries() { }
        protected override System.Threading.Tasks.Task CloseAsync() { }
        public System.Collections.Generic.IEnumerable<Catel.Logging.LogEntry> GetFilteredLogEntries() { }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
        public bool IsValidLogEntry(Catel.Logging.LogEntry logEntry) { }
    }
}
namespace XamlGeneratedNamespace
{
    public sealed class GeneratedInternalTypeHelper : System.Windows.Markup.InternalTypeHelper
    {
        public GeneratedInternalTypeHelper() { }
        protected override void AddEventHandler(System.Reflection.EventInfo eventInfo, object target, System.Delegate handler) { }
        protected override System.Delegate CreateDelegate(System.Type delegateType, object target, string handler) { }
        protected override object CreateInstance(System.Type type, System.Globalization.CultureInfo culture) { }
        protected override object GetPropertyValue(System.Reflection.PropertyInfo propertyInfo, object target, System.Globalization.CultureInfo culture) { }
        protected override void SetPropertyValue(System.Reflection.PropertyInfo propertyInfo, object target, object value, System.Globalization.CultureInfo culture) { }
    }
}