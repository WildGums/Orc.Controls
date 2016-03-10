// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextControlViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls.Examples.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.Data;
    using Catel.MVVM;

    public class ValidationContextControlViewModel : ViewModelBase
    {
        public ValidationContextControlViewModel()
        {
            CollapseExpand = new Command(OnCollapseExpandExecuted);
        }

        public IValidationContext ValidationContext { get; set; }

        public bool ShowErrors { get; set; }
        public bool ShowWarnings { get; set; }
        public string Filter { get; set; }
        public IEnumerable<IValidationContextTreeNode> Nodes { get; set; }

        public Command CollapseExpand { get; set; }

        private bool _expanded;
        public void OnCollapseExpandExecuted()
        {
            var nodes = Nodes;
            _expanded = !_expanded;
            CollapseOrExpand(nodes, _expanded);          
        }

        private static void CollapseOrExpand(IEnumerable<IValidationContextTreeNode> nodes, bool isExpanded)
        {
            foreach (var validationContextTreeNode in nodes)
            {
                validationContextTreeNode.IsExpanded = isExpanded;
                CollapseOrExpand(validationContextTreeNode.Children, isExpanded);
            }
        }

        protected override Task InitializeAsync()
        {
            var context = new ValidationContext();

            var result1 = BusinessRuleValidationResult.CreateErrorWithTag("Error1 message", "Rule1");
            var result2 = BusinessRuleValidationResult.CreateWarningWithTag("Warning1 message", "Rule1");

            var result3 = BusinessRuleValidationResult.CreateErrorWithTag("Error2 message", "Rule2");
            var result4 = BusinessRuleValidationResult.CreateErrorWithTag("Error3 message", "Rule2");

            context.AddBusinessRuleValidationResult(result1);
            context.AddBusinessRuleValidationResult(result2);
            context.AddBusinessRuleValidationResult(result3);
            context.AddBusinessRuleValidationResult(result4);

            ValidationContext = context;

            return base.InitializeAsync();
        }
    }
}