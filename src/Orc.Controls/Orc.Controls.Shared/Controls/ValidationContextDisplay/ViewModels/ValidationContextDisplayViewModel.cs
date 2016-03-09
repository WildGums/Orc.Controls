// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextDisplayViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Linq;
    using Catel.Collections;
    using Catel.Data;
    using Catel.MVVM;

    internal class ValidationContextDisplayViewModel : ViewModelBase
    {
        public ValidationContextDisplayViewModel()
        {
            ValidationRules = new FastObservableCollection<ValidationResultTagNode>();
        }

        #region Properties
        public ValidationContext ValidationContext { get; set; }

        public bool ShowWarnings { get; set; }

        public bool ShowErrors { get; set; }

        public FastObservableCollection<ValidationResultTagNode> ValidationRules { get; }        
        #endregion

        private void OnValidationContextChanged()
        {
            Update();
        }

        private void OnShowWarningsChanged()
        {
            Update();
        }

        private void OnShowErrorsChanged()
        {
            Update();
        }

        private void Update()
        {
            var validationContext = ValidationContext;
            if (validationContext == null)
            {
                return;
            }


            using (ValidationRules.SuspendChangeNotifications())
            {
                ValidationRules.Clear();

                var validationResultsByTag = validationContext.GetBusinessRuleValidations().GroupBy(x => x.Tag);
                foreach (var group in validationResultsByTag)
                {
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

                    var validationRule = new ValidationResultTagNode(ruleName);

                    if (ShowErrors)
                    {
                        var errors = validationContext.GetBusinessRuleErrors(group.Key);
                        var errorGroup = new ValidationResultTypeNode(ValidationResultType.Error, errors);
                        validationRule.Children.Add(errorGroup);
                    }

                    if (ShowWarnings)
                    {
                        var warnings = validationContext.GetBusinessRuleWarnings(group.Key);
                        var warningGroup = new ValidationResultTypeNode(ValidationResultType.Warning, warnings);
                        validationRule.Children.Add(warningGroup);
                    }

                    ValidationRules.Add(validationRule);
                }
            }
        }
    }
}