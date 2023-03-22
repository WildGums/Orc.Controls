namespace Orc.Controls;

using System.Windows;
using System.Windows.Automation.Peers;
using Automation;
using Catel.MVVM.Views;

/// <summary>
///     Interaction logic for OpenFilePicker.xaml
/// </summary>
public partial class OpenFilePicker
{
    static OpenFilePicker()
    {
        typeof(OpenFilePicker).AutoDetectViewPropertiesToSubscribe();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenFilePicker"/> class.
    /// </summary>
    /// <remarks>This method is required for design time support.</remarks>
    public OpenFilePicker()
    {
        InitializeComponent();
    }

    #region Properties
    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public double LabelWidth
    {
        get { return (double)GetValue(LabelWidthProperty); }
        set { SetValue(LabelWidthProperty, value); }
    }

    // Using a DependencyProperty as the backing store for LabelWidth.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LabelWidthProperty = DependencyProperty.Register(nameof(LabelWidth),
        typeof(double), typeof(OpenFilePicker), new PropertyMetadata(125d));

    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public string LabelText
    {
        get { return (string)GetValue(LabelTextProperty); }
        set { SetValue(LabelTextProperty, value); }
    }

    // Using a DependencyProperty as the backing store for LabelText.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(nameof(LabelText),
        typeof(string), typeof(OpenFilePicker), new PropertyMetadata(string.Empty));

    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public string SelectedFile
    {
        get { return (string)GetValue(SelectedFileProperty); }
        set { SetValue(SelectedFileProperty, value); }
    }

    // Using a DependencyProperty as the backing store for SelectedFile.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectedFileProperty = DependencyProperty.Register(nameof(SelectedFile), typeof(string),
        typeof(OpenFilePicker), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public string Filter
    {
        get { return (string)GetValue(FilterProperty); }
        set { SetValue(FilterProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Filter.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(string),
        typeof(OpenFilePicker), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public string BaseDirectory
    {
        get { return (string)GetValue(BaseDirectoryProperty); }
        set { SetValue(BaseDirectoryProperty, value); }
    }

    public static readonly DependencyProperty BaseDirectoryProperty = DependencyProperty.Register(nameof(BaseDirectory), typeof(string),
        typeof(OpenFilePicker), new PropertyMetadata(string.Empty));
    #endregion

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new OpenFilePickerAutomationPeer(this);
    }
}
