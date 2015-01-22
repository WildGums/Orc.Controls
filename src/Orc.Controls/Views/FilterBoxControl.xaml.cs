// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBoxControl.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Windows;
    using Catel.MVVM.Views;

    /// <summary>
    /// Interaction logic for FilterBox.xaml
    /// </summary>
    public partial class FilterBoxControl
    {
        #region Constructors
        static FilterBoxControl()
        {
            typeof (FilterBoxControl).AutoDetectViewPropertiesToSubscribe();
        }

        public FilterBoxControl()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        [ViewToViewModel(MappingType = ViewToViewModelMappingType.TwoWayViewWins)]
        public List<string> FilterSource
        {
            get { return (List<string>)GetValue(FilterSourceProperty); }
            set { SetValue(FilterSourceProperty, value); }
        }

        public static readonly DependencyProperty FilterSourceProperty = DependencyProperty.Register("FilterSource", typeof(List<string>), typeof(FilterBoxControl),
            new FrameworkPropertyMetadata(new List<string>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion
    }
}