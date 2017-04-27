// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionStringBuilder.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using Catel.MVVM.Views;

    public sealed partial class ConnectionStringBuilder
    {
        [ViewToViewModel("ConnectionStringStr", MappingType = ViewToViewModelMappingType.TwoWayViewModelWins, )]
        public string ConnectionString
        {
            get { return (string)GetValue(ConnectionStringProperty); }
            set { SetValue(ConnectionStringProperty, value); }
        }

        public static readonly DependencyProperty ConnectionStringProperty = DependencyProperty.Register(
            "ConnectionString", typeof (string), typeof (ConnectionStringBuilder), new PropertyMetadata(default(string)));

        static ConnectionStringBuilder()
        {
            typeof (ConnectionStringBuilder).AutoDetectViewPropertiesToSubscribe();
        }

        public ConnectionStringBuilder()
        {
            this.InitializeComponent();
        }
    }
}