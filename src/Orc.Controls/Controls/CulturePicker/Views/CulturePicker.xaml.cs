namespace Orc.Controls;

using System.Globalization;
using System.Windows;
using System.Windows.Automation.Peers;
using Catel.MVVM.Views;
using Orc.Automation;

public sealed partial class CulturePicker
{

    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public CultureInfo? SelectedCulture
    {
        get { return (CultureInfo?)GetValue(SelectedCultureProperty); }
        set { SetValue(SelectedCultureProperty, value); }
    }
    
    public static readonly DependencyProperty SelectedCultureProperty = DependencyProperty.Register(
        nameof(SelectedCulture), typeof(CultureInfo), typeof(CulturePicker),
        new FrameworkPropertyMetadata(default(CultureInfo), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new CulturePickerAutomationPeer(this);
    }
}
