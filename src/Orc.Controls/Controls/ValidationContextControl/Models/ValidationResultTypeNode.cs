namespace Orc.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using Catel;
using Catel.Collections;
using Catel.Data;
using Catel.IoC;
using Catel.Services;
using Microsoft.Extensions.DependencyInjection;

public class ValidationResultTypeNode : ValidationContextTreeNode
{
    public ValidationResultTypeNode(ValidationResultType resultType, IEnumerable<IValidationResult> validationResults,
        IValidationNamesService validationNamesService, bool isExpanded)
        : base(isExpanded)
    {
        ArgumentNullException.ThrowIfNull(validationResults);
        ArgumentNullException.ThrowIfNull(validationNamesService);

        ResultType = resultType;

        var children = validationResults.Select(x => new ValidationResultNode(x, validationNamesService, isExpanded))
            .OrderBy(x => x.LineNumber).ThenBy(x => x.DisplayName).ToList();

        Children.ReplaceRange(children);

        UpdateDisplayName();
    }

    private void UpdateDisplayName()
    {
        var languageService = IoCContainer.ServiceProvider.GetRequiredService<ILanguageService>();

        switch (ResultType)
        {
            case ValidationResultType.Error:
                DisplayName = languageService.GetRequiredString("Controls_ValidationContextControl_Errors");
                break;

            case ValidationResultType.Warning:
                DisplayName = languageService.GetRequiredString("Controls_ValidationContextControl_Warnings");
                break;
        }
    }
}
