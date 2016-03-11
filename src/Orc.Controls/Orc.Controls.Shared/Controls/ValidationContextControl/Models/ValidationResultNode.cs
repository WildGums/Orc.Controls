// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResultNode.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Globalization;
    using Catel.Data;

    internal class ValidationResultNode : ValidationContextTreeNode
    {
        public ValidationResultNode(IValidationResult validationResult)
        {
            var fieldValidationResult = validationResult as FieldValidationResult;
            if (string.IsNullOrEmpty(fieldValidationResult?.PropertyName))
            {
                DisplayName = validationResult.Message;
            }
            else
            {
                DisplayName = $"{fieldValidationResult.PropertyName}: {fieldValidationResult.Message}";
            }
            
            ResultType = validationResult.ValidationResultType;
        }

        public override void ApplyFilter(bool showErrors, bool showWarnings, string filter)
        {
            var isVisible = false;

            if (showErrors && ResultType != null && ResultType.Value == ValidationResultType.Error)
            {
                isVisible = true;
            }

            if (showWarnings && ResultType != null && ResultType.Value == ValidationResultType.Warning)
            {
                isVisible = true;
            }

            if (!isVisible)
            {
                IsVisible = false;
                return;
            }

            if (string.IsNullOrEmpty(filter))
            {
                IsVisible = true;
                return;
            }

            var culture = CultureInfo.InvariantCulture;
            IsVisible = culture.CompareInfo.IndexOf(DisplayName, filter, CompareOptions.IgnoreCase) >= 0;
        }
    }
}