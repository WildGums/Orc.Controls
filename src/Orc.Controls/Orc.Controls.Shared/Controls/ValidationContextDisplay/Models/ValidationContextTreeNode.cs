// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextTreeNode.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using Catel.Collections;
    using Catel.Data;

    public class ValidationContextTreeNode : ModelBase
    {
        protected ValidationContextTreeNode()
        {
            Children = new FastObservableCollection<ValidationContextTreeNode>();
        }

        public FastObservableCollection<ValidationContextTreeNode> Children { get; }

        public string DisplayName { get; protected set; }

        public bool IsExpanded { get; set; }

        public bool IsVisible { get; set; }

        public ValidationResultType? ResultType { get; set; }

        public virtual void ApplyFilter(bool showErrors, bool showWarnings,  string filter)
        {
            foreach (var validationContextTreeNode in Children)
            {
                validationContextTreeNode.ApplyFilter(showErrors, showWarnings, filter);
            }
        }
    }
}