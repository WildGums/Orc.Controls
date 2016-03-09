// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResultTagNode.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Specialized;
    using System.Linq;
    using Catel.Data;

    internal class ValidationResultTagNode : ValidationContextTreeNodeBase
    {
        public ValidationResultTagNode(string tagName)
        {
            TagName = tagName;
        }

        public string TagName { get; private set; }

        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (string.Equals(e.PropertyName, nameof(DisplayName)))
            {
                return;
            }

            UpdateDisplayName();
        }

        protected override void OnPropertyObjectCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateDisplayName();

            base.OnPropertyObjectCollectionChanged(sender, e);
        }

        private void UpdateDisplayName()
        {
            var errorCount = Children.OfType<ValidationResultTypeNode>().Where(x => x.ResultType == ValidationResultType.Error)
                .SelectMany(x => x.Children).Count();

            var warningCount = Children.OfType<ValidationResultTypeNode>().Where(x => x.ResultType == ValidationResultType.Warning)
                .SelectMany(x => x.Children).Count();

            DisplayName = $"{TagName} (Errors: {errorCount}, Warnings: {warningCount})";
        }
    }
}