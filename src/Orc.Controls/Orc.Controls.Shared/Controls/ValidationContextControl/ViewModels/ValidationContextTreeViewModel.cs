// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextTreeViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.Data;
    using Catel.MVVM;

    public class ValidationContextTreeViewModel : ViewModelBase
    {
        private readonly IValidationNamesService _validationNamesService;

        public ValidationContextTreeViewModel(IValidationNamesService validationNamesService)
        {
            Argument.IsNotNull(() => validationNamesService);

            _validationNamesService = validationNamesService;

            if (ValidationResultTags == null)
            {
                ValidationResultTags = new FastObservableCollection<ValidationResultTagNode>();
            }
        }

        #region Properties
        public string Filter { get; set; }

        public IValidationContext ValidationContext { get; set; }

        public bool ShowWarnings { get; set; }

        public bool ShowErrors { get; set; }

        public FastObservableCollection<ValidationResultTagNode> ValidationResultTags { get; }

        public IEnumerable<IValidationContextTreeNode> Nodes => ValidationResultTags.OfType<IValidationContextTreeNode>();
        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _validationNamesService.Clear();
        }

        protected override Task CloseAsync()
        {
            _validationNamesService.Clear();

            return base.CloseAsync();
        }

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
                .Select(x => _validationNamesService.GetTagName(x)).Distinct()
                .Select(tagName => new ValidationResultTagNode(tagName))
                .OrderBy(x => x);

            foreach (var tagNode in resultTagNodes)
            {
                tagNode.AddValidationResultTypeNode(validationContext, ValidationResultType.Error, _validationNamesService);
                tagNode.AddValidationResultTypeNode(validationContext, ValidationResultType.Warning, _validationNamesService);

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