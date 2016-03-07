// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationRule.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel.Data;

    public class ValidationRule : ModelBase
    {
        public ValidationRule(string name, ValidationContext validationContext)
        {
            DisplayName = name;
            ValidationContext = validationContext;
        }

        public string DisplayName { get; set; }
        public ValidationContext ValidationContext { get; set; }
        public IList<ValidationResultGroup> ResultGroups { get; set; }

        private void OnValidationContextChanged()
        {
            var groups = new List<ValidationResultGroup>();
            if (ValidationContext.HasErrors)
            {
                var errors = new ValidationResultGroup(ValidationResultType.Error);
                groups.Add(errors);

                errors.ValidationResults = ValidationContext.GetBusinessRuleErrors().Cast<IValidationResult>().ToList();
            }
        }
    }
}