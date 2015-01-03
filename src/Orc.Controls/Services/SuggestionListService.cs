// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuggestionListService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Services
{
    using System;
    using Interfaces;

    public class SuggestionListService : ISuggestionListService
    {
        #region ISuggestionListService Members
        public string[] GetSuggestionList(DateTime dateTime, DateTimePart editablePart)
        {
            return new[] {"qq", "ee", "ff"};
        }
        #endregion
    }
}