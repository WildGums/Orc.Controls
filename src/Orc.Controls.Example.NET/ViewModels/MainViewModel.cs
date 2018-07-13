// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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

            Title = "Orc.Controls example";
        }
        #endregion

        #region Properties
        public Brush AccentColorBrush { get; set; }
        #endregion

    }
}
