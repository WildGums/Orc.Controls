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
            var line = ExtractTagLine(validationResult);

            if (line.HasValue)
            {
                return $"Row {line}: {validationResult.Message}";
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

        protected virtual int? ExtractTagLine(IValidationResult validationResult)
        {
            var tag = validationResult.Tag;
            if (ReferenceEquals(tag, null))
            {
                return null;
            }

            if (tag is string)
            {
                return null;
            }

            var type = tag.GetType();
            var lineProperty = type.GetPropertyEx("Line");

            return lineProperty?.GetValue(tag, new object[0]) as int?;
        }
    }
}