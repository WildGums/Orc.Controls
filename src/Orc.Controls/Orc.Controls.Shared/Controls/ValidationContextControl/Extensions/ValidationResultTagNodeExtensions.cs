// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResultTagNodeExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Catel;
    using Catel.Data;

    internal static class ValidationResultTagNodeExtensions
    {
        public static void AddValidationResultTypeNode(this ValidationResultTagNode validationRule, IValidationContext validationContext, 
            ValidationResultType validationResultType, string filter = null)
        {
            Argument.IsNotNull(() => validationRule);
            Argument.IsNotNull(() => validationContext);

            var culture = CultureInfo.InvariantCulture;
            var validationResults = validationContext.GetValidations(validationRule.Tag).Where(x => x.ValidationResultType == validationResultType);
            if (!string.IsNullOrEmpty(filter))
            {
                validationResults = validationResults.Where(x => culture.CompareInfo.IndexOf(x.Message, filter, CompareOptions.IgnoreCase) >= 0).OrderBy(x => x.Message).ToList();
            }

            var validationResultsList = validationResults as IList<IValidationResult> ?? validationResults.ToList();
            if (!validationResultsList.Any())
            {
                return;
            }

            var resultTypeNode = new ValidationResultTypeNode(validationResultType, validationResultsList);
            validationRule.Children.Add(resultTypeNode);
        }
    }
}