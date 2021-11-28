namespace Orc.Automation
{
    using System.Collections.Generic;
    using System.Windows.Automation;
    using Catel;

    public static partial class AutomationElementExtensions
    {
        public static AutomationElement GetAncestor(this AutomationElement element, Condition condition)
        {
            Argument.IsNotNull(() => element);

            var treeWalker = new TreeWalker(condition);

            var isMatch = false;
            var parent = GetParent(element);
            while (!isMatch && parent is not null)
            {
                isMatch = treeWalker.GetParent(element) is not null;
                parent = GetParent(parent);
            }

            return parent;
        }

        public static IEnumerable<AutomationElement> GetAncestors(this AutomationElement element, Condition condition)
        {
            Argument.IsNotNull(() => element);

            var treeWalker = new TreeWalker(condition);

            var parent = GetParent(element);
            while (parent is not null)
            {
                if (treeWalker.GetParent(element) is not null)
                {
                    yield return parent;
                }

                parent = GetParent(parent);
            }
        }

        public static AutomationElement GetParent(this AutomationElement element, Condition condition = null)
        {
            Argument.IsNotNull(() => element);

            return condition is null ? TreeWalker.ControlViewWalker.GetParent(element) : new TreeWalker(condition).GetParent(element);
        }

        public static AutomationElement GetChild(this AutomationElement containerElement, int index)
        {
            Argument.IsNotNull(() => containerElement);

            var item = TreeWalker.ControlViewWalker.GetFirstChild(containerElement);
            var currentIndex = 0;

            while (!Equals(index, currentIndex) && item is not null)
            {
                item = TreeWalker.ControlViewWalker.GetNextSibling(item);
                currentIndex++;
            }

            return item;
        }

        public static IEnumerable<AutomationElement> GetChildElements(this AutomationElement containerElement)
        {
            Argument.IsNotNull(() => containerElement);

            var item = TreeWalker.ControlViewWalker.GetFirstChild(containerElement);

            while (item is not null)
            {
                item = TreeWalker.ControlViewWalker.GetNextSibling(item);

                yield return item;
            }
        }
    }
}
