// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResultTagNodeExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Linq;
    using Catel;
    using Catel.Data;

    internal static class ValidationResultTagNodeExtensions
    {
        #region Methods
        public static void AddValidationResultTypeNode(this ValidationResultTagNode validationResultTagNode, IValidationContext validationContext,
            ValidationResultType validationResultType, IValidationNamesService validationNamesService, bool isExpanded)
        {
            Argument.IsNotNull(() => validationResultTagNode);
            Argument.IsNotNull(() => validationContext);
            Argument.IsNotNull(() => validationNamesService);

            var validationResults = validationNamesService.GetCachedResultsByTagName(validationResultTagNode.TagName)
                .Where(x => x.ValidationResultType == validationResultType).ToList();
            if (!validationResults.Any())
            {
                return;
            }

            var resultTypeNode = new ValidationResultTypeNode(validationResultType, validationResults, validationNamesService, isExpanded);

            validationResultTagNode.Children.Add(resultTypeNode);
        }
        #endregion
    }
}
