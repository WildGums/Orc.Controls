// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResultTypeNode.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel.Collections;
    using Catel.Data;

    internal class ValidationResultTypeNode : ValidationContextTreeNodeBase
    {
        public ValidationResultTypeNode(ValidationResultType resultType, IEnumerable<IValidationResult> validationResults)
        {
            ResultType = resultType;            
            Children.ReplaceRange(validationResults.Select(x => new ValidationResultNode(x)));

            UpdateDisplayName();
        }

        private void UpdateDisplayName()
        {
            switch (ResultType)
            {
                case ValidationResultType.Error:
                    DisplayName = "Errors";
                    break;
                    
                case ValidationResultType.Warning:
                    DisplayName = "Warnings";
                    break;                    
            }
        }
    }
}