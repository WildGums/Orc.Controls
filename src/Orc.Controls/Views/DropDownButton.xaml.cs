// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropDownButton.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using Catel.MVVM.Views;

    /// <summary>
    /// Interaction logic for DropDownButton.xaml
    /// </summary>
    public partial class DropDownButton
    {
        #region Constructors
        public DropDownButton()
        {
            InitializeComponent();
        }
        #endregion

        

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.ViewToViewModel)]
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string),
            typeof(DropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ContextMenu DropDown
        {
            get { return (ContextMenu)this.GetValue(DropDownProperty); }
            set { this.SetValue(DropDownProperty, value); }
        }

        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register("DropDown", typeof(ContextMenu),
            typeof(DropDownButton), new UIPropertyMetadata(null));        
    }
}