namespace Orc.Controls;

using System.Windows;
using System.Windows.Automation.Peers;
using Automation;
using Catel.MVVM.Views;

/// <summary>
///     Interaction logic for SaveFilePicker.xaml
/// </summary>
public partial class SaveFilePicker
{
    static SaveFilePicker()
    {
        typeof(SaveFilePicker).AutoDetectViewPropertiesToSubscribe();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveFilePicker"/> class.
    /// </summary>
    /// <remarks>This method is required for design time support.</remarks>
    public SaveFilePicker()
    {
        InitializeComponent();
    }

    [ObsoleteEx(Message = "This property will be deprecated", TreatAsErrorFromVersion ="4.9.0", RemoveInVersion = "6.0.0")]
    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public double LabelWidth
    {
        get { return (double)GetValue(LabelWidthProperty); }
        set { SetValue(LabelWidthProperty, value); }
    }

    [ObsoleteEx(Message = "This property will be deprecated", TreatAsErrorFromVersion ="4.9.0", RemoveInVersion = "6.0.0")]
    public static readonly DependencyProperty LabelWidthProperty = DependencyProperty.Register(nameof(LabelWidth), typeof(double), 
        typeof(SaveFilePicker), new PropertyMetadata(0d));

    [ObsoleteEx(Message = "This property will be deprecated", TreatAsErrorFromVersion ="4.9.0", RemoveInVersion = "6.0.0")]
    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public string LabelText
    {
        get { return (string)GetValue(LabelTextProperty); }
        set { SetValue(LabelTextProperty, value); }
    }

    [ObsoleteEx(Message = "This property will be deprecated", TreatAsErrorFromVersion ="4.9.0", RemoveInVersion = "6.0.0")]
    public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(nameof(LabelText), typeof(string), 
        typeof(SaveFilePicker), new PropertyMetadata(string.Empty));


    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public string SelectedFile
    {
        get { return (string)GetValue(SelectedFileProperty); }
        set { SetValue(SelectedFileProperty, value); }
    }

    public static readonly DependencyProperty SelectedFileProperty = DependencyProperty.Register(nameof(SelectedFile), typeof(string),
        typeof(SaveFilePicker), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public string Filter
    {
        get { return (string)GetValue(FilterProperty); }
        set { SetValue(FilterProperty, value); }
    }

    public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(string),
        typeof(SaveFilePicker), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public string InitialFileName
    {
        get { return (string)GetValue(InitialFileNameProperty); }
        set { SetValue(InitialFileNameProperty, value); }
    }

    public static readonly DependencyProperty InitialFileNameProperty = DependencyProperty.Register(nameof(InitialFileName), typeof(string),
        typeof(SaveFilePicker), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public string InitialDirectory
    {
        get { return (string)GetValue(InitialDirectoryProperty); }
        set { SetValue(InitialDirectoryProperty, value); }
    }

    public static readonly DependencyProperty InitialDirectoryProperty = DependencyProperty.Register(nameof(InitialDirectory), typeof(string),
        typeof(SaveFilePicker), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new SaveFilePickerAutomationPeer(this);
    }
}
