// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationContextTreeNode.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Catel;
    using Catel.Collections;
    using Catel.Data;

    public class ValidationContextTreeNode : ChildAwareModelBase, IValidationContextTreeNode, IComparable
    {
        #region Constructors
        protected ValidationContextTreeNode(bool isExpanded)
        {
            Children = new FastObservableCollection<ValidationContextTreeNode>();
            IsExpanded = isExpanded;
        }
        #endregion

        #region Properties
        public FastObservableCollection<ValidationContextTreeNode> Children { get; }
        public string DisplayName { get; protected set; }
        public bool IsExpanded { get; set; }
        public bool IsVisible { get; set; }

        public ValidationResultType? ResultType { get; set; }
        IEnumerable<IValidationContextTreeNode> IValidationContextTreeNode.Children => Children.OfType<IValidationContextTreeNode>();
        #endregion

        #region IComparable Members
        public int CompareTo(object obj)
        {
            return CompareTo((ValidationContextTreeNode)obj);
        }
        #endregion

        #region Methods
        public void ApplyFilter(bool showErrors, bool showWarnings, string filter)
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
        #endregion
    }
}
