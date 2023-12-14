namespace Orc.Controls;

using System;
using Catel.Data;

public class ValidationResultNode : ValidationContextTreeNode
{
    public ValidationResultNode(IValidationResult validationResult, IValidationNamesService validationNamesService, bool isExpanded)
        : base(isExpanded)
    {
        ArgumentNullException.ThrowIfNull(validationResult);
        ArgumentNullException.ThrowIfNull(validationNamesService);

        DisplayName = validationNamesService.GetDisplayName(validationResult);

        ResultType = validationResult.ValidationResultType;

        LineNumber = validationNamesService.GetLineNumber(validationResult);
    }

    public int? LineNumber { get; private set; }
}