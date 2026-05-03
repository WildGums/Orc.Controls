namespace Orc.Controls;

using System;
using System.Linq;
using Catel.Data;

internal static class ValidationResultTagNodeExtensions
{
    public static void AddValidationResultTypeNode(this ValidationResultTagNode validationResultTagNode, IValidationContext validationContext,
        ValidationResultType validationResultType, IValidationNamesService validationNamesService, bool isExpanded)
    {
        ArgumentNullException.ThrowIfNull(validationResultTagNode);
        ArgumentNullException.ThrowIfNull(validationContext);
        ArgumentNullException.ThrowIfNull(validationNamesService);

        var validationResults = validationNamesService.GetCachedResultsByTagName(validationResultTagNode.TagName)
            .Where(x => x.ValidationResultType == validationResultType).ToList();
        if (!validationResults.Any())
        {
            return;
        }

        var resultTypeNode = new ValidationResultTypeNode(validationResultType, validationResults, validationNamesService, isExpanded);

        validationResultTagNode.Children.Add(resultTypeNode);
    }
}