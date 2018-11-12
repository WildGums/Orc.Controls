// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValidationNamesService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using Catel.Data;

    public interface IValidationNamesService
    {
        #region Methods
        string GetDisplayName(IValidationResult validationResult);
        string GetTagName(IValidationResult validationResult);
        int? GetLineNumber(IValidationResult validationResult);
        IEnumerable<IValidationResult> GetCachedResultsByTagName(string tagName);
        void Clear();
        #endregion
    }
}
