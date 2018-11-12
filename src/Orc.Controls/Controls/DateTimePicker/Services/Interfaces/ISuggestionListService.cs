// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISuggestionListService.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;

    public interface ISuggestionListService
    {
        #region Methods
        List<KeyValuePair<string, string>> GetSuggestionList(DateTime dateTime, DateTimePart editablePart, DateTimeFormatInfo dateTimeFormatInfo);
        #endregion
    }
}
