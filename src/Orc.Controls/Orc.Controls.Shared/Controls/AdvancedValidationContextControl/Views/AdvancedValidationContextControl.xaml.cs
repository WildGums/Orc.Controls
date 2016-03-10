// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdvancedValidationContextControl.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using Catel.Data;
    using Catel.MVVM.Views;

    public sealed partial class AdvancedValidationContextControl
    {
        static AdvancedValidationContextControl()
        {
            typeof(AdvancedValidationContextControl).AutoDetectViewPropertiesToSubscribe();
        }

        public AdvancedValidationContextControl()
        {
            this.InitializeComponent();
        }

        #region Dependency properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public IValidationContext ValidationContext
        {
            get { return (IValidationContext) GetValue(ValidationContextProperty); }
            set { SetValue(ValidationContextProperty, value); }
        }

        public static readonly DependencyProperty ValidationContextProperty = DependencyProperty.Register(
            "ValidationContext", typeof (IValidationContext), typeof (AdvancedValidationContextControl), new PropertyMetadata(null));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public bool ShowFilterBox
        {
            get { return (bool) GetValue(ShowFilterBoxProperty); }
            set { SetValue(ShowFilterBoxProperty, value); }
        }

        public static readonly DependencyProperty ShowFilterBoxProperty = DependencyProperty.Register(
            "ShowFilterBox", typeof (bool), typeof (AdvancedValidationContextControl), new PropertyMetadata(true));
        #endregion
    }
}