// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValidationResultNamesAdapter.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using Catel.Data;

    public interface IValidationResultNamesAdapter
    {
        string GetDisplayName(IValidationResult validationResult);
        string GetTagName(IValidationResult validationResult);
        IEnumerable<IValidationResult> GetCachedResultsByTagName(string tagName);
    }
}