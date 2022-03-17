namespace Orc.Controls.Automation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using Catel.Data;
    using Orc.Automation;
    using Orc.Automation.Controls;

    [AutomatedControl(Class = typeof(Controls.ValidationContextTree))]
    public class ValidationContextTree : FrameworkElement<ValidationContextTreeModel, ValidationContextTreeMap>,
        IEnumerable<ValidationContextTagTreeItem>
    {
        public ValidationContextTree(AutomationElement element)
            : base(element)
        {
        }

        public IReadOnlyList<ValidationContextTagTreeItem> TagNodes => Map.InnerTree.ChildItems
            .Select(x => x.As<ValidationContextTagTreeItem>())
            .ToList();

        public IReadOnlyList<string> GetValidationItems(string tag, ValidationResultType type)
        {
            var innerTree = Map.InnerTree;

            var firstItem = innerTree.ChildItems.FirstOrDefault(x => x.Header?.StartsWith(tag) ?? false);
            if (firstItem is null)
            {
                return Array.Empty<string>();
            }

            var typeItem = firstItem.ChildItems.FirstOrDefault(x => x.Header?.StartsWith(type.ToString()) ?? false);
            if (typeItem is null)
            {
                return Array.Empty<string>();
            }

            return typeItem.ChildItems.Select(x => x.Header).ToList();
        }

        public IEnumerator<ValidationContextTagTreeItem> GetEnumerator()
        {
            return TagNodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
