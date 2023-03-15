﻿namespace Orc.Controls;

using System.Globalization;
using System.Windows;
using System.Windows.Automation.Peers;
using Catel.MVVM.Views;
using Orc.Automation;

public sealed partial class CulturePicker
{
    static CulturePicker()
    {
        typeof(CulturePicker).AutoDetectViewPropertiesToSubscribe();
    }

    public CulturePicker()
    {
        InitializeComponent();
    }
    
    public static readonly DependencyProperty SelectedCultureProperty = DependencyProperty.Register(
        nameof(SelectedCulture), typeof(CultureInfo), typeof(CulturePicker),
        new FrameworkPropertyMetadata(default(CultureInfo), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
    public CultureInfo? SelectedCulture
    {
        get { return (CultureInfo?)GetValue(SelectedCultureProperty); }
        set { SetValue(SelectedCultureProperty, value); }
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new CulturePickerAutomationPeer(this);
    }
}
