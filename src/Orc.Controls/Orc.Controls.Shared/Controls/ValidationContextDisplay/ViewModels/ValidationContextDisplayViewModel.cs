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
    using Catel.Reflection;

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
                    var tag = @group.Key;
                    var ruleName = ExtractRuleName(tag);

                    var validationRule = new ValidationResultTagNode(ruleName);

                    if (ShowErrors)
                    {
                        var errors = validationContext.GetBusinessRuleErrors(tag);
                        var errorGroup = new ValidationResultTypeNode(ValidationResultType.Error, errors);
                        validationRule.Children.Add(errorGroup);
                    }

                    if (ShowWarnings)
                    {
                        var warnings = validationContext.GetBusinessRuleWarnings(tag);
                        var warningGroup = new ValidationResultTypeNode(ValidationResultType.Warning, warnings);
                        validationRule.Children.Add(warningGroup);
                    }

                    ValidationRules.Add(validationRule);
                }
            }
        }

        private static string ExtractRuleName(object value)
        {
            if (ReferenceEquals(value, null))
            {
                return string.Empty;
            }

            var stringValue = value as string;
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                return stringValue;
            }

            var type = value.GetType();
            var nameProperty = type.GetPropertyEx("Name");
            if (nameProperty != null)
            {
                return (string)nameProperty.GetValue(value, new object[0]);
            }
            
            return value.ToString();
        }
    }
}