// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbProviderPicker.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using Catel.MVVM.Views;

    [ObsoleteEx(TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0", Message = "Use DbProviderPicker from Orc.DataAccess.Xaml library instead")]
    public sealed partial class DbProviderPicker
    {
        #region Constants
        public static readonly DependencyProperty DbProviderProperty = DependencyProperty.Register(
            nameof(DbProvider), typeof(DbProvider), typeof(DbProviderPicker), new PropertyMetadata(default(DbProvider)));
        #endregion

        #region Constructors
        static DbProviderPicker()
        {
            typeof(DbProviderPicker).AutoDetectViewPropertiesToSubscribe();
        }

        public DbProviderPicker()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewModelWins)]
        public DbProvider DbProvider
        {
            get { return (DbProvider)GetValue(DbProviderProperty); }
            set { SetValue(DbProviderProperty, value); }
        }
        #endregion
    }
}
