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

    public class ValidationContextTreeNode : ModelBase, IValidationContextTreeNode, IComparable
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

        public void ApplyFilter(bool showErrors, bool showWarnings,  string filter)
        {
            foreach (var validationContextTreeNode in Children)
            {
                validationContextTreeNode.ApplyFilter(showErrors, showWarnings, filter);
            }

            if (Children.Any(x => x.IsVisible))
            {
                IsVisible = true;
                return;
            }

            var isVisible = ResultType == null;

            if (showErrors && ResultType != null && ResultType.Value == ValidationResultType.Error)
            {
                isVisible = true;
            }

            if (showWarnings && ResultType != null && ResultType.Value == ValidationResultType.Warning)
            {
                isVisible = true;
            }

            if (!isVisible)
            {
                IsVisible = false;
                return;
            }

            if (string.IsNullOrEmpty(filter))
            {
                IsVisible = true;
                return;
            }

            var culture = CultureInfo.InvariantCulture;
            IsVisible = culture.CompareInfo.IndexOf(DisplayName, filter, CompareOptions.IgnoreCase) >= 0;
        }

        public virtual int CompareTo(ValidationContextTreeNode node)
        {
            Argument.IsNotNull(() => node);

            return CultureInfo.InstalledUICulture.CompareInfo.Compare(DisplayName, node.DisplayName);
        }

        public int CompareTo(object obj)
        {
            return CompareTo((ValidationContextTreeNode)obj);
        }
    }
}