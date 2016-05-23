// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResultTypeNode.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using Catel.Collections;
    using Catel.Data;

    public class ValidationResultTypeNode : ValidationContextTreeNode
    {
        public ValidationResultTypeNode(ValidationResultType resultType, IEnumerable<IValidationResult> validationResults,
            IValidationNamesService validationNamesService, bool isExpanded)
            : base(isExpanded)
        {
            Argument.IsNotNull(() => validationResults);
            Argument.IsNotNull(() => validationNamesService);

            ResultType = resultType;

            var children = validationResults.Select(x => new ValidationResultNode(x, validationNamesService, isExpanded)).OrderBy(x => x).ToList();
            Children.ReplaceRange(children);

            UpdateDisplayName();
        }

        private void UpdateDisplayName()
        {
            switch (ResultType)
            {
                case ValidationResultType.Error:
                    DisplayName = LanguageHelper.GetString("Controls_ValidationContextControl_Errors");
                    break;

                case ValidationResultType.Warning:
                    DisplayName = LanguageHelper.GetString("Controls_ValidationContextControl_Warnings");
                    break;
            }
        }
    }
}