namespace Orc.Controls;

using System;
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
        ArgumentNullException.ThrowIfNull(languageService);

        _languageService = languageService;
    }

    public virtual string GetDisplayName(IValidationResult validationResult)
    {
        var tagData = ExtractTagData(validationResult);

        var line = tagData.Line;
        var columnName = tagData.ColumnName;
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

        return !string.IsNullOrWhiteSpace(messagePrefix)
            ? $"{messagePrefix} : {validationResult.Message}"
            : validationResult.Message;
    }

    public void Clear()
    {
        _cache.Clear();
    }

    public string GetTagName(IValidationResult validationResult)
    {
        var tagName = ExtractTagName(validationResult);

        if (!_cache.TryGetValue(tagName, out var results))
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
        return !_cache.TryGetValue(tagName, out var results) ? Enumerable.Empty<IValidationResult>() : results.AsEnumerable();
    }

    protected virtual string ExtractTagName(IValidationResult validationResult)
    {
        var tag = validationResult.Tag;
        if (tag is not null)
        {
            var stringValue = tag as string;
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                return stringValue;
            }

            var type = tag.GetType();
            var nameProperty = type.GetPropertyEx("Name");
            if (nameProperty is not null)
            {
                var nameValue = (string?)nameProperty.GetValue(tag, Array.Empty<object>());
                if (nameValue is not null)
                {
                    return nameValue;
                }
            }

            var tagString = tag.ToString();
            if (tagString is not null)
            {
                return tagString;
            }
        }

        return _languageService.GetRequiredString("Controls_ValidationContextControl_Misc");
    }

    protected virtual int? ExtractTagLine(IValidationResult validationResult)
    {
        return ExtractTagData(validationResult).Line;
    }

    protected virtual TagDetails ExtractTagData(IValidationResult validationResult)
    {
        var tag = validationResult.Tag;
        var tagDetails = new TagDetails();

        if (tag is null)
        {
            return tagDetails;
        }

        if (tag is string)
        {
            return tagDetails;
        }

        var type = tag.GetType();

        var lineProperty = type.GetPropertyEx("Line");
        tagDetails.Line = lineProperty?.GetValue(tag, Array.Empty<object>()) as int?;

        var columnNameProperty = type.GetPropertyEx("ColumnName");
        tagDetails.ColumnName = columnNameProperty?.GetValue(tag, Array.Empty<object>()) as string ?? string.Empty;

        var columnIndexProperty = type.GetPropertyEx("ColumnIndex");
        tagDetails.ColumnIndex = columnIndexProperty?.GetValue(tag, Array.Empty<object>()) as int?;

        return tagDetails;
    }

    protected class TagDetails
    {
        public int? Line { get; set; }
        public string ColumnName { get; set; } = string.Empty;
        public int? ColumnIndex { get; set; }
    }
}
