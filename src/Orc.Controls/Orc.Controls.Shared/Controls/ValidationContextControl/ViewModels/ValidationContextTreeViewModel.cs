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
            ValidationResultTags = new FastObservableCollection<ValidationResultTagNode>();
        }

        #region Properties
        public string Filter { get; set; }

        public IValidationContext ValidationContext { get; set; }

        public bool ShowWarnings { get; set; }

        public bool ShowErrors { get; set; }

        public FastObservableCollection<ValidationResultTagNode> ValidationResultTags { get; }

        public IEnumerable<IValidationContextTreeNode> Nodes => ValidationResultTags.OfType<IValidationContextTreeNode>();
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
            ValidationResultTags.Clear();

            var validationContext = ValidationContext;
            if (validationContext == null)
            {
                return;
            }

            var resultTagNodes = validationContext
                .GetValidations()
                .Select(x => x.Tag).Distinct()
                .Select(tag => new ValidationResultTagNode(tag))
                .OrderBy(x => x);

            foreach (var tagNode in resultTagNodes)
            {
                tagNode.AddValidationResultTypeNode(validationContext, ValidationResultType.Error);

                tagNode.AddValidationResultTypeNode(validationContext, ValidationResultType.Warning);

                ValidationResultTags.Add(tagNode);
            }
        }

        private void ApplyFilter()
        {
            foreach (var tagNode in ValidationResultTags)
            {
                tagNode.ApplyFilter(ShowErrors, ShowWarnings, Filter);
            }
        }       
    }
}