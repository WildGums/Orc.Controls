// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterBoxControl.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
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
    }
}