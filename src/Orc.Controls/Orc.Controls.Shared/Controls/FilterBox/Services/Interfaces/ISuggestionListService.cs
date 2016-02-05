// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISuggestionListService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Services
{
    using System;
    using System.Collections.Generic;

    public interface ISuggestionListService
    {
        List<KeyValuePair<string, string>> GetSuggestionList(DateTime dateTime, DateTimePart editablePart);
    }
}