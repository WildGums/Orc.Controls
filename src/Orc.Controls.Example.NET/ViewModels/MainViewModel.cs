// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Example.ViewModels
{
    using System.Windows.Media;
    using Catel.MVVM;

    public class MainViewModel : ViewModelBase
    {
        #region Constructors
        public MainViewModel()
        {
            AccentColorBrush = Brushes.Orange;
        }
        #endregion

        #region Properties
        public override string Title => "Orc.Controls example";
        public Brush AccentColorBrush { get; set; }
        #endregion
    }
}
