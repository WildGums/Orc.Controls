// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFindReplaceService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Services
{
    public interface IFindReplaceService
    {
        #region Methods
        string GetInitialFindText();
        bool FindNext(string textToFind, FindReplaceSettings settings);
        bool Replace(string textToFind, string textToReplace, FindReplaceSettings settings);
        void ReplaceAll(string textToFind, string textToReplace, FindReplaceSettings settings);
        #endregion
    }
}
