// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextInputViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2020 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.ViewModels
{
    using Catel.MVVM;

    public class TextInputViewModel : ViewModelBase
    {
        #region Constructors
        public TextInputViewModel(string title)
        {
            Title = title;
        }
        #endregion

        #region Properties
        public string Text { get; set; }
        #endregion
    }
}
