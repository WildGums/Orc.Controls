// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationNamesService.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using Catel.Data;
    using Catel.Reflection;
    using Catel.Services;

    public class ValidationNamesService : IValidationNamesService
    {
        private readonly IDictionary<string, List<IValidationResult>> _cache = new Dictionary<string, List<IValidationResult>>();

        private readonly ILanguageService _languageService;

        public ValidationNamesService(ILanguageService languageService)
        {
            Argument.IsNotNull(() => languageService);

            _languageService = languageService;
        }

        public virtual string GetDisplayName(IValidationResult validationResult)
        {
            var tagData = ExtractTagData(validationResult);

            var line = tagData.Line;
            var columnName  = tagData.ColumnName;
            var columnIndex = tagData.ColumnIndex;

            var messagePrefix = string.Empty;

            var hasLine = line.HasValue;
            var hasColumnIndex = columnIndex.HasValue;
            var hasColumnName = !string.IsNullOrWhiteSpace(columnName);

            if (hasLine)
            {
                messagePrefix += $"Row {line}";
            }
            
            if (hasLine && (hasColumnIndex || hasColumnName))
            {
                messagePrefix += ", Column ";
            }

            if (hasColumnName)
            {
                messagePrefix += $"'{columnName}' ";
            }

            if (hasColumnIndex)
            {
                messagePrefix += $"({columnIndex}) ";
            }

            if (!string.IsNullOrWhiteSpace(messagePrefix))
            {
                return $"{messagePrefix} : { validationResult.Message}";
            }

            return validationResult.Message;
        }

        public void Clear()
        {
            _cache.Clear();
        }

        public string GetTagName(IValidationResult validationResult)
        {
            var tagName = ExtractTagName(validationResult);

            List<IValidationResult> results;
            if (!_cache.TryGetValue(tagName, out results))
            {
                results = new List<IValidationResult>();
                _cache.Add(tagName, results);
            }

            var exists = (from result in results
                          where result.Message.EqualsIgnoreCase(validationResult.Message) &&
                                result.ValidationResultType == validationResult.ValidationResultType &&
                                ObjectHelper.AreEqual(result.Tag, validationResult.Tag)
                          select result).Any();
            if (!exists)
            {
                results.Add(validationResult);
            }

            return tagName;
        }

        public int? GetLineNumber(IValidationResult validationResult)
        {
            return ExtractTagData(validationResult).Line;
        }

        public virtual IEnumerable<IValidationResult> GetCachedResultsByTagName(string tagName)
        {
            List<IValidationResult> results;
            if (!_cache.TryGetValue(tagName, out results))
            {
                return Enumerable.Empty<IValidationResult>();
            }

            return results.AsEnumerable();
        }

        protected virtual string ExtractTagName(IValidationResult validationResult)
        {
            var tag = validationResult.Tag;
            if (ReferenceEquals(tag, null))
            {
                return _languageService.GetString("Controls_ValidationContextControl_Misc");
            }

            var stringValue = tag as string;
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                return stringValue;
            }

            var type = tag.GetType();
            var nameProperty = type.GetPropertyEx("Name");
            if (nameProperty != null)
            {
                return (string) nameProperty.GetValue(tag, new object[0]);
            }

            return tag.ToString();
        }

        protected virtual (int? Line, string ColumnName, int? ColumnIndex) ExtractTagData(IValidationResult validationResult)
        {
            var tag = validationResult.Tag;
            if (ReferenceEquals(tag, null))
            {
                return ((int?) null, string.Empty, (int?) null );
            }

            if (tag is string)
            {
                return ((int?)null, string.Empty, (int?)null);
            }

            var type = tag.GetType();

            var lineProperty = type.GetPropertyEx("Line");
            var line = lineProperty?.GetValue(tag, new object[0]) as int?;

            var columnNameProperty = type.GetPropertyEx("ColumnName");
            var columnName = columnNameProperty?.GetValue(tag, new object[0]) as string;

            var columnIndexProperty = type.GetPropertyEx("ColumnIndex");
            var columnIndex = columnIndexProperty?.GetValue(tag, new object[0]) as int?;

            return (line, columnName, columnIndex);
        }
    }
}