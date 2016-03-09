// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextDisplay.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using Catel.Data;
    using Catel.MVVM.Views;

    public sealed partial class ValidationContextDisplay
    {
        static ValidationContextDisplay()
        {
            typeof (ValidationContextDisplay).AutoDetectViewPropertiesToSubscribe();
        }

        public ValidationContextDisplay()
        {
            this.InitializeComponent();
        }

        #region Dependency properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public ValidationContext ValidationContext
        {
            get { return (ValidationContext) GetValue(ValidationContextProperty); }
            set { SetValue(ValidationContextProperty, value); }
        }

        public static readonly DependencyProperty ValidationContextProperty = DependencyProperty.Register(
            "ValidationContext", typeof(ValidationContext), typeof(ValidationContextDisplay), new PropertyMetadata(default(ValidationContext)));        

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public bool ShowErrors
        {
            get { return (bool) GetValue(ShowErrorsProperty); }
            set { SetValue(ShowErrorsProperty, value); }
        }

        public static readonly DependencyProperty ShowErrorsProperty = DependencyProperty.Register(
            "ShowErrors", typeof(bool), typeof(ValidationContextDisplay), new PropertyMetadata(true));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public bool ShowWarnings
        {
            get { return (bool) GetValue(ShowWarningsProperty); }
            set { SetValue(ShowWarningsProperty, value); }
        }

        public static readonly DependencyProperty ShowWarningsProperty = DependencyProperty.Register(
            "ShowWarnings", typeof(bool), typeof(ValidationContextDisplay), new PropertyMetadata(true));
        #endregion
    }
}