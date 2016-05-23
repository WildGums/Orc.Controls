// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResultNode.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel;
    using Catel.Data;

    public class ValidationResultNode : ValidationContextTreeNode
    {
        public ValidationResultNode(IValidationResult validationResult, IValidationNamesService validationNamesService, bool isExpanded)
            : base(isExpanded)
        {
            Argument.IsNotNull(() => validationResult);
            Argument.IsNotNull(() => validationNamesService);

            DisplayName = validationNamesService.GetDisplayName(validationResult);

            ResultType = validationResult.ValidationResultType;
        }
    }
}