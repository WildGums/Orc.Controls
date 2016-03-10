// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextControlEx.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using Catel.Data;
    using Catel.MVVM.Views;

    public sealed partial class ValidationContextControlEx
    {
        static ValidationContextControlEx()
        {
            typeof(ValidationContextControlEx).AutoDetectViewPropertiesToSubscribe();
        }

        public ValidationContextControlEx()
        {
            this.InitializeComponent();
        }

        #region Dependency properies
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public ValidationContext ValidationContext
        {
            get { return (ValidationContext) GetValue(ValidationContextProperty); }
            set { SetValue(ValidationContextProperty, value); }
        }

        public static readonly DependencyProperty ValidationContextProperty = DependencyProperty.Register(
            "ValidationContext", typeof (ValidationContext), typeof (ValidationContextControlEx), new PropertyMetadata(default(ValidationContext)));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowFilterBox
        {
            get { return (bool) GetValue(ShowFilterBoxProperty); }
            set { SetValue(ShowFilterBoxProperty, value); }
        }

        public static readonly DependencyProperty ShowFilterBoxProperty = DependencyProperty.Register(
            "ShowFilterBox", typeof (bool), typeof (ValidationContextControlEx), new PropertyMetadata(true));
        #endregion
    }
}