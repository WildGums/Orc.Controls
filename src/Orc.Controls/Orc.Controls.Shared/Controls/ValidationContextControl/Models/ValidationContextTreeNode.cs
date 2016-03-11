// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextTreeNode.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using Catel;
    using Catel.Collections;
    using Catel.Data;

    internal class ValidationContextTreeNode : ModelBase, IValidationContextTreeNode, IComparable
    {
        protected ValidationContextTreeNode()
        {
            Children = new FastObservableCollection<ValidationContextTreeNode>();
        }

        public FastObservableCollection<ValidationContextTreeNode> Children { get; }

        public string DisplayName { get; protected set; }

        [DefaultValue(false)]
        public bool IsExpanded { get; set; }

        public bool IsVisible { get; set; }

        public ValidationResultType? ResultType { get; set; }

        IEnumerable<IValidationContextTreeNode> IValidationContextTreeNode.Children => Children.OfType<IValidationContextTreeNode>();

        public virtual void ApplyFilter(bool showErrors, bool showWarnings,  string filter)
        {
            foreach (var validationContextTreeNode in Children)
            {
                validationContextTreeNode.ApplyFilter(showErrors, showWarnings, filter);
            }
        }

        public virtual int CompareTo(ValidationContextTreeNode nodeToCompare)
        {
            Argument.IsNotNull(() => nodeToCompare);

            return CultureInfo.InstalledUICulture.CompareInfo.Compare(nodeToCompare.DisplayName, DisplayName);
        }

        public int CompareTo(object obj)
        {
            return CompareTo((ValidationContextTreeNode)obj);
        }
    }
}