// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextDisplayViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.Data;
    using Catel.MVVM;

    public class ValidationContextDisplayViewModel : ViewModelBase
    {
        public ValidationContextDisplayViewModel()
        {
            var validationContext = new ValidationContext();

        var businessRuleValidationResult = BusinessRuleValidationResult.CreateError("SomeMessage", "MyTag1");
     //   validationcontext.AddResult(businessRuleValidationResult);
        }

        public IList<ValidationRule> ValidationRules { get; set; }

        protected override Task InitializeAsync()
        {
            var context1 = new ValidationContext();
            context1.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateError("error1"));
            context1.AddBusinessRuleValidationResult(BusinessRuleValidationResult.CreateError("error2"));
            var context2 = new ValidationContext();
            var context3 = new ValidationContext();

            ValidationRules = new List<ValidationRule>()
            {
                new ValidationRule("Rule1", context1),
                new ValidationRule("Rule2", context2),
                new ValidationRule("Rule3", context3),
            };
            return base.InitializeAsync();
        }
    }
}