// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextDisplayViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.Data;
    using Catel.MVVM;

    public class ValidationContextDisplayViewModel : ViewModelBase
    {
        public ValidationContextDisplayViewModel()
        {
            ValidationRules = new List<ValidationRule>();
        }

        public IList<ValidationRule> ValidationRules { get; set; }

        protected override Task InitializeAsync()
        {
            //NOTE: A context display will only show the results for *one* ValidationContext
            var context = new ValidationContext();

            var result1 = BusinessRuleValidationResult.CreateErrorWithTag("Error1 message", "Rule1");
            var result2 = BusinessRuleValidationResult.CreateWarningWithTag("Warning1 message", "Rule1");

            var result3 = BusinessRuleValidationResult.CreateErrorWithTag("Error2 message", "Rule2");
            var result4 = BusinessRuleValidationResult.CreateErrorWithTag("Error3 message", "Rule2");

            context.AddBusinessRuleValidationResult(result1);
            context.AddBusinessRuleValidationResult(result2);
            context.AddBusinessRuleValidationResult(result3);
            context.AddBusinessRuleValidationResult(result4);

            var validationResultsByTag = context.GetBusinessRuleValidations().GroupBy(x => x.Tag);

            foreach (var group in validationResultsByTag)
            {
                var errors = context.GetBusinessRuleErrors(group.Key);
                var warnings = context.GetBusinessRuleWarnings(group.Key);

                var ruleName = string.Empty;

                if (group.Key is string)
                {
                    ruleName = group.Key.ToString();
                }
                else
                {
                    // TODO: reflfect on the object to get the "Name" value.
                    // First check that a "Name" property exists.

                    // We have an object and we need to retreive the "Name" property which we assume is a string.
                    // Need to use reflrection to get the value of group.key.Name
                }

                var validationRule = new ValidationRule(ruleName);

                var errorGroup = new ValidationResultGroup(ValidationResultType.Error, errors);
                var warningGroup = new ValidationResultGroup(ValidationResultType.Warning, warnings);
                
                validationRule.ResultGroups.Add(errorGroup);
                validationRule.ResultGroups.Add(warningGroup);

                validationRule.SetSummary(); // This should really happen automatically

                ValidationRules.Add(validationRule);
            }

            return base.InitializeAsync();
        }
    }
}