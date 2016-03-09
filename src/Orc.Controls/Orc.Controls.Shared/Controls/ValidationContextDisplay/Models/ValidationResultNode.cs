// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResultNode.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel.Data;

    internal class ValidationResultNode : ValidationContextTreeNodeBase
    {
        public ValidationResultNode(IValidationResult validationResult)
        {
            DisplayName = validationResult.Message;
            ResultType = validationResult.ValidationResultType;
        }
    }
}