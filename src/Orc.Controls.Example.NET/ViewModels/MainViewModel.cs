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

            DeferValidationUntilFirstSaveCall = false;
        }
        #endregion

        #region Properties
        public override string Title => "Orc.Controls example";

        [ObsoleteEx(TreatAsErrorFromVersion = "3.0", RemoveInVersion = "4.0", Message = "Use AccentColorBrush markup extension instead")]
        public Brush AccentColorBrush { get; set; }
        #endregion
    }
}
