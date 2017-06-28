// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbProviderPicker.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Controls
{
    using System.Data;
    using System.Windows;
    using Catel.MVVM.Views;

    public sealed partial class DbProviderPicker
    {
        public static readonly DependencyProperty DbProviderProperty = DependencyProperty.Register(
            "DbProvider", typeof (DbProvider), typeof (DbProviderPicker), new PropertyMetadata(default(DbProvider)));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewModelWins)]
        public DbProvider DbProvider
        {
            get { return (DbProvider) GetValue(DbProviderProperty); }
            set { SetValue(DbProviderProperty, value); }
        }

        static DbProviderPicker()
        {
            typeof(DbProviderPicker).AutoDetectViewPropertiesToSubscribe();
        }

        public DbProviderPicker()
        {
            this.InitializeComponent();
        }
    }
}