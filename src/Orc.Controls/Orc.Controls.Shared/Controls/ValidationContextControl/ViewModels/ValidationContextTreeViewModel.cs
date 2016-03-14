// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextTreeViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using Catel.Collections;
    using Catel.Data;
    using Catel.MVVM;

    internal class ValidationContextTreeViewModel : ViewModelBase
    {
        private readonly IValidationResultNamesAdapter _resultNamesAdapter;

        public ValidationContextTreeViewModel(IValidationResultNamesAdapter resultNamesAdapter)
        {
            Argument.IsNotNull(() => resultNamesAdapter);

            _resultNamesAdapter = resultNamesAdapter;
            
            if (ValidationResultTags == null)
            {
                ValidationResultTags = new FastObservableCollection<ValidationResultTagNode>();
            }

            if (NamesAdapter == null)
            {
                NamesAdapter = resultNamesAdapter;
            }
        }

        #region Properties
        public string Filter { get; set; }

        public IValidationContext ValidationContext { get; set; }

        public bool ShowWarnings { get; set; }

        public bool ShowErrors { get; set; }

        public FastObservableCollection<ValidationResultTagNode> ValidationResultTags { get; }

        public IEnumerable<IValidationContextTreeNode> Nodes => ValidationResultTags.OfType<IValidationContextTreeNode>();

        public IValidationResultNamesAdapter NamesAdapter { get; set; }
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
                .Select(x => _resultNamesAdapter.GetTagName(x)).Distinct()
                .Select(tagName => new ValidationResultTagNode(tagName))
                .OrderBy(x => x);

            foreach (var tagNode in resultTagNodes)
            {
                tagNode.AddValidationResultTypeNode(validationContext, ValidationResultType.Error, NamesAdapter);

                tagNode.AddValidationResultTypeNode(validationContext, ValidationResultType.Warning, NamesAdapter);

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