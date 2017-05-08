// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbProviderControl.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Controls
{
    using System.Data;
    using System.Windows;
    using Catel.MVVM.Views;

    public sealed partial class DbProviderControl
    {
        public static readonly DependencyProperty DbProviderProperty = DependencyProperty.Register(
            "DbProvider", typeof (DbProvider), typeof (DbProviderControl), new PropertyMetadata(default(DbProvider)));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewModelWins)]
        public DbProvider DbProvider
        {
            get { return (DbProvider) GetValue(DbProviderProperty); }
            set { SetValue(DbProviderProperty, value); }
        }

        static DbProviderControl()
        {
            typeof(DbProviderControl).AutoDetectViewPropertiesToSubscribe();
        }

        public DbProviderControl()
        {
            this.InitializeComponent();
        }
    }
}