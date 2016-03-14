// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResultNamesAdapter.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel.Data;
    using Catel.Reflection;

    public class ValidationResultNamesAdapter : IValidationResultNamesAdapter
    {
        private readonly IDictionary<string, List<IValidationResult>> _cache = new Dictionary<string, List<IValidationResult>>();

        public virtual string GetDisplayName(IValidationResult validationResult)
        {
            var fieldValidationResult = validationResult as FieldValidationResult;
            if (string.IsNullOrEmpty(fieldValidationResult?.PropertyName))
            {
                return validationResult.Message;
            }

            return $"{fieldValidationResult.PropertyName}: {fieldValidationResult.Message}";
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

            results.Add(validationResult);

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
                return "Misc";
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
    }
}