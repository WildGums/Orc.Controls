// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResultNode.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Globalization;
    using Catel;
    using Catel.Data;

    internal class ValidationResultNode : ValidationContextTreeNode
    {
        public ValidationResultNode(IValidationResult validationResult, IValidationResultNamesAdapter resultNamesAdapter)
        {
            Argument.IsNotNull(() => validationResult);
            Argument.IsNotNull(() => resultNamesAdapter);

            DisplayName = resultNamesAdapter.GetDisplayName(validationResult);
            var fieldValidationResult = validationResult as FieldValidationResult;            
            
            ResultType = validationResult.ValidationResultType;
        }
    }
}