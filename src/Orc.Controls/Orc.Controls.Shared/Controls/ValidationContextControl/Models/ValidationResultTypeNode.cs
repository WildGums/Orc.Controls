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

    internal class ValidationResultTypeNode : ValidationContextTreeNode
    {
        public ValidationResultTypeNode(ValidationResultType resultType, IEnumerable<IValidationResult> validationResults,
            IValidationResultNamesAdapter resultNamesAdapter)
        {
            Argument.IsNotNull(() => validationResults);
            Argument.IsNotNull(() => resultNamesAdapter);

            ResultType = resultType;            
            Children.ReplaceRange(validationResults.Select(x => new ValidationResultNode(x, resultNamesAdapter)));

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

        public override void ApplyFilter(bool showErrors, bool showWarnings, string filter)
        {
            base.ApplyFilter(showErrors, showWarnings, filter);

            if (showErrors && ResultType != null && ResultType.Value == ValidationResultType.Error)
            {
                IsVisible = Children.Any(x => x.IsVisible);
                return;
            }

            if (showWarnings && ResultType != null && ResultType.Value == ValidationResultType.Warning)
            {
                IsVisible = Children.Any(x => x.IsVisible);
                return;
            }

            IsVisible = false;
        }
    }
}