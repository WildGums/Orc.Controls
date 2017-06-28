// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryPicker.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Windows;
    using Catel.MVVM.Views;

    /// <summary>
    ///     Interaction logic for DirectoryPicker.xaml
    /// </summary>
    public partial class DirectoryPicker
    {
        static DirectoryPicker()
        {
            typeof (DirectoryPicker).AutoDetectViewPropertiesToSubscribe();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryPicker"/> class.
        /// </summary>
        /// <remarks>This method is required for design time support.</remarks>
        public DirectoryPicker()
        {
            InitializeComponent();
        }

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public double LabelWidth
        {
            get { return (double) GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelWidthProperty = DependencyProperty.Register("LabelWidth", typeof (double), typeof (DirectoryPicker), new PropertyMetadata(125d));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string LabelText
        {
            get { return (string) GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register("LabelText", typeof (string), typeof (DirectoryPicker), new PropertyMetadata(string.Empty));

        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public string SelectedDirectory
        {
            get { return (string) GetValue(SelectedDirectoryProperty); }
            set { SetValue(SelectedDirectoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedDirectory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDirectoryProperty = DependencyProperty.Register("SelectedDirectory", typeof (string),
            typeof (DirectoryPicker), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion
    }
}