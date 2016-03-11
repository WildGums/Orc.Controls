// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextTreeViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel.Collections;
    using Catel.Data;
    using Catel.MVVM;

    internal class ValidationContextTreeViewModel : ViewModelBase
    {
        public ValidationContextTreeViewModel()
        {
            ValidationRules = new FastObservableCollection<ValidationResultTagNode>();
        }

        #region Properties
        public string Filter { get; set; }

        public IValidationContext ValidationContext { get; set; }

        public bool ShowWarnings { get; set; }

        public bool ShowErrors { get; set; }

        public FastObservableCollection<ValidationResultTagNode> ValidationRules { get; }

        public IEnumerable<IValidationContextTreeNode> Nodes => ValidationRules.OfType<IValidationContextTreeNode>();
        #endregion

        private void OnValidationContextChanged()
        {
            Update();
            ApplyFilter();
        }

        private void OnShowWarningsChanged()
        {
            ApplyFilter();
        }

        private void OnShowErrorsChanged()
        {
            ApplyFilter();
        }

        private void OnFilterChanged()
        {
            ApplyFilter();
        }

        private void Update()
        {
            ValidationRules.Clear();

            var validationContext = ValidationContext;
            if (validationContext == null)
            {
                return;
            }

            var validationResults = validationContext
                .GetValidations()
                .Select(x => x.Tag).Distinct()
                .Select(tag => new ValidationResultTagNode(tag))
                .OrderBy(x => x);

            foreach (var validationRule in validationResults)
            {
                validationRule.AddValidationResultTypeNode(validationContext, ValidationResultType.Error);

                validationRule.AddValidationResultTypeNode(validationContext, ValidationResultType.Warning);

                ValidationRules.Add(validationRule);
            }
        }

        private void ApplyFilter()
        {
            foreach (var rules in ValidationRules)
            {
                rules.ApplyFilter(ShowErrors, ShowWarnings, Filter);
            }
        }       
    }
}