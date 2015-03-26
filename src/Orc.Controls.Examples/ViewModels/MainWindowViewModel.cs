// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using System.Windows.Media;
    using Catel.MVVM;

    public class MainWindowViewModel : ViewModelBase
    {
        #region Constructors
        public MainWindowViewModel()
        {
            AccentColorBrush = Brushes.Orange;
        }
        #endregion

        #region Properties
        public Brush AccentColorBrush { get; set; }
        #endregion

    }
}