// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextTree.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Windows;
    using Catel.Data;
    using Catel.MVVM.Views;

    internal sealed partial class ValidationContextTree
    {
        static ValidationContextTree()
        {
            typeof(ValidationContextTree).AutoDetectViewPropertiesToSubscribe();
        }

        public ValidationContextTree()
        {
            InitializeComponent();
        }

        #region Dependency properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public IValidationContext ValidationContext
        {
            get { return (IValidationContext)GetValue(ValidationContextProperty); }
            set { SetValue(ValidationContextProperty, value); }
        }

        public static readonly DependencyProperty ValidationContextProperty = DependencyProperty.Register(
            "ValidationContext", typeof(IValidationContext), typeof(ValidationContextTree), new PropertyMetadata(null));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public bool ShowErrors
        {
            get { return (bool)GetValue(ShowErrorsProperty); }
            set { SetValue(ShowErrorsProperty, value); }
        }

        public static readonly DependencyProperty ShowErrorsProperty = DependencyProperty.Register(
            "ShowErrors", typeof(bool), typeof(ValidationContextTree), new PropertyMetadata(true));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public bool ShowWarnings
        {
            get { return (bool)GetValue(ShowWarningsProperty); }
            set { SetValue(ShowWarningsProperty, value); }
        }

        public static readonly DependencyProperty ShowWarningsProperty = DependencyProperty.Register(
            "ShowWarnings", typeof(bool), typeof(ValidationContextTree), new PropertyMetadata(true));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(
            "Filter", typeof(string), typeof(ValidationContextTree), new PropertyMetadata(default(string)));


        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewModelToView)]
        public IEnumerable<IValidationContextTreeNode> Nodes
        {
            get { return (IEnumerable<IValidationContextTreeNode>)GetValue(NodesProperty); }
            set { SetValue(NodesProperty, value); }
        }

        public static readonly DependencyProperty NodesProperty = DependencyProperty.Register(
            "Nodes", typeof(IEnumerable<IValidationContextTreeNode>), typeof(ValidationContextTree), new PropertyMetadata(default(IEnumerable<IValidationContextTreeNode>)));
        #endregion        
    }
}