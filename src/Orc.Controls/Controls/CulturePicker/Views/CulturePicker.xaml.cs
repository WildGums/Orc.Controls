// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CulturePicker.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Globalization;
    using System.Windows;
    using Catel.MVVM.Views;

    public sealed partial class CulturePicker
    {
        #region Constructors
        static CulturePicker()
        {
            typeof(CulturePicker).AutoDetectViewPropertiesToSubscribe();
        }

        public CulturePicker()
        {
            InitializeComponent();
        }
        #endregion

        #region Dependency properties
        public static readonly DependencyProperty SelectedCultureProperty = DependencyProperty.Register(
            nameof(SelectedCulture), typeof(CultureInfo), typeof(CulturePicker),
            new FrameworkPropertyMetadata(default(CultureInfo), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public CultureInfo SelectedCulture
        {
            get { return (CultureInfo)GetValue(SelectedCultureProperty); }
            set { SetValue(SelectedCultureProperty, value); }
        }
        #endregion
    }
}
