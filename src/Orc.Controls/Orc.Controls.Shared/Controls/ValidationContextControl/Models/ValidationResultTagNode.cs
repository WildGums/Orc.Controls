// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationResultTagNode.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Controls
{
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using Catel;
    using Catel.Data;
    using Catel.Reflection;

    internal class ValidationResultTagNode : ValidationContextTreeNode
    {
        public ValidationResultTagNode(object tag)
        {
            Tag = tag;
            TagName = ExtractRuleName(tag);
        }

        public object Tag { get; }

        public string TagName { get; }

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

        private static string ExtractRuleName(object value)
        {
            if (ReferenceEquals(value, null))
            {
                return "Misc";
            }

            var stringValue = value as string;
            if (!string.IsNullOrWhiteSpace(stringValue))
            {
                return stringValue;
            }

            var type = value.GetType();
            var nameProperty = type.GetPropertyEx("Name");
            if (nameProperty != null)
            {
                return (string) nameProperty.GetValue(value, new object[0]);
            }

            return value.ToString();
        }

        private void UpdateDisplayName()
        {
            var errorCount = Children.OfType<ValidationResultTypeNode>().Where(x => x.ResultType == ValidationResultType.Error)
                .SelectMany(x => x.Children).Count();

            var warningCount = Children.OfType<ValidationResultTypeNode>().Where(x => x.ResultType == ValidationResultType.Warning)
                .SelectMany(x => x.Children).Count();

            DisplayName = $"{TagName} (Errors: {errorCount}, Warnings: {warningCount})";
        }

        public override void ApplyFilter(bool showErrors, bool showWarnings, string filter)
        {
            base.ApplyFilter(showErrors, showWarnings, filter);

            IsVisible = Children.Any(x => x.IsVisible);
        }

        public override int CompareTo(ValidationContextTreeNode nodeToCompare)
        {
            Argument.IsNotNull(() => nodeToCompare);

            var node = (ValidationResultTagNode) nodeToCompare;
            if (string.Equals(TagName, "Misc") || !string.Equals(node.TagName, "Misc"))
            {
                return 1;
            }

            if (!string.Equals(TagName, "Misc") || string.Equals(node.TagName, "Misc"))
            {
                return -1;
            }
            
            return CultureInfo.InstalledUICulture.CompareInfo.Compare(TagName, node.TagName);
        }
    }
}